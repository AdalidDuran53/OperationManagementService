# OperationManagementService (OMS)

Modular service for enterprise operations management, designed with clean architecture, reusable patterns, and scalability focus.

## 🚀 Key Features

- RESTful API in .NET 9 with stateless authentication (API Key)
- Robust JSON/XML serialization with pre-validation
- Configurable rate limiters by IP
- Structured logging and secure error handling
- Integration with Entity Framework Core (including keyless models)
- Reusable helpers for operation control and data transformation

## 🧱 Project Structure

```plaintext
├── EntitiesCustom/                # Custom and keyless models
├── Errors/                        # Controlled exceptions and helpers
├── OperationManagementService/    # Implementation and business rules
├── database/                      # Database scripts
├── OperationManagementService.sln
└── .gitignore
