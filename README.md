
# Car Stock Management API

## Project Overview

**Project Name**: Car Stock Management API

**Description**: A web API that allows car dealers to manage their car stocks, including adding/removing cars, listing cars and stock levels, updating car stock levels, and searching cars by make and model.

**Tech Stack**: 
- C#
- .NET Core
- FastEndpoints for API routing
- SQLite with Dapper for database management
- JWT for authentication
- Moq and xUnit for unit testing

**Purpose**: Simplifies inventory management for car dealerships by providing easy-to-use APIs for managing car stock.



## Installation & Setup

### Pre-requisites:
- [.NET SDK](https://dotnet.microsoft.com/download) (version 8)
- SQLite
- Any REST client (Postman or Curl)

## File Structure

```
├── CarStockManagementAPI/
│   ├── Data/                 # Contains DbContext, only responsible for creating tables
│   ├── Dtos/                 # Request and response models for API endpoints
│   ├── Endpoints/            # Contains all the API endpoints
│   ├── Models/               # Car and Dealer data models
│   ├── Repositories/         # Data access logic for the API, using Dapper for SQLite
│   ├── Services/             # Business logic for car management and authentication
│   └── Utils/                # Utility classes, such as JWT token generation
├── CarStockManagementAPI.Tests/  # Unit tests for services and endpoints
└── README.md                 # This file
```

### Clone the Repository:
```bash
git clone https://github.com/SNOWOVERSEER/Car-Stock-Management-System.git
cd CarStockManagementAPI
```

### Install Dependencies:
```bash
dotnet restore
```

### Database Setup:
- The project uses SQLite. A sample SQLite connection string is already provided in the `appsettings.json`.



## Running the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/your-repository-link.git
   ```

2. Navigate to the project folder:
   ```bash
   cd CarStockManagementAPI
   ```

3. Run the API:
   ```bash
   dotnet run
   ```

4. The API will run by default on `http://localhost:5176`. You can modify this base URL in the `launchSettings.json` file.

5. API documentation (Swagger) can be accessed at `http://localhost:5176/swagger/index.html` after the API is running.



## API Endpoints
### 1. **Register Dealer**
- **Endpoint**: `/api/auth/register`
- **Method**: POST
- **Request**:
    ```json
    {
      "name": "string",
      "email": "string",
      "password": "string"
    }
    ```
- **Response**:
    - 200 OK: `{"message": "Registration Success"}`
    - 400 Bad Request: `{"message": "Email already exists"}`

### 2. **Login Dealer**
- **Endpoint**: `/api/auth/login`
- **Method**: POST
- **Request**:
    ```json
    {
      "email": "string",
      "password": "string"
    }
    ```
- **Response**:
    - 200 OK: `{"message": "Login Success", "token": "JWT_TOKEN"}`
    - 401 Unauthorized: `{"message": "Invalid email or password"}`

### 3. **Add Car**
- **Endpoint**: `/api/cars/add`
- **Method**: POST
- **Request**:
    ```json
    {
      "make": "string",
      "model": "string",
      "year": 0,
      "color": "string",
      "stock": 0
    }
    ```
- **Response**:
    - 200 OK: `{"message": "Car added successfully"}`
    - 400 Bad Request: `{"message": "Car already exists for this dealer"}`

### 4. **Remove Car**
- **Endpoint**: `/api/cars/remove`
- **Method**: POST
- **Request**:
    ```json
    {
      "carId": 0
    }
    ```
- **Response**:
    - 200 OK: `{"message": "Car removed successfully"}`
    - 400 Bad Request: `{"message": "Car not found or you do not have permission to delete this car"}`

### 5. **List Cars**
- **Endpoint**: `/api/cars/list`
- **Method**: GET
- **Response**:
    - 200 OK:
        ```json
        {
          "message": "Cars found",
          "cars": [
            {
              "make": "string",
              "model": "string",
              "year": 0,
              "color": "string",
              "stock": 0
            }
          ]
        }
        ```
    - 200 OK (Empty):
        ```json
        {
          "message": "No cars found",
          "cars": []
        }
        ```

### 6. **Search Cars**
- **Endpoint**: `/api/cars/search`
- **Method**: POST
- **Request**:
    ```json
    {
      "make": "string",
      "model": "string" (nullable or optional)
    }
    ```
- **Response**:
    - 200 OK (when cars are found):
        ```json
        {
          "make": "string",
          "model": "string",
          "year": 0,
          "color": "string",
          "stock": 0
        }
        ```
    - 404 Not Found:
        ```json
        {
          "message": "No cars found"
        }
        ```

> **Note:** You can omit the `model` in the search request, or pass an empty string(or null), to search for all cars matching the provided `make`.

### 7. **Update Car Stock**
- **Endpoint**: `/api/cars/update-stock`
- **Method**: POST
- **Request**:
    ```json
    {
      "carId": 0,
      "newStock": 0
    }
    ```
- **Response**:
    - 200 OK: `{"message": "Stock updated successfully"}`
    - 400 Bad Request: `{"message": "Car not found or you do not have permission to update this car"}`


## Demo Dealer User Accounts

To test the system, you can use the following demo dealer accounts:

1. **Email:** `example@gmail.com`  
   **Password:** `123456`

2. **Email:** `example2@gmail.com`  
   **Password:** `123456`

Feel free to register your own account as well to explore additional features.


## Authentication Details

The API uses **JWT (JSON Web Tokens)** for securing endpoints.

After login, you will receive a token which needs to be passed in the `Authorization` header as a `Bearer` token for protected routes like:
- Add Car
- Remove Car
- List Cars
- Update Car Stock

Example Header:
```
Authorization: Bearer your_jwt_token
```

**JWT Token Expired**:
  - Tokens expire after 1 hour. Re-login to obtain a new token.




## Input Validation

For most requests, input validation is implemented to ensure data integrity:

1. **Add Car**: Validates that the `make`, `model`, `year`, `color`, and `stock` fields are provided and valid.
2. **Remove Car**: Validates the `carId` field.
3. **Search Cars**: Validates the `make` field, while `model` is optional.
4. **Update Car Stock**: Validates `carId` and `newStock` fields.



## Error Handling

Friendly error messages are returned in case of invalid input, unauthorized access, or server errors.

Standard response structure for errors:
```json
{
  "message": "error description"
}
```

**Status Codes**:
- `200 OK`: Successful operation
- `400 Bad Request`: Invalid input data
- `401 Unauthorized`: Invalid or missing JWT token
- `500 Internal Server Error`: Friendly error message is shown for errors.



## Unit Testing

The project uses **xUnit** and **Moq** for unit tests.

### Run Unit Tests at project root:
```bash
dotnet test
dotnet test CarStockManagementAPI.Tests
```

**Test cases include**:
- Authentication (login, registration)
- Adding/removing cars
- Updating car stock
- Listing and searching cars



## Logging

Logging is implemented across the repository layer to log any database-related exceptions.

Errors related to business logic are logged at the service level.


## Future Enhancements

- Adding role-based access control for dealers and administrators.
- Advanced car search functionality (by price, year range, etc.).
- Introducing rate-limiting to prevent abuse of the API.
