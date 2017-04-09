using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Engine
{
    public class RandomNumberGenerator
    {
        private static Random generator = new Random();
        
        public RandomNumberGenerator()
        {

        }

      
    }  public  int NumberBetween(int minimumValue, int maximumValue)
        {
            return generator.Next(minimumValue, maximumValue + 1);
        }
       
}
