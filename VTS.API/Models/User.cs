using System.Collections.Generic;

namespace VTS.API.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ToDoList> ToDoLists { get; set; }

    }
}