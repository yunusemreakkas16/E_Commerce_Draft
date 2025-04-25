using E_Commerce_Draft.API.Models.Domain;
using E_Commerce_Draft.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static E_Commerce_Draft.API.Models.Domain.User;

namespace E_Commerce_Draft.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<ActionResult<object>> CreateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest(new { MessageId = -2, MessageDescription = "User data is required." });

            var (messageId, messageDescription, newUser) = await userRepository.CreateUserAsync(user);

            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });

            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });

            return Ok(new { MessageId = messageId, MessageDescription = messageDescription, User = newUser });
        }

        [HttpPost]
        [Route("UserDetail")]
        public async Task<ActionResult<object>> GetUserById([FromBody] UserDetailParamModel detailParamModel)
        {
            var (messageId, messageDescription, userDetail) = await userRepository.GetUserByIdAsync(detailParamModel.ID);

            if (messageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = messageDescription });

            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });

            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });

            return Ok(new { MessageId = messageId, MessageDescription = messageDescription, UserDetail = userDetail });
        }


        [HttpPost]
        [Route("UserList")]
        public async Task<ActionResult<object>> GetAllUsers()
        {
            var (messageId, messageDescription, users) = await userRepository.GetAllUsersAsync();

            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });

            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });

            return Ok(new { MessageId = messageId, MessageDescription = messageDescription, Users = users });
        }

        [HttpPost]
        [Route("UpdateUser")]
        public async Task<ActionResult<object>> UpdateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest(new { MessageId = -2, MessageDescription = "User data is required." });

            var (messageId, messageDescription, updatedUser) = await userRepository.UpdateUserAsync(user);

            if (messageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = messageDescription });

            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });

            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });

            return Ok(new { MessageId = messageId, MessageDescription = messageDescription, UpdatedUser = updatedUser });
        }
    }
}
