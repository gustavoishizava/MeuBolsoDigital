using System.Net.Mime;
using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.Identity.Application.Interfaces;
using MBD.Identity.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MBD.Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class UsersController : ControllerBase
    {
        private readonly IUserAppService _service;

        public UsersController(IUserAppService service)
        {
            _service = service;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            var response = await _service.CreateAsync(request);
            if (!response.Succeeded)
                return BadRequest(response);

            return NoContent();
        }
    }
}