# Vehicule Rental

This project is a RESTfUL API developed in .NET 9 and C# to manage vehicule rentals for delivery drivers. The application allows for the registration of motorcycles, the registration of drivers, the rental process, and the return of motorcycles with cost calculation.

## Prerequisites

To run this project, you will need following tools installed:

- [.NET SDK 9](https://learn.microsoft.com/pt-br/dotnet/core/install/linux?WT.mc_id=dotnet-35129-website)
- [IDE](https://code.visualstudio.com/Download?WT.mc_id=dotnet-35129-website)
- [Docker](https://docs.docker.com/get-started/get-docker/) and Docker Compose

## Technologies Used

- **Backend:** .NET 9 / C#
- **Framework:** ASP.NET Core
- **Database:** PostgreSQL
- **Containerization:** Docker and Docker Compose
- **ORM:** Entity Framework Core 9
- **API Documentation:** Swagger (Swashbuckle)

## Features

- **Motorcycle Management:**
  - Register new motorcycles with a unique license plate.
  - Query motorcycles with a filter by license plate.
  - Update a motorcycle's license plate.
  - Delete motorcycles (as long as they have no associated rentals).

- **Driver Management:**
  - Register new drivers with a unique CNPJ (Business Registry Number) and CNH (Driver's License Number).
  - Upload a driver's license image (.png or .bmp formats).
  - Query drivers.  

- **Rental System:**
  - Create new rentals based on predefined plans.
  - Validate business rules (e.g., license type 'A', motorcycle availability).
  - Endpoint for motorcycle returns with total cost calculation (including penalties for early returns or fees for late returns).

## Architecture

The applicaiton follows a simple, layered RESTful API architecture:

- **Controllers:** Responsible for receiving HTTP requests, validating input, and orchestrating the response.
- **DTOs (Data Transfer Objects):** Classes that define the data shape for API requests and responses, decoupling the domain model from external exposure.
- **Models (Entities):** Classes that represent the database tables (`Motorcycle`, `Driver`, `Rental`, `RentalPlan`).
- **Data (DbContext):** Data access layer implemented with Entity Framework Core, which bridges the gap between C# objects and the PostgreSQL database.

The environment is fully containerized with Docker, ensuring portability and a streamlined setup process.

## How to Run the Project

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/fervinicius/vehicule-rental.git
    ```

2.  **Navigate to the project folder:**
    ```bash
    cd vehicule-rental
    ```

3.  **Start the containers:**
    This command will build the .NET application's image and start the API and database containers.
    ```bash
    docker-compose up --build
    ```

4.  **Access the application:**
    - The API will be available at: `http://localhost:8080`
    - The interactive Swagger documentation will be available at: `http://localhost:8080/swagger`

## Useful Entity Framework Core Commands

To manage database migrations, use the following commands in the terminal from the project's root folder. Remember that the database container must be running (`docker-compose up -d`).

-   **Create a new migration:**
    ```bash
    dotnet ef migrations add YourMigrationName
    ```

-   **Apply migrations to the database:**
    ```bash
    dotnet ef database update
    ```

-   **Remove the last migration (if something goes wrong):**
    ```bash
    dotnet ef migrations remove
    ```

## API Endpoints

The following is a list of the main endpoints available in the application:

### Motorcycles
- `POST /motorcycles` - Registers a new motorcycle.
- `GET /motorcycles` - Lists all motorcycles.
- `GET /motorcycles?licensePlate={plate}` - Filters motorcycles by license plate.
- `PUT /motorcycles/{id}` - Updates a motorcycle's license plate.
- `DELETE /motorcycles/{id}` - Deletes a motorcycle.

### Drivers
- `POST /drivers` - Registers a new driver.
- `GET /drivers` - Lists all drivers.
- `GET /drivers/{id}` - Gets a driver by ID.
- `POST /drivers/{id}/cnh-image` - Uploads the driver's license image for a driver.

### Rentals
- `POST /rentals` - Creates a new rental.
- `POST /rentals/{id}/return` - Finalizes a rental and calculates the total cost.
- `GET /rentals/plans` - Lists the available rental plans.