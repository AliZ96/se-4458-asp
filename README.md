# SE4458 Midterm Project – Airline API

This project is a RESTful Web API developed with ASP.NET Core for the SE4458 Software Engineering course. It implements a fully functional airline ticketing system based on the midterm project requirements.

## Features

- Add flight
- Query flights with filters and paging
- Buy ticket (passenger auto-created if not found)
- Check-in
- Retrieve passenger list with paging
- JWT-based authentication and authorization
- API documentation with Swagger
- Cloud-based deployment and database

## Authentication

- Login endpoint: `/api/auth/login`
- JWT token is required for all protected endpoints
- Swagger UI provides Authorize button for token usage

## Deployment Info

- API Base URL: [https://se-4458-asp.onrender.com](https://se-4458-asp.onrender.com)
- Swagger UI: [https://se-4458-asp.onrender.com/swagger/index.html](https://se-4458-asp.onrender.com/swagger/index.html)

## Assumptions

- Only hardcoded admin user exists:  
  Username: `admin`  
  Password: `123456`
- Seat numbers are assigned incrementally upon check-in
- No cancellation or update logic is implemented for flights or tickets

## Technologies Used

- ASP.NET Core Web API (.NET 8)
- Entity Framework Core with Pomelo MySQL
- MySQL (hosted on Railway)
- JWT Authentication (Bearer tokens)
- Swagger (via Swashbuckle)
- Deployed via Render.com

## Test Instructions

1. Open Swagger UI from the link above
2. Use the login endpoint to obtain a JWT token
3. Click the Authorize button and paste the token
4. Add a flight, buy a ticket, check-in, and list passengers
## Example flight
{
  "fromAirport": "AYT",
  "toAirport": "ADB",
  "dateFrom": "2025-06-10T09:00:00",
  "dateTo": "2025-06-10T10:30:00",
  "duration": 90,
  "capacity": 5
}


## Developer Info

- Name: Ali Özmen
- Date: April 2025
