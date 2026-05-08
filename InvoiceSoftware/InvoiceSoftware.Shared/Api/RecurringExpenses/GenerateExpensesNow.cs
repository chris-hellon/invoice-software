using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Expenses;

namespace InvoiceSoftware.Shared.Api.RecurringExpenses;

[Route("api/recurring-expenses/{Id}/generate")]
public class GenerateExpensesNow : IPost<ExpenseSummaryDto?>
{
    [RouteParam]
    public Guid Id { get; init; }
}
