# Visitor Management System

A multi-tenant visitor management system built with .NET 9, implementing Clean Architecture and CQRS patterns.

## Features

- 🏢 Multi-tenant architecture
- 🔐 JWT Authentication
- 👥 User management
- 📊 Visitor tracking
- 🏗️ Clean Architecture
- 📝 CQRS with MediatR
- 🗄️ PostgreSQL database
- 🧪 Unit testing with xUnit

## Prerequisites

- .NET 9.0 SDK
- PostgreSQL 15+
- Visual Studio 2022 or VS Code

## Getting Started

1. Clone the repository
```bash
git clone https://github.com/yourusername/MultiTenantExample.git
```

2. Navigate to the project directory
```bash
cd MultiTenantExample
```

3. Update the connection strings in `appsettings.json`
```json
{
  "ConnectionStrings": {
    "MasterDatabase": "Host=localhost;Port=5432;Database=visitor_master;Username=your_username;Password=your_password"
  }
}
```

4. Run the migrations
```bash
dotnet ef database update --project src/VisitorManagement.Infrastructure --startup-project src/VisitorManagement.Api
```

5. Run the application
```bash
dotnet run --project src/VisitorManagement.Api
```

## Project Structure

```
├── src/
│   ├── VisitorManagement.Domain        # Enterprise business rules
│   ├── VisitorManagement.Application   # Application business rules
│   ├── VisitorManagement.Infrastructure# External concerns
│   └── VisitorManagement.Api           # Entry point
└── tests/
    └── VisitorManagement.Application.UnitTests
```

## Architecture

This project follows Clean Architecture principles:

- **Domain Layer**: Contains enterprise-wide business rules and entities
- **Application Layer**: Contains application-specific business rules
- **Infrastructure Layer**: Contains frameworks and external concerns
- **API Layer**: Contains controllers and configuration

## Testing

Run the tests using:
```bash
dotnet test
```

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
