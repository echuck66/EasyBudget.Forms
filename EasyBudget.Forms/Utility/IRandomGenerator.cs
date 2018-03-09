using System;
namespace EasyBudget.Forms.Utility
{
    public interface IRandomGenerator
    {
        Random GetRandom(int? seed);
    }
}
