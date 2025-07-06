# Income Tax Calculator
This repository contains a web-based income tax calculation application built using ASP.NET Core. The application provides an API for calculating income tax based on predefined tax bands and rates.
## Architecture
The application follows the **Repository Pattern** combined with the **Service Layer Pattern**, providing a clean separation of concerns:
- **Controllers**: Handle HTTP requests and responses
- **Services**: Contain business logic for tax calculations
- **Data Layer**: Manages database operations via Entity Framework Core
- **Models**: Define the data structures used throughout the application

## Design Patterns
### Dependency Injection
The application heavily utilizes the built-in dependency injection container provided by ASP.NET Core. This allows for:
- Loose coupling between components
- Easier unit testing through interface-based programming
- More maintainable and extensible code

For example, the `TaxController` depends on the `ITaxService` interface rather than a concrete implementation:
``` csharp
public class TaxController : ControllerBase
{
    private readonly ITaxService _taxService;

    public TaxController(ITaxService taxService)
    {
        _taxService = taxService;
    }
    
    // Controller methods
}
```
### Repository Pattern
Data access is abstracted through repositories, providing a clean separation between the data access layer and business logic. This pattern:
- Centralizes data access logic
- Makes the application more testable
- Provides a consistent data access interface

### Service Layer Pattern
Business logic is encapsulated in service classes, which:
- Implement the application's business rules
- Act as an intermediary between controllers and repositories
- Allow for reuse of business logic across different parts of the application

## Database
The application uses Entity Framework Core for data access and migrations. The included migrations create:
- A `TaxBands` table storing the tax brackets and rates
- Seed data for common tax bands (0%, 20%, and 40%)

## API Endpoints
### GET /api/tax/{salary}
Calculates income tax based on the provided salary.
**Parameters**:
- `salary` (decimal): The annual salary (must be non-negative)

**Responses**:
- `200 OK`: Returns the tax calculation result
- `400 Bad Request`: If the salary is negative

## Third-Party Libraries
### Entity Framework Core
Used for database operations, including:
- Object-relational mapping
- Database migrations
- Seeding initial data

### xUnit and Moq
Used for unit testing:
- xUnit provides the testing framework
- Moq enables mocking of dependencies for isolated unit tests

## Project Structure
- **API**: Contains the ASP.NET Core Web API
    - **Controllers**: API endpoints
    - **Services**: Business logic
    - **Models**: Data models
    - **Data**: Database context and migrations
    - **Tests**: Unit tests for the API

- **UI**: Contains the frontend application written using Razor Pages