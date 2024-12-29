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

        /// <summary>
        /// Gets true with a chance of parameter.
        /// </summary>
        /// <param name="trueChance"></param>
        /// <returns></returns>
        public static bool RandomBool(double trueChance) 
        {
            return random.NextDouble() < trueChance;
        }

        public static double RandomDouble()
        {
            return random.NextDouble();
        }
    }
}
