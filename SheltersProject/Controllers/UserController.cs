
using BLL.Repositories;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace SheltersProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        // GET: api/User
        [HttpGet]
        public ActionResult<List<UserProfileDTO>> GetAll()
        {
            var users = _repository.GetAll();
            return Ok(users);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public ActionResult<UserProfileDTO> GetById(int id)
        {
            var user = _repository.GetById(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/User
        [HttpPost]
        public ActionResult<UserProfileDTO> Add(
            [FromBody] UserProfileDTO dto)
        {
            var addedUser = _repository.Add(dto);
            return Ok(addedUser);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public ActionResult<UserProfileDTO> Update(
            int id,
            [FromBody] UserProfileDTO dto)
        {
            var updatedUser = _repository.Update(id, dto);

            if (updatedUser == null)
                return NotFound();

            return Ok(updatedUser);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool deleted = _repository.Delete(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
