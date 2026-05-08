using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.BankImport;

namespace InvoiceSoftware.Shared.Api.BankImport;

[Route("api/bank-import/transactions/{Id}/suggestions")]
public class GetSuggestedMatches : IGet<List<TransactionMatchDto>>
{
    [RouteParam]
    public Guid Id { get; init; }
}
