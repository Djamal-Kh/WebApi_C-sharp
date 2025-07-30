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
            var ExistenceAnimal = await animalRepository.GetAnimalByIdAsync(id);

            if (ExistenceAnimal == null)
                throw new Exception("Animal Not Found");

            return true;
        }
    }
}
