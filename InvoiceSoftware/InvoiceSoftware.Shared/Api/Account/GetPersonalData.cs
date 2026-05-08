using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.Account;

[Route("api/account/personal-data")]
public class GetPersonalData : IGet<byte[]>;
