# 🌿 PlantPlanner

PlantPlanner is an ASP.NET Core MVC web application designed to help users manage and care for their plants.  
It allows tracking watering schedules, managing plant data, and receiving helpful care suggestions based on plant type, soil, and watering intervals.

---

## Features
## 🎥 Demo

### User Flow
- Register a new account
- Login
- Create a plant
- Add a reminder
- View plant details and care feedback

### Per-User Behavior
- Each user sees only their own plants and reminders

### Admin Features
- Admin has access to a dashboard
- Can view system statistics (plants, reminders, users)

### Additional Pages
- Care Tips page
- Light Tips page

### Technical Features
- Pagination
- Filtering and search
- Custom error pages (404 / 500)
- Unit tests for service logic

### 🔐 Authentication & Roles
- User registration and login (ASP.NET Core Identity)
- Role-based access:
  - **Admin** – access to admin dashboard
  - **User** – personal plant management

---

### 👤 Per-User Data Isolation
- Each user can only see and manage their own:
  - Plants
  - Reminders
- Ensures a personalized and secure experience

---

### 🌱 Plant Management
- Create, edit, delete, and view plants
- Store information such as:
  - Name
  - Type
  - Light requirements
  - Watering interval
  - Soil type
  - Location
- Filtering by soil
- Search by plant name
- Pagination for better usability

---

### 💧 Smart Care Feedback System
The application analyzes plant data and provides feedback:

- ⚠️ Warning if:
  - Incorrect soil is selected for the plant type
  - Watering interval is too short or too long

- ✅ Success message:
  - **"That’s one thriving plant!"** when conditions are optimal

---

### ⏰ Reminders
- Create reminders for plant care tasks
- Includes date and time
- Reminders are user-specific

---

### 🌿 Care Tips
- Predefined tips for:
  - Watering
  - Soil
  - Light
- Organized and displayed in a user-friendly way

---

### ☀ Light Tips Page
- Dedicated page with guidance on plant light requirements
- Helps users better understand plant positioning and sunlight needs

---

### 🧪 Unit Tests
Unit tests are implemented for the service layer (PlantService), covering:

- Filtering plants by owner
- Pagination logic
- Care feedback logic (soil and watering validation)

Technologies used:
- xUnit
- EF Core InMemory Database

---

### ⚙️ Error Handling
- Custom error pages:
  - 404 Not Found
  - 500 Server Error

---

## 🛠 Technologies Used

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- xUnit (Unit Testing)
- Bootstrap (UI)

---

## ▶️ How to Run

Follow these steps to run the application locally:

### 1. Clone the repository

### 2. Open the project
- Open the solution file in **Visual Studio 2022**

### 3. Configure the database
- Open **Package Manager Console**
- Run: Update-Database
       
This will create the database using Entity Framework Core migrations.

### 4. Run the application
- Press **F5** or click **Start**
- The application will open in your browser

### 5. Login or Register
- You can create a new user account
- Or use the admin account: 
- Email: superadmin@plantplanner.com
  Password: Admin123!

  
---

### 🧪 Run Unit Tests
- Open **Test Explorer**
- Click **Run All Tests**

---

### ⚠️ Notes
- Make sure SQL Server is running
- If migrations fail, try:
- Drop-Database
  Update-Database



