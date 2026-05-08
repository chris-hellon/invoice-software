using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.RecurringExpenses;

[Route("api/recurring-expenses/{Id}/pause")]
public class PauseRecurringExpense : IPost
{
    [RouteParam]
    public Guid Id { get; init; }
}
