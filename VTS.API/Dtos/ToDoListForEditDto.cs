using System;
using System.Collections.Generic;
using VTS.API.Data;

namespace VTS.API.Dtos
{
    public class ToDoListForEditDto
    {
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}