using AutoMapper;
using DomainAnimal.Entities;

namespace ZooApi.Mapping
{
    public class CreateAnimalProfile : Profile
    {
        public CreateAnimalProfile()
        {
            CreateMap<Animal, CreateAnimalProfile>();
        }
    }
}
