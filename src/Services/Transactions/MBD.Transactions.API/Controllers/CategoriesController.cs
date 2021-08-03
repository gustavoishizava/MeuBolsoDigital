using System.Collections.Generic;
using System;
using System.Net.Mime;
using System.Threading.Tasks;
using MBD.Core.Extensions;
using MBD.Transactions.API.Models;
using MBD.Transactions.Application.Interfaces;
using MBD.Transactions.Application.Request;
using MBD.Transactions.Application.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MBD.Transactions.Domain.Enumerations;

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

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            var result = await _service.CreateAsync(request);
            if (!result.Succeeded)
                return BadRequest(new ErrorModel(result));

            return Created($"/api/categories/{result.Data.Id}", result.Data);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryRequest request)
        {
            var result = await _service.UpdateAsync(request);
            if (!result.Succeeded)
                return BadRequest(new ErrorModel(result));

            return NoContent();
        }

        [HttpDelete("{id:GUID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _service.RemoveAsync(id);
            if (!result.Succeeded)
                return BadRequest(new ErrorModel(result));

            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(typeof(CategoryByTypeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            if (result.Income.IsNullOrEmpty() && result.Expense.IsNullOrEmpty())
                return NoContent();

            return Ok(result);
        }

        [HttpGet("{type:transactionType}/transactionType")]
        [ProducesResponseType(typeof(IEnumerable<CategoryWithSubCategoriesResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllByType([FromRoute] TransactionType type)
        {
            var result = await _service.GetByTypeAsync(type);
            if (result.IsNullOrEmpty())
                return NoContent();

            return Ok(result);
        }

        [HttpGet("{id:GUID}")]
        [ProducesResponseType(typeof(CategoryByTypeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (!result.Succeeded)
                return NotFound();

            return Ok(result.Data);
        }
    }
}