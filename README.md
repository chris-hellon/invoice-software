# InvoicePro

A modern, full-featured invoicing and business management application built with .NET 10 and Blazor Web App using Interactive Auto render mode.

## Features

### Core Invoicing
- **Invoice Management** - Create, edit, send, and track invoices with customizable templates
- **Estimates/Quotes** - Create estimates that can be accepted by clients and converted to invoices
- **Recurring Invoices** - Set up automated recurring billing on custom schedules
- **PDF Generation** - Professional PDF invoices and estimates using QuestPDF
- **Public Invoice Links** - Share invoices via secure token-based URLs

### Client Management
- **Client Database** - Store client details, billing addresses, and contact information
- **Client Portal** - Dedicated client-facing area for viewing invoices and estimates
- **Client-specific Settings** - Per-client language preferences and billing configurations

### Time & Expense Tracking
- **Timesheet** - Track billable hours with project and job associations
- **Expense Tracking** - Log and categorize business expenses
- **Recurring Expenses** - Automate regular expense entries
- **Bank Statement Import** - Import CSV bank statements and match transactions to invoices

### Project Management
- **Projects** - Organize work into projects with sections
- **Jobs** - Define billable work items with hourly rates
- **Product/Service Catalog** - Maintain a reusable catalog of products and services

### Business Settings
- **Business Profile** - Company details, logo, tax numbers, and registration info
- **Invoice Templates** - Multiple customizable visual styles (Minimal, Professional, Modern, Classic)
- **Multi-Currency Support** - Handle invoices in different currencies with per-currency payment settings
- **Multi-Language Support** - Localized invoices and estimates
- **Payment Options** - Bank transfer details, PayPal, Wise, Revolut integration
- **VietQR Integration** - QR code payments for Vietnamese bank transfers

### Security & Authentication
- **ASP.NET Core Identity** - Full authentication with email confirmation
- **Two-Factor Authentication** - TOTP authenticator app support
- **Passkey/WebAuthn** - Modern passwordless authentication
- **External OAuth** - Support for external login providers

## Technology Stack

- **.NET 10** (Preview)
- **Blazor Web App** - Interactive Auto render mode (Server + WebAssembly)
- **Entity Framework Core** - SQL Server database
- **ASP.NET Core Identity** - Authentication and authorization
- **QuestPDF** - PDF generation
- **Tailwind CSS** - Styling
- **BlazorUtils.EasyApi** - Simplified API handling

## Project Structure

```
InvoiceSoftware/
├── InvoiceSoftware/                 # Server project (ASP.NET Core host)
│   ├── Components/
│   │   ├── Account/                 # Identity UI pages
│   │   └── Pages/                   # Server-rendered pages
│   └── Data/                        # DbContext and migrations
│
├── InvoiceSoftware.Client/          # WebAssembly client project
│   ├── Pages/                       # Application pages
│   │   ├── Invoices/
│   │   ├── Estimates/
│   │   ├── Clients/
│   │   ├── Projects/
│   │   ├── Jobs/
│   │   ├── Timesheet/
│   │   ├── Expenses/
│   │   ├── Products/
│   │   ├── RecurringInvoices/
│   │   ├── RecurringExpenses/
│   │   ├── BankImport/
│   │   ├── ClientPortal/
│   │   ├── Settings/
│   │   └── Account/
│   ├── Components/                  # Shared UI components
│   └── Layout/                      # App layout components
│
├── InvoiceSoftware.Domain/          # Domain entities and business logic
│   ├── Entities/
│   ├── Enums/
│   └── ValueObjects/
│
├── InvoiceSoftware.Infrastructure/  # Data access and services
│   ├── Data/                        # EF Core configurations
│   ├── Handlers/                    # API request handlers
│   └── Services/                    # Business services (PDF, etc.)
│
└── InvoiceSoftware.Shared/          # Shared DTOs and API contracts
    ├── Api/                         # API request/response definitions
    ├── Dtos/                        # Data transfer objects
    └── Services/                    # Shared service interfaces
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (Preview)
- A code editor (VS Code, Visual Studio 2022, or JetBrains Rider)

### Running the Application

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd InvoiceSoftware
   ```

2. Build the solution:
   ```bash
   dotnet build InvoiceSoftware.sln
   ```

3. Run the application:
   ```bash
   dotnet run --project InvoiceSoftware/InvoiceSoftware/InvoiceSoftware.csproj
   ```

4. Open your browser and navigate to `https://localhost:5001` or `http://localhost:5000`

### Development with Hot Reload

```bash
dotnet watch --project InvoiceSoftware/InvoiceSoftware/InvoiceSoftware.csproj
```

### Database Migrations

Create a new migration:
```bash
dotnet ef migrations add <MigrationName> --project InvoiceSoftware/InvoiceSoftware
```

Apply migrations:
```bash
dotnet ef database update --project InvoiceSoftware/InvoiceSoftware
```

## Configuration

### Database

The application uses SQL Server. Copy `appsettings.json.example` to `appsettings.json` and configure your connection string:

```bash
cp InvoiceSoftware/InvoiceSoftware/appsettings.json.example InvoiceSoftware/InvoiceSoftware/appsettings.json
```

Then edit `appsettings.json` with your SQL Server credentials:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=InvoiceSoftware;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=True;Encrypt=False;"
  }
}
```

### Email Configuration

The application uses a no-op email sender by default. For production, configure a real SMTP provider in `Program.cs`:

```csharp
builder.Services.AddTransient<IEmailSender<ApplicationUser>, SmtpEmailSender>();
```

### External Authentication

Configure OAuth providers in `Program.cs`:

```csharp
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = "your-client-id";
        options.ClientSecret = "your-client-secret";
    });
```

## Domain Entities

| Entity | Description |
|--------|-------------|
| `Client` | Customer/client information |
| `Project` | Work projects with sections |
| `Job` | Billable work items |
| `TimeEntry` | Logged time entries |
| `Invoice` | Customer invoices |
| `Estimate` | Quotes/estimates |
| `Expense` | Business expenses |
| `Product` | Product/service catalog items |
| `RecurringInvoice` | Recurring billing templates |
| `RecurringExpense` | Recurring expense templates |
| `InvoiceTemplate` | Visual invoice styles |
| `BankTransaction` | Imported bank transactions |
| `Attachment` | File attachments |
| `DigitalSignature` | Electronic signatures |
| `BusinessProfile` | Company settings |

## API Architecture

The application uses **BlazorUtils.EasyApi** for a clean request/response pattern:

- API contracts are defined in `InvoiceSoftware.Shared/Api/`
- Handlers implement the contracts in `InvoiceSoftware.Infrastructure/Handlers/`
- Client-side calls use `ICall<TRequest, TResponse>` injection

Example:
```csharp
// Contract (Shared)
public record GetInvoice(Guid Id) : IGet<InvoiceDetailDto>;

// Handler (Infrastructure)
public class GetInvoiceHandler : IHandle<GetInvoice, InvoiceDetailDto>
{
    public async Task<HttpResult<InvoiceDetailDto>> Handle(GetInvoice request, CancellationToken ct)
    {
        // Implementation
    }
}

// Usage (Client)
@inject ICall<GetInvoice, InvoiceDetailDto> GetInvoiceCall

var invoice = await GetInvoiceCall.Call(new GetInvoice(invoiceId));
```

## License

This project is proprietary software. All rights reserved.

## Support

For issues and feature requests, please contact the development team.
