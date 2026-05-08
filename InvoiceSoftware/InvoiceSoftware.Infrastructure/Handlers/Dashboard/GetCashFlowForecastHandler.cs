using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Enums;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Dashboard;
using InvoiceSoftware.Shared.Dtos.Dashboard;
using InvoiceSoftware.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Dashboard;

public class GetCashFlowForecastHandler(IDbContextFactory<ApplicationDbContext> dbFactory, ICurrentUserService currentUserService)
    : IHandle<GetCashFlowForecast, CashFlowForecastDto?>
{
    public async Task<HttpResult<CashFlowForecastDto?>> Handle(
        GetCashFlowForecast request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return HttpResult<CashFlowForecastDto?>.Unauthorized();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var months = request.Months > 0 ? request.Months : 3;
        months = Math.Min(months, 12);

        var now = DateOnly.FromDateTime(DateTime.UtcNow);

        // Get outstanding invoices (expected income)
        var outstandingInvoices = await db.Invoices
            .Include(i => i.TimeEntries)
                .ThenInclude(te => te.Job)
            .Include(i => i.Client)
            .Where(i => (i.Status == InvoiceStatus.Sent || i.Status == InvoiceStatus.Overdue) &&
                        i.Client.UserId == userId)
            .ToListAsync(cancellationToken);

        // Get recurring invoices (future expected income)
        var recurringInvoices = await db.RecurringInvoices
            .Include(r => r.LineItems)
            .Where(r => r.UserId == userId && r.IsActive)
            .ToListAsync(cancellationToken);

        // Get recurring expenses (expected outgoing) - filter by CreatedBy since RecurringExpense doesn't have UserId
        var recurringExpenses = await db.RecurringExpenses
            .Where(e => e.CreatedBy == userId && e.IsActive)
            .ToListAsync(cancellationToken);

        var dataPoints = new List<CashFlowPointDto>();
        var currentMonth = new DateOnly(now.Year, now.Month, 1);

        var cumulativeBalance = 0m;

        for (var i = 0; i < months; i++)
        {
            var monthStart = currentMonth.AddMonths(i);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);

            // Expected income from outstanding invoices due this month
            var expectedIncome = outstandingInvoices
                .Where(inv => inv.DueDate >= monthStart && inv.DueDate <= monthEnd)
                .Sum(inv =>
                {
                    decimal subtotal = 0;
                    foreach (var te in inv.TimeEntries)
                    {
                        subtotal += te.Hours.Value * te.Job.GetEffectiveHourlyRate();
                    }
                    return subtotal + (subtotal * inv.TaxRate / 100);
                });

            // Expected income from recurring invoices
            foreach (var ri in recurringInvoices)
            {
                var nextDate = ri.NextInvoiceDate;
                while (nextDate <= monthEnd)
                {
                    if (nextDate >= monthStart)
                    {
                        var subtotal = ri.LineItems.Sum(li => li.LineTotal.Amount);
                        expectedIncome += subtotal + (subtotal * ri.TaxRate / 100);
                    }
                    nextDate = CalculateNextDate(nextDate, ri.FrequencyInterval, ri.Frequency);
                }
            }

            // Expected expenses from recurring expenses
            var expectedExpenses = 0m;
            foreach (var re in recurringExpenses)
            {
                var nextDate = re.NextExpenseDate;
                while (nextDate <= monthEnd)
                {
                    if (nextDate >= monthStart)
                    {
                        expectedExpenses += re.Amount.Amount;
                    }
                    nextDate = CalculateNextDate(nextDate, re.FrequencyInterval, re.Frequency);
                }
            }

            cumulativeBalance += expectedIncome - expectedExpenses;

            dataPoints.Add(new CashFlowPointDto(
                monthStart,
                expectedIncome,
                expectedExpenses,
                cumulativeBalance));
        }

        var dto = new CashFlowForecastDto(dataPoints, "USD");
        return HttpResult<CashFlowForecastDto?>.Ok(dto);
    }

    private static DateOnly CalculateNextDate(DateOnly current, int interval, RecurrenceFrequency frequency)
    {
        return frequency switch
        {
            RecurrenceFrequency.Day => current.AddDays(interval),
            RecurrenceFrequency.Week => current.AddDays(interval * 7),
            RecurrenceFrequency.Month => current.AddMonths(interval),
            RecurrenceFrequency.Year => current.AddYears(interval),
            _ => current.AddMonths(interval)
        };
    }
}
