using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fireBwall.Filters.NDIS
{
    public interface INDISFilterList
    {
        void ShutdownAll();
        void CloseAllInterfaces();
        void CloseDriver();
        void OpenDriver();
        INDISFilter[] GetAllAdapters();
        INDISFilter[] GetNewAdapters();
    }
}
