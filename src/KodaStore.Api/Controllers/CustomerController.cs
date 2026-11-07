using KodaStore.Api.Data;
using KodaStore.Api.DTOs;
using KodaStore.Api.Models;
using KodaStore.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace KodaStore.Api.Controllers;

[ApiController]
[Route("api/v1/customers")]

public sealed class CustomerController : ControllerBase
{
    private readonly ICustomerServices _service;

    public CustomerController(ICustomerServices service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CustomerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IReadOnlyList<CustomerResponse>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<Customers>>> GetAll(CancellationToken cancellationToken)
    {
        var customers = await _service.GetAllAsync(cancellationToken);
        return Ok(customers);
    }


    [HttpGet("{customerCode}")]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]

    public async Task<ActionResult<CustomerResponse>> GetByCode([FromRoute] string customerCode, CancellationToken cancellationToken)
    {
        var customers = await _service.GetByCodeAsync(customerCode, cancellationToken);
        return customers is null? Problem(statusCode: StatusCodes.Status404NotFound, title: "Data tidak ditemukan"): Ok(customers);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CustomerResponse>> Create([FromBody] CustomersRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _service.CreateAsync(request, cancellationToken);
            if(created is null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, title: "Data tidak dapat di dibuat");
            }

            return CreatedAtAction(
                nameof(GetByCode),
                new {customerCode = created.CustomerCode},
                created
            );
        }
        catch (System.Exception e)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "invalid create data",
                detail: e.Message
            );
        }
    }

    [HttpDelete("{customerCode}")]
    [ProducesResponseType(typeof(CustomerResponse),StatusCodes.Status200OK )]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] string customerCode, CancellationToken cancellationToken)
    {
        var deleted = await _service.DeleteAsync(customerCode, cancellationToken);
        return deleted ? NoContent() : Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: "Gagal delete",
            detail: $"gagal {customerCode}"
        );
    }

    [HttpPut("{customerCode}")]
    [ProducesResponseType(typeof(CustomerResponse),StatusCodes.Status200OK )]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerResponse>> Update([FromBody] UpdateCustomerRequest request, [FromRoute] string customerCode, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _service.UpdateAsync(customerCode, request, cancellationToken);
            return updated is null ? Problem(statusCode: StatusCodes.Status404NotFound, title: "gagal update", detail: $"No product was found for code {customerCode}") : Ok(updated);
        }
        catch (System.Exception e)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "invalid update data",
                detail: e.Message
            );
        }
    }

}