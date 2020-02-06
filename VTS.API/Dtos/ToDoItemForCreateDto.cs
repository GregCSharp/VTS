using System;

namespace VTS.API.Dtos
{
    public class ToDoItemForCreateDto
    {
        public ToDoItemForCreateDto()
        {
            CreatedDate = DateTime.Now;
        }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}