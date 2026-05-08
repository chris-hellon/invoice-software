using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Jobs;

namespace InvoiceSoftware.Shared.Api.Jobs;

[Route("api/jobs/active")]
public class GetActiveJobs : IGet<List<ActiveJobDto>>
{
}
