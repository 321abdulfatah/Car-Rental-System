# Car-Rental-System

## Description

This is a car rental system that automates the core business of a car rental company. The system includes a database that stores information about the company's cars, including their unique car number, type, engine capacity, color, daily fare, and whether they come with or without a driver. The system also allows drivers to replace each other in driving one of the cars in their absence, and customers can rent more than one car according to the type of car.

This project was implemented using C# and ASP.NET Core 6, with Entity Framework Code-First approach for creating tables in a SQL database. The system also implements N-Tier Architecture and Repository Pattern to maintain separation of concerns and improve code maintainability.

The system exposes multiple endpoints for the Car entity to perform CRUD operations using Entity Framework Core, with support for paging, searching, and sorting. For performance purposes, the system also implements caching using an in-memory or distributed cache to cache cars and create an endpoint to get cars from the cache.

## Installation

1- Clone the repository to your local machine.

2- Open the solution file in Visual Studio.

3- Update the connection string in the appsettings.json file to the local database path.

4- Build the project to ensure that all the dependencies are installed correctly.

5- Run the migration to create the database schema using the Package Manager Console: Update-Database.

6- Start the application by pressing F5 or clicking on the Start button in Visual Studio.

## Usage

To use the system, follow these steps:

Start the application and navigate to the appropriate endpoint for the desired operation.

Use the appropriate HTTP method (GET, POST, PUT, DELETE) to perform the desired operation.

For CRUD operations, provide the necessary data in the request body in JSON format.

For paging, searching, and sorting, provide the appropriate query parameters in the URL.

For caching, use the appropriate endpoint to retrieve cars from the cache.


## Additional Information

For more information about the system and its implementation, please refer to the [source code](https://github.com/321abdulfatah/Car-Rental-System) and documentation in the repository. If you encounter any issues or have any questions, please don't hesitate to contact the developers.
