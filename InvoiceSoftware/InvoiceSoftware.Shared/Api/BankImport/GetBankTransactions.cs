using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.BankImport;

namespace InvoiceSoftware.Shared.Api.BankImport;

[Route("api/bank-import/transactions")]
public class GetBankTransactions : IGet<PaginatedResponse<BankTransactionDto>>, IPaginatedRequest
{
    [QueryStringParam]
    public int Page { get; set; } = 1;

    [QueryStringParam]
    public int PageSize { get; set; } = 15;

    [QueryStringParam]
    public string? Search { get; set; }

    [QueryStringParam]
    public bool? IsMatched { get; init; }

    [QueryStringParam]
    public bool? IsIgnored { get; init; }

    [QueryStringParam]
    public DateOnly? FromDate { get; init; }

    [QueryStringParam]
    public DateOnly? ToDate { get; init; }
}
