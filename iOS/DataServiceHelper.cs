using System;
using EasyBudget.Business;
using EasyBudget.Forms;

namespace EasyBudget.iOS
{
    public class DataServiceHelper : IDataServiceHelper
    {
        const string dbFileName = "dbEasyBudget.sqlite";

        public string DbFilePath 
        {
            get
            {
                return FileAccessHelper.GetDataFilePath(dbFileName);
            }
        }
    }
}
