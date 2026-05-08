using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Enums;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Dashboard;
using InvoiceSoftware.Shared.Dtos.Dashboard;
using InvoiceSoftware.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Dashboard;

public class GetInvoiceStatusBreakdownHandler(IDbContextFactory<ApplicationDbContext> dbFactory, ICurrentUserService currentUserService)
    : IHandle<GetInvoiceStatusBreakdown, InvoiceStatusBreakdownDto?>
{
    public async Task<HttpResult<InvoiceStatusBreakdownDto?>> Handle(
        GetInvoiceStatusBreakdown request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return HttpResult<InvoiceStatusBreakdownDto?>.Unauthorized();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        // Get all user's invoices
        var invoices = await db.Invoices
            .Include(i => i.TimeEntries)
                .ThenInclude(te => te.Job)
            .Include(i => i.Client)
            .Where(i => i.Client.UserId == userId)
            .ToListAsync(cancellationToken);

        // Count by status
        var draftCount = invoices.Count(i => i.Status == InvoiceStatus.Draft);
        var sentCount = invoices.Count(i => i.Status == InvoiceStatus.Sent);
        var paidCount = invoices.Count(i => i.Status == InvoiceStatus.Paid);
        var overdueCount = invoices.Count(i => i.Status == InvoiceStatus.Overdue);
        var voidCount = invoices.Count(i => i.Status == InvoiceStatus.Void);

        // Helper to calculate invoice total
        decimal CalculateInvoiceTotal(Domain.Entities.Invoice invoice)
        {
            decimal subtotal = 0;
            foreach (var te in invoice.TimeEntries)
            {
                var hourlyRate = te.Job.GetEffectiveHourlyRate();
                subtotal += te.Hours.Value * hourlyRate;
            }
            return subtotal + (subtotal * invoice.TaxRate / 100);
        }

        // Calculate amounts by status
        var draftAmount = invoices.Where(i => i.Status == InvoiceStatus.Draft).Sum(CalculateInvoiceTotal);
        var sentAmount = invoices.Where(i => i.Status == InvoiceStatus.Sent).Sum(CalculateInvoiceTotal);
        var paidAmount = invoices.Where(i => i.Status == InvoiceStatus.Paid).Sum(CalculateInvoiceTotal);
        var overdueAmount = invoices.Where(i => i.Status == InvoiceStatus.Overdue).Sum(CalculateInvoiceTotal);

        var dto = new InvoiceStatusBreakdownDto(
            draftCount,
            sentCount,
            paidCount,
            overdueCount,
            voidCount,
            draftAmount,
            sentAmount,
            paidAmount,
            overdueAmount);

        return HttpResult<InvoiceStatusBreakdownDto?>.Ok(dto);
    }
}
