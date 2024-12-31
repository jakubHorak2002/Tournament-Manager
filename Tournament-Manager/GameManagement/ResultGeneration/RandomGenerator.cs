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

        public static int RandomInInterval(int min, int max)
        {
            return random.Next(min, max);
        }

        public static int GetRandomFromAvarage(double avarage)
        {
            int score = 0;

            double L = Math.Exp(-avarage); // e^(-lambda)
            double p = 1;

            do
            {
                score++;
                p *= RandomGenerator.RandomDouble(); // Multiply by a new random number
            } while (p > L);

            return score - 1; // Subtract 1 because k starts at 1
        }
    }
}
