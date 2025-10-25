using AutoMapper;
using DomainAnimal.Entities;
using ZooApi.DTO;

namespace ZooApi.Mapping
{
    public class AnimalProfile : Profile
    {
        public AnimalProfile()
        {
            CreateMap<Animal, AnimalDto>();
        }
    }

}
