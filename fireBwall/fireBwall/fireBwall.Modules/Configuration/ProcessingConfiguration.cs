using System;
using System.Collections.Generic;
using fireBwall.Filters.NDIS;
using System.Text;

namespace fireBwall.Configuration
{
    public class ProcessingConfiguration
    {
        #region ConcurrentSingleton

        private static volatile ProcessingConfiguration instance;
        //private static object syncRoot = new Object();

        private ProcessingConfiguration() 
        {
            NDISFilterList = new WinpkFilterList();
            NDISFilterList.OpenDriver();
            NDISFilterList.GetAllAdapters();           
        }

        /// <summary>
        /// Makes sure that the creation of a new GeneralConfiguration is threadsafe
        /// </summary>
        public static ProcessingConfiguration Instance
        {
            get 
            {
                //lock (syncRoot) 
                //{
                    if (instance == null)
                        instance = new ProcessingConfiguration();
                //}
                return instance;               
            }
        }

        #endregion

        #region Members

        public INDISFilterList NDISFilterList;

        #endregion
    }
}
