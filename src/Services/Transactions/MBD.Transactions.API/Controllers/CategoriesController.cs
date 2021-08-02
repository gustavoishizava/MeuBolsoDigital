using System.Net.Mime;
using MBD.Transactions.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MBD.Transactions.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryAppService _service;

        public CategoriesController(ICategoryAppService service)
        {
            _service = service;
        }
    }
}