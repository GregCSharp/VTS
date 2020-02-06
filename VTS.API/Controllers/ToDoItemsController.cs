using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VTS.API.Data;
using VTS.API.Dtos;

namespace VTS.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/lists/{listId}/[controller]")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        public IVtsRepository _repo { get; }
        public IMapper _mapper { get; }
        public ToDoItemsController(IVtsRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetItems(int userId, int listId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var listFromRepo = await _repo.GetToDoList(listId);
            if (listFromRepo == null)
                return BadRequest("Could not find the to-do list");

            if (listFromRepo.UserId != userId)
                return Unauthorized();

            if (listFromRepo.ToDoItems.Any())
                return Ok(listFromRepo.ToDoItems);
            else
                return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(int userId, int listId, ToDoItemForCreateDto toDoItemForCreateDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var listFromRepo = await _repo.GetToDoList(listId);
            if (listFromRepo == null)
                return BadRequest("Could not find the to-do list");

            if (listFromRepo.UserId != userId)
                return Unauthorized();

            var itemToCreate = new ToDoItem();
            _mapper.Map(toDoItemForCreateDto, itemToCreate);

            listFromRepo.ToDoItems.Add(itemToCreate);

            if (await _repo.SaveAll())
            {
                return CreatedAtRoute("", new { userId = userId, listId = listFromRepo.Id, id = itemToCreate.Id },
                    itemToCreate);
            }

            return BadRequest("Could not Create to-do item");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int userId, int listId, int id)
        {
            var v = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var listFromRepo = await _repo.GetToDoList(listId);
            if (listFromRepo == null)
                return BadRequest("Could not find the to-do list");

            if (listFromRepo.UserId != userId)
                return Unauthorized();

            var item = listFromRepo.ToDoItems.FirstOrDefault(i => i.Id == id);
            if (item == null)
                return BadRequest("Could not retrieve the to-do item");

            return Ok(item);
        }
    
        [HttpPut("{id}")]
        public async Task<IActionResult> EditItem(int userId, int listId, int id, ToDoItemForEditDto toDoItemForEdit)
        {
            var v = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var listFromRepo = await _repo.GetToDoList(listId);
            if (listFromRepo == null)
                return BadRequest("Could not retrieve the list of the to-do item to edit");
            if (listFromRepo.UserId != userId)
                return Unauthorized();

            var itemFromRepo = await _repo.GetToDoItem(id);
            if (itemFromRepo == null)
                return BadRequest("Could not retrieve the to-do item to edit");
            if (itemFromRepo.ToDoListId != listId)
                return BadRequest("Could not retrieve the to-do item from the given list to edit");

            _mapper.Map(toDoItemForEdit, itemFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Updating to-do list {toDoItemForEdit.Name} failed on save");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int userId, int listId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var listFromRepo = await _repo.GetToDoList(listId);
            if (listFromRepo == null)
                return BadRequest("Could not retrieve the list of the to-do item to delete");
            if (listFromRepo.UserId != userId)
                return Unauthorized();

            var itemFromRepo = await _repo.GetToDoItem(id);
            if (itemFromRepo == null)
                return BadRequest("Could not retrieve the to-do item to delete");
            if (itemFromRepo.ToDoListId != listId)
                return BadRequest("Could not retrieve the to-do item from the given list to delete");

            _repo.Delete(itemFromRepo);
            
            if (await _repo.SaveAll())
                return NoContent();
            
            throw new Exception($"Deleting to-do list {itemFromRepo.Name} failed");
        }

        [HttpPost("{id}/setCompleted")]
        public async Task<IActionResult> SetItemCompleted(int userId, int listId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var listFromRepo = await _repo.GetToDoList(listId);
            if (listFromRepo == null)
                return BadRequest("Could not retrieve the list of the to-do item to delete");
            if (listFromRepo.UserId != userId)
                return Unauthorized();

            var itemFromRepo = await _repo.GetToDoItem(id);
            if (itemFromRepo == null)
                return BadRequest("Could not retrieve the to-do item to delete");
            if (itemFromRepo.ToDoListId != listId)
                return BadRequest("Could not retrieve the to-do item from the given list to delete");

            itemFromRepo.IsComplete = true;
            itemFromRepo.CompleteDate = DateTime.Now;

            //if all items from list are now completed, set list as completed
            if (!listFromRepo.ToDoItems.Any(i => !i.IsComplete))
            {
                listFromRepo.IsComplete = true;
                listFromRepo.CompletedDate = DateTime.Now;
            }

            if (await _repo.SaveAll())
                return NoContent();


            return BadRequest("Could not set the item as completed");
        }

        [HttpPost("{id}/setIncomplete")]
        public async Task<IActionResult> SetItemIncomplete(int userId, int listId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var listFromRepo = await _repo.GetToDoList(listId);
            if (listFromRepo == null)
                return BadRequest("Could not retrieve the list of the to-do item to set as incomplete");
            if (listFromRepo.UserId != userId)
                return Unauthorized();

            var itemFromRepo = await _repo.GetToDoItem(id);
            if (itemFromRepo == null)
                return BadRequest("Could not retrieve the to-do item to delete");
            if (itemFromRepo.ToDoListId != listId)
                return BadRequest("Could not retrieve the to-do item from the given list to set as incomplete");

            itemFromRepo.IsComplete = false;
            itemFromRepo.CompleteDate = null;

            //set list as incomplete as well
            listFromRepo.IsComplete = false;
            listFromRepo.CompletedDate = null;

            if (await _repo.SaveAll())
                return NoContent();

            return BadRequest("Could not set the item as incompleted");
        }
    }

}