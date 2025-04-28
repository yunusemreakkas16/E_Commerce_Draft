using E_Commerce_Draft.API.Models.Domain;
using E_Commerce_Draft.API.Repositories;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        [Route("AddUser")]
        public async Task<ActionResult<UserResponseModel>> CreateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest(new { MessageId = -2, MessageDescription = "User data is required." });

            var userResponseModel = await userRepository.CreateUserAsync(user);

            if (userResponseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = userResponseModel.MessageDescription });

            if (userResponseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = userResponseModel.MessageDescription });

            return Ok(new { MessageId = userResponseModel.MessageId, MessageDescription = userResponseModel.MessageDescription, User = userResponseModel.User });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("UserDetail")]
        public async Task<ActionResult<UserResponseModel>> GetUserById([FromBody] UserDetailParamModel detailParamModel)
        {
            var userResponseModel = await userRepository.GetUserByIdAsync(detailParamModel.ID);

            if (userResponseModel.MessageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = userResponseModel.MessageDescription });

            if (userResponseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = userResponseModel.MessageDescription });

            if (userResponseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = userResponseModel.MessageDescription });

            return Ok(new { MessageId = userResponseModel.MessageId, MessageDescription = userResponseModel.MessageDescription, UserDetail = userResponseModel.User });
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("UserList")]
        public async Task<ActionResult<UserListResponseModel>> GetAllUsers()
        {
            var userListResponseModel = await userRepository.GetAllUsersAsync();

            if (userListResponseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = userListResponseModel.MessageDescription });

            if (userListResponseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = userListResponseModel.MessageDescription });

            return Ok(new { MessageId = userListResponseModel.MessageId, MessageDescription = userListResponseModel.MessageDescription, Users = userListResponseModel.Users });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("UpdateUser")]
        public async Task<ActionResult<UserResponseModel>> UpdateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest(new { MessageId = -2, MessageDescription = "User data is required." });

            var userResponseModel = await userRepository.UpdateUserAsync(user);

            if (userResponseModel.MessageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = userResponseModel.MessageDescription });

            if (userResponseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = userResponseModel.MessageDescription });

            if (userResponseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = userResponseModel.MessageDescription });

            return Ok(new { MessageId = userResponseModel.MessageId, MessageDescription = userResponseModel.MessageDescription, UpdatedUser = userResponseModel.User });
        }
    }
}
