using EnergyBilling.Application.Invoices.Queries;
using EnergyBilling.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EnergyBilling.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IMediator _mediator;

    public InvoiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST: /api/invoice/calculate
    [HttpPost("calculate")]
    public async Task<ActionResult<InvoiceCalculationResponseDto>> Calculate([FromBody] InvoiceCalculationRequestDto request)
    {
        // Fatura hesaplama query'sini handler'a gönder
        var result = await _mediator.Send(new CalculateInvoiceQuery(request));
        return Ok(result);
    }
}