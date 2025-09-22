using DomainAnimal.Entities;
using DomainAnimal.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace ApplicationAnimal.Services
{

    public class AnimalService : IAnimalService
    {
        private readonly ILogger<AnimalService> _logger;
        private readonly IAnimalRepository _animalRepository;

        public AnimalService(IAnimalRepository animalRepository, ILogger<AnimalService> logger)
        {
            _logger = logger;
            _animalRepository = animalRepository;
        }

        public async Task<Animal> CreateAnimalAsync(AnimalType animalType, string NameOfAnimal, CancellationToken cancellationToken = default)
        {
            /* Была мысль использовать здесь GOF паттерн "Фабричный метод" для практики, но в данном контексте он будет здесь лишним 
              т.к. код становится слишком громоздким и менее читаемым */
            _logger.LogInformation("Попытка создать животное с Name: {Name}", NameOfAnimal);
            bool ExistsName = await _animalRepository.ExistsByName(NameOfAnimal);
            if (ExistsName)
            {
                _logger.LogWarning("Попытка создать животное с Name: {Name} ПРОВАЛЕНА !", NameOfAnimal);
                throw new ValidationException();
            }

            Animal newAnimal;
            switch (animalType)
            {
                case AnimalType.Lion:
                    newAnimal = new Lion(NameOfAnimal);
                    break;

                case AnimalType.Monkey:
                    newAnimal = new Monkey(NameOfAnimal);
                    break;

                default: throw new ArgumentException();
            }

            await _animalRepository.CreateAnimalAsync(newAnimal);
            return newAnimal;
        }


        public async Task<List<Animal>> GetAllAnimalsAsync(CancellationToken cancellationToken = default)
        {
            var animal = await _animalRepository.GetAllAnimalsAsync();
            return animal;
        }


        public async Task<Animal> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var animal = await _animalRepository.GetAnimalByIdAsync(id);
            return animal;
        }


        // Тут у меня вызываемый репозиторий "умный" 
        public async Task<string> FeedAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _animalRepository.FeedAnimalAsync(id);
            return result;
        }


        /*Здесь же репозиторий "тупой". 
          Сделал я это чтобы сравнить два подхода.
          Сейчас я понимаю, что к чистой архитектуре больше подходит подход
          с "тупым" репозиторием*/
        public async Task<string> DeleteAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            var animal = await _animalRepository.GetAnimalByIdAsync(id);

            await _animalRepository.DeleteAnimalAsync(animal);
            string result = $"Животное с id = {id} было удалено";
            return result;
        }
    }
}
