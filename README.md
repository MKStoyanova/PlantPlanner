# PlantPlanner

PlantPlanner is an ASP.NET Core MVC web application for managing plants and tracking their watering schedule.

This project was created as part of an ASP.NET Fundamentals course assignment.

---

## Features

- Create, edit, delete plants
- Select plant type (with custom option)
- Select soil type (predefined options)
- Log watering with one click
- Automatic watering reminder logic
- View watering history per plant
- Clean MVC structure following ASP.NET conventions

---

## Database Structure

### Entities:
- Plant
- WateringLog
- Soil

### Relationships:
- One Plant has many WateringLogs
- One Plant has one Soil

---

## Watering Logic

- If a plant has never been watered → shows *No watering yet*
- If recently watered → shows *It will need water in X days*
- If watering is overdue → shows *Don't forget to water today!*

---

## Technologies Used

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server (LocalDB)
- Razor Views
- Bootstrap

---

## How to Run

1. Clone the repository
2. Open the project in Visual Studio
3. Run `Update-Database` in Package Manager Console
4. Start the application

---

## Purpose

The goal of this project is to demonstrate understanding of:

- MVC architecture
- Models and relationships
- Form validation
- Entity Framework Core
- Clean and modular project structure
