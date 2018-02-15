using System;
using EasyBudget.Business;

namespace EasyBudget.Business
{
    public interface IDataServiceHelper
    {
        string DbFilePath { get; }
    }
}
