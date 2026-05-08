using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Enums;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Dashboard;
using InvoiceSoftware.Shared.Dtos.Dashboard;
using InvoiceSoftware.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Dashboard;

public class GetDashboardMetricsHandler(IDbContextFactory<ApplicationDbContext> dbFactory, ICurrentUserService currentUserService)
    : IHandle<GetDashboardMetrics, DashboardMetricsDto?>
{
    public async Task<HttpResult<DashboardMetricsDto?>> Handle(
        GetDashboardMetrics request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return HttpResult<DashboardMetricsDto?>.Unauthorized();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var now = DateOnly.FromDateTime(DateTime.UtcNow);

        // Calculate period based on request
        var periodStart = request.FromDate ?? request.Period switch
        {
            "week" => now.AddDays(-7),
            "quarter" => now.AddMonths(-3),
            "year" => now.AddYears(-1),
            _ => now.AddMonths(-1) // default to month
        };
        var periodEnd = request.ToDate ?? now;

        // Get all user's invoices with their time entries
        var invoices = await db.Invoices
            .Include(i => i.TimeEntries)
                .ThenInclude(te => te.Job)
            .Include(i => i.Client)
            .Where(i => i.Client.UserId == userId)
            .ToListAsync(cancellationToken);

        // Helper function to calculate invoice total
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

        // Paid invoices in period (PaidDate is already DateOnly)
        var paidInvoices = invoices
            .Where(i => i.Status == InvoiceStatus.Paid &&
                        i.PaidDate.HasValue &&
                        i.PaidDate.Value >= periodStart &&
                        i.PaidDate.Value <= periodEnd)
            .ToList();

        decimal paidRevenue = paidInvoices.Sum(CalculateInvoiceTotal);

        // Calculate total revenue (all paid invoices, not just in period)
        var allPaidInvoices = invoices.Where(i => i.Status == InvoiceStatus.Paid).ToList();
        decimal totalRevenue = allPaidInvoices.Sum(CalculateInvoiceTotal);

        // Outstanding (sent invoices)
        var outstandingInvoices = invoices.Where(i => i.Status == InvoiceStatus.Sent).ToList();
        decimal outstandingAmount = outstandingInvoices.Sum(CalculateInvoiceTotal);

        // Overdue invoices
        var overdueInvoices = invoices.Where(i => i.Status == InvoiceStatus.Overdue).ToList();
        decimal overdueAmount = overdueInvoices.Sum(CalculateInvoiceTotal);

        // Unbilled time entries (not linked to any invoice)
        var unbilledTimeEntries = await db.TimeEntries
            .Include(te => te.Job)
            .Where(te => te.UserId == userId && te.InvoiceId == null)
            .ToListAsync(cancellationToken);

        var unbilledHours = unbilledTimeEntries.Sum(te => te.Hours.Value);

        // Calculate unbilled amount (hours * effective hourly rate)
        decimal unbilledAmount = 0;
        foreach (var te in unbilledTimeEntries)
        {
            var hourlyRate = te.Job.GetEffectiveHourlyRate();
            unbilledAmount += te.Hours.Value * hourlyRate;
        }

        // Calculate billable hours (all time in period)
        var billableTimeEntries = await db.TimeEntries
            .Where(te => te.UserId == userId &&
                        te.Date >= periodStart &&
                        te.Date <= periodEnd)
            .ToListAsync(cancellationToken);
        var billableHours = billableTimeEntries.Sum(te => te.Hours.Value);

        // Counts
        var totalInvoiceCount = invoices.Count;
        var paidInvoiceCount = paidInvoices.Count;
        var outstandingInvoiceCount = outstandingInvoices.Count;
        var overdueInvoiceCount = overdueInvoices.Count;

        var activeClients = await db.Clients
            .Where(c => c.UserId == userId)
            .CountAsync(cancellationToken);

        var activeProjects = await db.Projects
            .Include(p => p.Client)
            .Where(p => p.Client.UserId == userId && p.Status == ProjectStatus.Active)
            .CountAsync(cancellationToken);

        var dto = new DashboardMetricsDto(
            TotalRevenue: totalRevenue,
            PaidRevenue: paidRevenue,
            OutstandingAmount: outstandingAmount,
            OverdueAmount: overdueAmount,
            TotalInvoices: totalInvoiceCount,
            PaidInvoices: paidInvoiceCount,
            OutstandingInvoices: outstandingInvoiceCount,
            OverdueInvoices: overdueInvoiceCount,
            BillableHours: billableHours,
            UnbilledHours: unbilledHours,
            UnbilledAmount: unbilledAmount,
            ActiveClients: activeClients,
            ActiveProjects: activeProjects);

        return HttpResult<DashboardMetricsDto?>.Ok(dto);
    }
}
