
# Employment Management System
  

A full-stack web application for employee management using a vertical clean architecture approach.
  

## Table of Contents

- [Overview](#overview)

- [Architecture](#architecture)

- [Database Design](#database-design)

- [API Endpoints](#api-endpoints)

- [Frontend](#frontend)

- [Authentication](#authentication)

- [Getting Started](#getting-started)

- [Project Structure](#project-structure)

  

## Overview

  

This application provides a simple interface for managing employee information, including their personal details and department assignments. The system consists of:

  

1. A relational database to store employee and department information

2. A RESTful API built with vertical clean architecture

3. A responsive frontend for viewing and managing employees

  

## Architecture

  

This project implements a **Vertical Clean Architecture** approach, which separates concerns while organizing code by feature rather than technical layer.

  

### Key Components:

  

-  **Core Domain Layer**: Contains entities, value objects, and domain services

-  **Application Layer**: Contains use cases/application services implementing business logic

-  **Infrastructure Layer**: Contains implementations of repositories, external services

-  **Presentation Layer**: Contains API controllers and frontend components

-  **Cross-Cutting Concerns**: Authentication, logging, and other shared utilities

  

Each vertical slice (feature) follows SOLID principles and implements appropriate patterns:

- Dependency Injection

- CQRS with MediatR

- Repository Pattern

- Unit of Work

- Fluent Validation

  

## Database Design

  

### Entity Relationship Diagram

  

```

┌────────────────┐ ┌────────────────┐

│ Department │ │ Employee │

├────────────────┤ ├────────────────┤

│ Id (PK) │◄─────┤ Id (PK) │

│ Name │ │ FirstName │

│ Description │ │ LastName │

│ │ │ HireDate │

│ │ │ DepartmentId(FK)│

│ │ │ Phone │

│ │ │ Address │

│ │ │ AvatarUrl │

└────────────────┘ └────────────────┘

```

  

### Tables and Fields

  

**Department Table**

-  `Id` (INT, PK): Unique identifier

-  `Name` (VARCHAR(100)): Department name

-  `Description` (VARCHAR(500), nullable): Optional department description

  

**Employee Table**

-  `Id` (INT, PK): Unique identifier

-  `FirstName` (VARCHAR(100)): Employee's first name

-  `LastName` (VARCHAR(100)): Employee's last name

-  `HireDate` (DATE): Date when employee was hired

-  `DepartmentId` (INT, FK): Foreign key to Department table

-  `Phone` (VARCHAR(20)): Employee's phone number

-  `Address` (VARCHAR(200)): Employee's address

-  `AvatarUrl` (VARCHAR(255), nullable): URL to employee's avatar image

  

### Migrations

Database migrations are managed through the ORM's migration system. Initial migration scripts are included in the `Infrastructure/Persistence/Migrations` folder.

  

## API Endpoints

  

The API follows RESTful principles and is secured with API key authentication.

  

### Employee Endpoints

  

#### `POST /api/employees`

Creates a new employee.

- Request Body: Employee details including departmentId

- Response: Created employee with full department information

  

#### `GET /api/employees`

Returns all employees with their departments.

- Query Parameters:

-  `page`: Page number (optional)

-  `pageSize`: Items per page (optional)

- Response: List of employees with pagination metadata

  

#### `GET /api/employees/{id}`

Returns a specific employee by ID.

- Path Parameters:

-  `id`: Employee ID

- Response: Employee details with department information

  

#### `PUT /api/employees/{id}`

Updates an existing employee.

- Path Parameters:

-  `id`: Employee ID

- Request Body: Updated employee details

- Response: Updated employee with department information

  

#### `DELETE /api/employees/{id}`

Deletes an employee.

- Path Parameters:

-  `id`: Employee ID

- Response: 204 No Content

  

### Department Endpoints

  

#### `GET /api/departments`

Returns all departments.

- Response: List of all departments

  

## Frontend

  

The frontend is built as a responsive single-page application with the following key features:

  

### Employee List Page

- Displays a table/grid of employees with:

- Employee name (First + Last)

- Department

- Hire date in format: "May 2, 2021 (2y – 1m – 4d)" showing tenure

- Employee avatar

- Action buttons:

- "View Details" to view employee details

- "Delete" (X) to remove an employee

- "New Employee" button to open creation modal

  

### Employee Details Page/Modal

- Shows all employee information

- Department dropdown with current selection

- "Update" button to save changes to department

- Automatically reflects UI changes when updated

  

### New Employee Modal

- Form for entering all required employee information

- Department selection dropdown

- Form validation for required fields

- Submit button to create a new employee

  

## Authentication

  

The API implements a simple API key authentication mechanism:

- API requests require an `X-API-Key` header with a valid key

- Keys are stored securely and validated on each request

- Unauthorized requests return 401 Unauthorized responses

  

## Getting Started

  

### Prerequisites

- .NET 8 SDK

- Node.js and npm

- SQL Server/MySQL/PostgreSQL (or other compatible database)

  

### Setup Instructions

  

1.  **Clone the repository**

```bash

git clone https://github.com/yourusername/employee-management.git

cd employee-management

```

  

2.  **Configure database connection**

Edit `appsettings.json` in the API project with your database connection string.

  

3.  **Run database migrations**

```bash

cd src/Infrastructure

dotnet ef database update

```

  

4.  **Start the API**

```bash

cd ../API

dotnet run

```

  

5.  **Setup the frontend**

```bash

cd ../ClientApp

npm install

npm start

```

  

6.  **Access the application**

Open your browser and navigate to `http://localhost:3000`

  

## Project Structure

  

```

employee-management/

├── src/

│ ├── Domain/ # Core domain models and interfaces

│ ├── Application/ # Application services and DTOs

│ │ ├── Employees/ # Employee-related features

│ │ │ ├── Commands/ # Create, Update, Delete operations

│ │ │ └── Queries/ # Get operations

│ │ └── Departments/ # Department-related features

│ │ └── Queries/ # Get operations

│ ├── Infrastructure/ # Data access, external services

│ │ ├── Persistence/ # Database context and repositories

│ │ └── Services/ # External service implementations

│ ├── API/ # API Controllers and configuration

│ └── ClientApp/ # Frontend application

└── tests/ # Unit and integration tests

├── UnitTests/

└── IntegrationTests/

```