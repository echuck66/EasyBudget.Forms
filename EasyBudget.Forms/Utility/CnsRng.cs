using System;
namespace EasyBudget.Forms.Utility
{
    public class RandomGenerator
    {
        public RandomGenerator()
        {
        }

        public Random GetRandom(int? seed)
        {
            if (seed == null)
                return new Random();
            else
                return new Random((int)seed);
        }
    }
}
