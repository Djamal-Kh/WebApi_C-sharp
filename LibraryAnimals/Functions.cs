using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace LibraryAnimals
{
    public class Functions
    {
        public static async Task<bool> CheckOnNull<T>(IAnimalRepository animalRepository, int id)
        {
            var animal = await animalRepository.GetAllAnimalsAsync();  
            if (animal.Count == 0)
            {
                throw new Exception("Incorrect Data !");
            }

            return true;

        }

        public static async Task<bool> CheckOnNull<T>(IAnimalRepository animalRepository)
        {
            var animal = await animalRepository.GetAllAnimalsAsync();
            if (animal.Count == 0)
            {
                throw new Exception("Incorrect Data !");
            }

            return true;

        }
    }
}
