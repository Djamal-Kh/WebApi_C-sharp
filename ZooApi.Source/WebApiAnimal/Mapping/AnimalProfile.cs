using AutoMapper;
using DomainAnimal.Entities;
using WebApiAnimal.DTO;
using ZooApi.DTO;

namespace ZooApi.Mapping
{
    public class AnimalProfile : Profile
    {
        public AnimalProfile()
        {
            CreateMap<Animal, AnimalResponseDto>();
            CreateMap<Animal, CreateAnimalResponseDto>();
        }
    }

}
