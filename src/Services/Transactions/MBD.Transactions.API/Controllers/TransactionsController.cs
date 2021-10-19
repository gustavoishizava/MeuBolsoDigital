using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using MBD.Core.Extensions;
using MBD.Transactions.API.Models;
using MBD.Transactions.Application.Commands;
using MBD.Transactions.Application.Queries.Transactions.Queries;
using MBD.Transactions.Application.Response;
using MBD.Transactions.Application.Response.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MBD.Transactions.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateTransactionCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Succeeded)
                return BadRequest(new ErrorModel(result));

            return Ok(result.Data);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] UpdateTransactionCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Succeeded)
                return BadRequest(new ErrorModel(result));

            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TransactionModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllTransactionsQuery());
            if (result.IsNullOrEmpty())
                return NoContent();

            return Ok(result);
        }

        [HttpGet("{id:GUID}")]
        [ProducesResponseType(typeof(TransactionModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetTransactionByIdQuery(id));
            if (!result.Succeeded)
                return NotFound();

            return Ok(result.Data);
        }

        [HttpDelete("{id:GUID}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var command = new DeleteTransactionCommand(id);
            var result = await _mediator.Send(command);
            if (!result.Succeeded)
                return BadRequest(new ErrorModel(result));

            return NoContent();
        }
    }
}