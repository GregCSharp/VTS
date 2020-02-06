using System.Collections.Generic;
using System.Threading.Tasks;

namespace VTS.API.Data
{
    public interface IVtsRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<User> GetUser(int userId);
         Task<IEnumerable<ToDoList>> GetToDoListsForUser(int userId);
         Task<IEnumerable<ToDoItem>> GetToDoItems(int toDoListId);
         Task<ToDoList> GetToDoList(int id);
         Task<ToDoItem> GetToDoItem(int id);

    }
}