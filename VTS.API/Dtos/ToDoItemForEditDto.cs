using System;

namespace VTS.API.Dtos
{
    public class ToDoItemForEditDto
    {
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}