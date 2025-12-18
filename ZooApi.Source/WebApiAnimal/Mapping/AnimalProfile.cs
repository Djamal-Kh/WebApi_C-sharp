using ApplicationAnimal.Common.DTO;
using ApplicationAnimal.DTO;
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
            CreateMap<Animal, AddAnimalResponseDto>();
            CreateMap<EnumAnimalTypeCountDto, GetNumberAnimalsTypeResponseDto>();
        }
    }

}
