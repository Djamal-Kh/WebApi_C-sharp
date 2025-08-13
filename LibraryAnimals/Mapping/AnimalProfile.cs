using AutoMapper; 
using DataAccess;
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
