using System;
using System.Collections.Generic;
using fireBwall.Logging;
using System.Text;
using System.Threading;

namespace fireBwall.Filters.NDIS
{
    public class WinpkFilterList : INDISFilterList
    {
        #region Variables

        //object padlock = new object();
        List<WinpkFilter> currentAdapters = new List<WinpkFilter>();
        List<WinpkFilter> newAdapters = new List<WinpkFilter>();
        IntPtr hNdisapi = IntPtr.Zero;
        bool isNdisFilterDriverOpen = false;
        Thread updateThread = null;

        #endregion

        #region Functions

        public void ShutdownAll()
        {
            //lock (padlock)
            //{
                CloseAllInterfaces();
                CloseDriver();
            //}
        }

        public void CloseAllInterfaces()
        {
            updateThread.Abort();
            foreach (WinpkFilter na in currentAdapters)
            {
                na.StopProcessing();
            }
        }

        public void CloseDriver()
        {
            Ndisapi.CloseFilterDriver(hNdisapi);
            isNdisFilterDriverOpen = false;
        }

        public void OpenDriver()
        {
            if (hNdisapi != IntPtr.Zero)
            {
                LogCenter.Instance.LogDebugMessage("Bad state was found, attempting to open the NDIS Filter Driver while the IntPtr != IntPtr.Zero, continuing");
            }

            hNdisapi = Ndisapi.OpenFilterDriver(Ndisapi.NDISRD_DRIVER_NAME);
            TCP_AdapterList adList = new TCP_AdapterList();
            Ndisapi.GetTcpipBoundAdaptersInfo(hNdisapi, ref adList);
            if (adList.m_nAdapterCount == 0)
            {
                LogCenter.Instance.LogDebugMessage("No adapters found on this driver interface");
                return;
            }
            isNdisFilterDriverOpen = true;
            if (updateThread == null)
            {
                updateThread = new Thread(NewAdaptersLoop);
                updateThread.Name = "WinpkFilterList New Adapters Loop";
                updateThread.Start();
            }
        }

        void NewAdaptersLoop()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    if (!isNdisFilterDriverOpen)
                    {
                        OpenDriver();
                    }
                    TCP_AdapterList adList = new TCP_AdapterList();
                    Ndisapi.GetTcpipBoundAdaptersInfo(hNdisapi, ref adList);
                    for (int x = 0; x < currentAdapters.Count; x++)
                    {
                        for (int y = 0; y < adList.m_nAdapterCount; y++)
                        {
                            if (adList.m_nAdapterHandle[y] == currentAdapters[x].adapterHandle)
                            {
                                currentAdapters[x].UpdateNetworkInterface(Encoding.ASCII.GetString(adList.m_szAdapterNameList, y * 256, 256));
                            }
                        }
                    }
                    for (int x = 0; x < adList.m_nAdapterCount; x++)
                    {
                        bool found = false;
                        for (int y = 0; y < currentAdapters.Count; y++)
                        {
                            if (adList.m_nAdapterHandle[x] == currentAdapters[y].adapterHandle)
                                found = true;
                        }
                        if (!found)
                        {
                            WinpkFilter newAdapter = new WinpkFilter(hNdisapi, adList.m_nAdapterHandle[x], Encoding.ASCII.GetString(adList.m_szAdapterNameList, x * 256, 256));
                            if (newAdapter.GetAdapterInformation() != null && !string.IsNullOrEmpty(newAdapter.GetAdapterInformation().Name))
                            {
                                //lock (newAdapters)
                                //{
                                    newAdapters.Add(newAdapter);
                                //}
                            }
                        }
                    }
                }
            }
            catch (ThreadAbortException) { }
        }

        void UpdateCurrentAdapters()
        {
            bool succeeded = false;
            while (!succeeded)
            {
                if (!isNdisFilterDriverOpen)
                    OpenDriver();
                TCP_AdapterList adList = new TCP_AdapterList();
                Ndisapi.GetTcpipBoundAdaptersInfo(hNdisapi, ref adList);
                List<WinpkFilter> tempList = new List<WinpkFilter>();

                //Populate with current adapters
                List<WinpkFilter> notFound = new List<WinpkFilter>();
                for (int x = 0; x < currentAdapters.Count; x++)
                {
                    bool found = false;
                    for (int y = 0; y < adList.m_nAdapterCount; y++)
                    {
                        if (adList.m_nAdapterHandle[y] == currentAdapters[x].adapterHandle)
                        {
                            currentAdapters[x].UpdateNetworkInterface(Encoding.ASCII.GetString(adList.m_szAdapterNameList, y * 256, 256));
                            tempList.Add(currentAdapters[x]);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        notFound.Add(currentAdapters[x]);
                    }
                }

                //Deal with no longer existant adapters
                for (int x = 0; x < notFound.Count; x++)
                {
                    notFound[x].StopProcessing();
                }

                //Adding any new adapters
                for (int x = 0; x < adList.m_nAdapterCount; x++)
                {
                    bool found = false;
                    for (int y = 0; y < currentAdapters.Count; y++)
                    {
                        if (adList.m_nAdapterHandle[x] == currentAdapters[y].adapterHandle)
                            found = true;
                    }
                    if (!found)
                    {
                        WinpkFilter newAdapter = new WinpkFilter(hNdisapi, adList.m_nAdapterHandle[x], Encoding.ASCII.GetString(adList.m_szAdapterNameList, x * 256, 256));
                        if (newAdapter.GetAdapterInformation() != null && !string.IsNullOrEmpty(newAdapter.GetAdapterInformation().Name))
                        {
                            tempList.Add(newAdapter);
                        }
                    }
                }

                currentAdapters = new List<WinpkFilter>(tempList);
                succeeded = true;
            }
        }

        public INDISFilter[] GetAllAdapters()
        {
            //lock (padlock)
            //{
                UpdateCurrentAdapters();
                return new List<WinpkFilter>(currentAdapters).ToArray();
            //}
        }

        public INDISFilter[] GetNewAdapters()
        {
            List<WinpkFilter> temp = new List<WinpkFilter>();
            //lock (newAdapters)
            //{
                temp = new List<WinpkFilter>(newAdapters);                
                newAdapters.Clear();
            //}
            currentAdapters.AddRange(temp);
            return temp.ToArray();
        }

        #endregion
    }
}
