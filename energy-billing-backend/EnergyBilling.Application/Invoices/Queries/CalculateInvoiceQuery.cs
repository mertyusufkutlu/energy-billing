using EnergyBilling.Shared.Dtos;
using MediatR;

namespace EnergyBilling.Application.Invoices.Queries;

public class CalculateInvoiceQuery : IRequest<InvoiceCalculationResponseDto>
{
    public InvoiceCalculationRequestDto Input { get; set; }

    public CalculateInvoiceQuery(InvoiceCalculationRequestDto input)
    {
        Input = input;
    }
}