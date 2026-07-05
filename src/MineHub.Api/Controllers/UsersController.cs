using Microsoft.AspNetCore.Mvc;
using MineHub.Application.Users.Queries.GetUser;
using MineHub.Application.Users.Queries.GetUsers;

namespace MineHub.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly GetUserQueryHandler _getUserHandler;
        private readonly GetUsersQueryHandler _getUsersHandler;

        public UsersController(GetUserQueryHandler getUserHandler, GetUsersQueryHandler getUsersHandler)
        {
            _getUserHandler = getUserHandler;
            _getUsersHandler = getUsersHandler;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken token)
        {
            var user = await _getUserHandler.HandleAsync(id, token);
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var users = await _getUsersHandler.HandleAsync(token);
            return Ok(users);
        }

    }
}
