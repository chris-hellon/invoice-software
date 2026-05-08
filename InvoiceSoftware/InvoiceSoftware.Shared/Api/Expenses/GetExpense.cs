using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Expenses;

namespace InvoiceSoftware.Shared.Api.Expenses;

[Route("api/expenses/{Id}")]
public class GetExpense : IGet<ExpenseDetailDto?>
{
    [RouteParam]
    public Guid Id { get; init; }
}
