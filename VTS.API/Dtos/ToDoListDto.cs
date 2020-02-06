using System;

namespace VTS.API.Dtos
{
    public class ToDoListDto
    {
        public ToDoListDto()
        {
            CreatedDate = DateTime.Now;
        }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}