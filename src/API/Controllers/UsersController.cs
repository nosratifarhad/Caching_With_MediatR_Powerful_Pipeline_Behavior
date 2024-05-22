using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Features.Users.Queries.GetAllUsers;
using Application.Features.Users.Queries.GetUserById;

namespace API.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public UsersController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [Route("api/v.1/users/get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetAllUsersQuery();
            
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("api/v.1/users/get-user-by-id")]
        public async Task<IActionResult> GetUserById(GetUserByIdQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}
