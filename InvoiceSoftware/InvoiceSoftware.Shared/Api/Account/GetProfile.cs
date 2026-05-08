using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Account;

namespace InvoiceSoftware.Shared.Api.Account;

[Route("api/account/profile")]
public class GetProfile : IGet<ProfileDto>;
