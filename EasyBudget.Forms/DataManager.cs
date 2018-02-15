using System;
using EasyBudget.Business;

namespace EasyBudget.Forms
{
    public class DataManager
    {
        public static DataManager Instance { get; private set; }

        IDataServiceHelper provider;

        public DataManager(IDataServiceHelper service)
        {
            provider = service;
        }
    }
}
