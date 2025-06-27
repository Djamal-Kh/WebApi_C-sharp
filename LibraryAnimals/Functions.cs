using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAnimals
{
    public class Functions
    {
        public static async Task<bool> CheckOnNull<T>(List<T> animal, int id)
        {
            if (animal.Count == 0 || animal.Count < id)
            {
                throw new Exception("Incorrect Data !");
            }

            return true;

        }

        public static async Task<bool> CheckOnNull<T>(List<T> animal)
        {
            if (animal.Count == 0)
            {
                throw new Exception("Incorrect Data !");
            }

            return true;

        }
    }
}
