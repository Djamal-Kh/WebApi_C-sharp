using AutoMapper;
using DataAccess;

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
