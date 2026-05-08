namespace InvoiceSoftware.Shared.Dtos.Dashboard;

public record CashFlowForecastDto(
    List<CashFlowPointDto> DataPoints,
    string Currency);

public record CashFlowPointDto(
    DateOnly Date,
    decimal ExpectedInflow,
    decimal ExpectedOutflow,
    decimal CumulativeBalance);
