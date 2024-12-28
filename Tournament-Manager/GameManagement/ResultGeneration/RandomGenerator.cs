using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManagement.ResultGeneration
{
    public static class RandomGenerator
    {
        private static Random random = new Random();

        public static bool RandomBool(double trueChance) 
        {
            return random.NextDouble() < trueChance;
        }
    }
}
