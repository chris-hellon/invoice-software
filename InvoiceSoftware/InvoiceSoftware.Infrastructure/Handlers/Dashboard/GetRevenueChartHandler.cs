using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Enums;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Dashboard;
using InvoiceSoftware.Shared.Dtos.Dashboard;
using InvoiceSoftware.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Dashboard;

public class GetRevenueChartHandler(IDbContextFactory<ApplicationDbContext> dbFactory, ICurrentUserService currentUserService)
    : IHandle<GetRevenueChart, RevenueChartDataDto>
{
    public async Task<HttpResult<RevenueChartDataDto>> Handle(
        GetRevenueChart request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return HttpResult<RevenueChartDataDto>.Unauthorized();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var now = DateTime.UtcNow;
        var months = request.Months > 0 ? request.Months : 12;
        var dataPoints = new List<RevenueDataPoint>();

        // Get all invoices for the user
        var invoices = await db.Invoices
            .Include(i => i.TimeEntries)
                .ThenInclude(te => te.Job)
            .Include(i => i.Client)
            .Where(i => i.Client.UserId == userId)
            .ToListAsync(cancellationToken);

        // Get all expenses for the user (Expense uses CreatedBy, not UserId)
        var expenses = await db.Expenses
            .Where(e => e.CreatedBy == userId)
            .ToListAsync(cancellationToken);

        // Get default currency
        var businessProfile = await db.BusinessProfiles
            .FirstOrDefaultAsync(bp => bp.UserId == userId, cancellationToken);
        var currency = businessProfile?.DefaultCurrency ?? "USD";

        // Group by month
        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(now.Year, now.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);
            var label = monthStart.ToString("MMM yyyy");

            // Paid invoices in this month (PaidDate is DateOnly, compare with DateOnly)
            var monthStartDate = DateOnly.FromDateTime(monthStart);
            var monthEndDate = DateOnly.FromDateTime(monthEnd);
            var monthInvoices = invoices
                .Where(inv => inv.Status == InvoiceStatus.Paid &&
                             inv.PaidDate.HasValue &&
                             inv.PaidDate.Value >= monthStartDate &&
                             inv.PaidDate.Value <= monthEndDate)
                .ToList();

            decimal revenue = 0;
            foreach (var invoice in monthInvoices)
            {
                decimal subtotal = 0;
                foreach (var te in invoice.TimeEntries)
                {
                    var hourlyRate = te.Job.GetEffectiveHourlyRate();
                    subtotal += te.Hours.Value * hourlyRate;
                }
                revenue += subtotal + (subtotal * invoice.TaxRate / 100);
            }

            // Expenses in this month (use monthStartDate and monthEndDate already calculated)
            var monthExpenses = expenses
                .Where(e => e.ExpenseDate >= monthStartDate && e.ExpenseDate <= monthEndDate)
                .Sum(e => e.Amount.Amount);

            var profit = revenue - monthExpenses;

            dataPoints.Add(new RevenueDataPoint(label, revenue, monthExpenses, profit));
        }

        var dto = new RevenueChartDataDto(dataPoints, currency, request.Period);

        return HttpResult<RevenueChartDataDto>.Ok(dto);
    }
}
