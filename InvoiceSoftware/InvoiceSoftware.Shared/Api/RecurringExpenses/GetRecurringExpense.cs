using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.RecurringExpenses;

namespace InvoiceSoftware.Shared.Api.RecurringExpenses;

[Route("api/recurring-expenses/{Id}")]
public class GetRecurringExpense : IGet<RecurringExpenseDetailDto?>
{
    [RouteParam]
    public Guid Id { get; init; }
}
