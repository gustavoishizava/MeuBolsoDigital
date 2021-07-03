using System.Net.Mime;
using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.Identity.Application.Interfaces;
using MBD.Identity.Application.Requests;
using MBD.Identity.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MBD.Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationAppService _service;

        public AuthenticationController(IAuthenticationAppService service)
        {
            _service = service;
        }

        [HttpPost("auth")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
        {
            var response = await _service.AuthenticateAsync(request);
            if(!response.Succeeded)
                return BadRequest(response);
            
            return Ok(response.Data);
        }

        [HttpPost("refresh")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var response = await _service.RefreshTokenAsync(request);
            if(!response.Succeeded)
                return BadRequest(response);
            
            return Ok(response.Data);
        }
    }
}