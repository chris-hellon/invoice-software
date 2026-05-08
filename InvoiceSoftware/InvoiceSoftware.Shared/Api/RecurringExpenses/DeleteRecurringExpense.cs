using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.RecurringExpenses;

[Route("api/recurring-expenses/{Id}")]
public class DeleteRecurringExpense : IDelete
{
    [RouteParam]
    public Guid Id { get; init; }
}
