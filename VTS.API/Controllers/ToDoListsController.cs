using System;
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
    [Route("api/users/{userId}/lists")]
    [ApiController]
    public class ToDoListsController : ControllerBase
    {
        public IVtsRepository _repo { get; }
        public IMapper _mapper { get; }
        public ToDoListsController(IVtsRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToDoList(int userId, ToDoListDto toDolistDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var toDoListToCreate = new ToDoList
            {
                Name = toDolistDto.Name,
                CreatedDate = toDolistDto.CreatedDate
            };

            var userFromRepo = await _repo.GetUser(userId);
            userFromRepo.ToDoLists.Add(toDoListToCreate);

            if (await _repo.SaveAll())
            {
                return CreatedAtRoute("", new { userId = userId, id = toDoListToCreate.Id },
                    toDoListToCreate);
            }

            return BadRequest("Could not Create to-do list");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetToDoList(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var toDoList = await _repo.GetToDoList(id);
            if (toDoList == null)
                return BadRequest("Could not retrieve the to-do list");

            if (toDoList.UserId != userId)
                return Unauthorized();

            return Ok(toDoList);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditToDoList(int userId, int id, ToDoListForEditDto toDoListForEditDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);
            var toDoList = userFromRepo.ToDoLists.FirstOrDefault(l => l.Id == id);
            if (toDoList == null)
                return BadRequest("Could not retrieve the to-do list");

            _mapper.Map(toDoListForEditDto, toDoList);
            
            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Updating to-do list {toDoList.Name} failed on save");
        }

        [HttpGet]
        public async Task<IActionResult> GetToDoList(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var listsfromRepo = await _repo.GetToDoListsForUser(userId);

            if (!listsfromRepo.Any())
                return BadRequest("You have no to do list yet");

            return Ok(listsfromRepo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoList(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var listFromRepo = await _repo.GetToDoList(id);

            if (listFromRepo == null)
                return BadRequest("Could not delete the to-do list");

            if (listFromRepo.UserId != userId) //list does not belong to user
                return Unauthorized();

            _repo.Delete(listFromRepo);
            
            if (await _repo.SaveAll())
                return NoContent();
            
            throw new Exception($"Deleting to-do list {listFromRepo.Name} failed");
        }
    }
}