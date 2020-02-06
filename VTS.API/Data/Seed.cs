using System;
using System.Collections.Generic;
using System.Linq;

namespace VTS.API.Data
{
    public static class Seed
    {
        public static void SeedData(DataContext context)
        {
            if (!context.Users.Any())
            {
                #region items
                var item1 = new ToDoItem
                {
                    Name = "Bananas",
                    IsComplete = false,
                    CreatedDate = DateTime.Now,
                    CompleteDate = null
                };
                var item2 = new ToDoItem
                {
                    Name = "Oranges",
                    IsComplete = false,
                    CreatedDate = DateTime.Now,
                    CompleteDate = null
                };
                var item3 = new ToDoItem
                {
                    Name = "Bread",
                    IsComplete = false,
                    CreatedDate = DateTime.Now,
                    CompleteDate = null
                };
                
                var item4 = new ToDoItem
                {
                    Name = "Review company website",
                    IsComplete = false,
                    CreatedDate = DateTime.Now,
                    CompleteDate = null
                };
                
                var item5 = new ToDoItem
                {
                    Name = "Print extra resume",
                    IsComplete = false,
                    CreatedDate = DateTime.Now,
                    CompleteDate = null
                };
                
                var item6 = new ToDoItem
                {
                    Name = "Clean desk",
                    IsComplete = false,
                    CreatedDate = DateTime.Now,
                    CompleteDate = null
                };
                
                var item7 = new ToDoItem
                {
                    Name = "Sort papers",
                    IsComplete = false,
                    CreatedDate = DateTime.Now,
                    CompleteDate = null
                };
                
                var item8 = new ToDoItem
                {
                    Name = "Review candidates LinkedIn",
                    IsComplete = false,
                    CreatedDate = DateTime.Now,
                    CompleteDate = null
                };
                #endregion

                #region lists
                var list1 = new ToDoList 
                {
                    Name = "Groceries",
                    IsComplete = false,
                    CreatedDate = DateTime.Now,
                    CompletedDate = null,
                    ToDoItems = new List<ToDoItem> { item1, item2, item3 }
                };

                var list2 = new ToDoList 
                {
                    Name = "Prepare for interview",
                    IsComplete = false,
                    CreatedDate = DateTime.Now,
                    CompletedDate = null,
                    ToDoItems = new List<ToDoItem> { item4, item5 }
                };

                var list3 = new ToDoList 
                {
                    Name = "Clean office",
                    IsComplete = false,
                    CreatedDate = DateTime.Now,
                    CompletedDate = null,
                    ToDoItems = new List<ToDoItem> { item6, item7 }
                };

                var list4 = new ToDoList 
                {
                    Name = "Prepare to hire",
                    IsComplete = false,
                    CreatedDate = DateTime.Now,
                    CompletedDate = null,
                    ToDoItems = new List<ToDoItem> { item8 }
                };
                #endregion

                var userg = new User();
                userg.Name = "Gregory";
                userg.ToDoLists = new List<ToDoList> { list1, list2 };
                context.Users.Add(userg);

                var userb = new User();
                userb.Name = "Brian";
                userb.ToDoLists = new List<ToDoList> { list3, list4 };
                context.Users.Add(userb);

                context.SaveChanges();
            }
        }
    }
}