using AutoMapper;
using VTS.API.Data;
using VTS.API.Dtos;

namespace VTS.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ToDoListForEditDto, ToDoList>();
            CreateMap<ToDoItemForCreateDto, ToDoItem>();
            CreateMap<ToDoItemForEditDto, ToDoItem>();
        }
    }
}