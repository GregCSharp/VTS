using System;
using System.Collections.Generic;

namespace VTS.API.Data
{
    public class ToDoList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int UserId { get; set; }
        public ICollection<ToDoItem> ToDoItems { get; set; }

    }
}