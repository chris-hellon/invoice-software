namespace InvoiceSoftware.Shared.Dtos.Account;

public record EnableTwoFactorResultDto(bool Success, string? ErrorMessage, string[]? RecoveryCodes);
