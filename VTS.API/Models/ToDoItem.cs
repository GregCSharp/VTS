using System;

namespace VTS.API.Data
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public int ToDoListId { get; set; }
    }
}