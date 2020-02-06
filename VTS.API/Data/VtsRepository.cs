using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VTS.API.Data
{
    public class VtsRepository : IVtsRepository
    {
        public DataContext _context { get; }
        public VtsRepository(DataContext context)
        {
            this._context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<ToDoItem>> GetToDoItems(int toDoListId)
        {
            var items = await _context.ToDoItems.Where(i => i.ToDoListId == toDoListId).ToListAsync();
            return items;
        }

        public async Task<IEnumerable<ToDoList>> GetToDoListsForUser(int userId)
        {
            var lists = await _context.ToDoLists.Where(l => l.UserId == userId).ToListAsync();
            return lists;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUser(int userId)
        {
            var user = await _context.Users.Include(u => u.ToDoLists).FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }

        public async Task<ToDoList> GetToDoList(int id)
        {
            var list = await _context.ToDoLists.Include( l => l.ToDoItems).FirstOrDefaultAsync(l => l.Id == id);
            return list;
        }
        
        public async Task<ToDoItem> GetToDoItem(int id)
        {
            var item = await _context.ToDoItems.FirstOrDefaultAsync(i => i.Id == id);
            return item;
        }
    }
}