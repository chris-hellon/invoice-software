using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.Expenses;

[Route("api/expenses/{Id}")]
public class DeleteExpense : IDelete
{
    [RouteParam]
    public Guid Id { get; init; }
}
