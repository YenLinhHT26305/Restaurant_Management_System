# Restaurant Management System

A desktop-based **Restaurant Management System** developed using **C# and .NET Windows Forms**. The project applies **Three-tier Architecture** to separate the user interface, business logic, and data access layers, with **Microsoft SQL Server** as the database.

The project was developed as part of a **Windows Programming** course, focusing on practical software development, database connectivity, system architecture, and business process implementation.

## 📌 Project Overview

The system is designed to support common restaurant management operations, including menu management, employee management, invoice processing, and revenue reporting.

The project focuses on applying **C#/.NET programming**, **Windows Forms**, **SQL Server**, and **three-tier architecture** to build a structured and maintainable desktop application.

## 🎯 Objectives

* Develop a functional restaurant management desktop application.
* Apply **C# and .NET** to build the application.
* Design a user-friendly interface using **Windows Forms**.
* Apply **Three-tier Architecture** to separate application responsibilities.
* Connect and interact with **SQL Server** through data access technologies.
* Implement common database operations such as `INSERT`, `UPDATE`, `DELETE`, and `SELECT`.
* Manage restaurant data including menu items, categories, employees, invoices, and revenue.
* Study and compare **ADO.NET and Entity Framework** for database access.

## ✨ Main Features

### 👤 User & Employee Management

* Manage employee information.
* Manage user accounts and roles.
* Support different levels of access based on user roles.

### 🍽️ Menu Management

* Manage food and beverage categories.
* Add, update, delete, and search menu items.
* Manage product information and prices.

### 🧾 Invoice Management

* Create and manage invoices.
* Add products to invoices.
* Calculate invoice totals.
* Store invoice and transaction details.

### 📊 Revenue Management

* Retrieve and summarize sales data.
* Generate revenue statistics.
* Support business performance monitoring.

### 🗄️ Database Management

* Design relational database structures.
* Define primary keys and foreign keys.
* Maintain relationships and data integrity.
* Perform CRUD operations using SQL Server.

## 🏗️ System Architecture

The application follows a **Three-tier Architecture**, consisting of three main layers:

```text
┌──────────────────────────────┐
│     Presentation Layer      │
│       Windows Forms         │
└──────────────┬───────────────┘
               │
┌──────────────▼───────────────┐
│     Business Logic Layer    │
│       Business Rules        │
└──────────────┬───────────────┘
               │
┌──────────────▼───────────────┐
│      Data Access Layer      │
│    ADO.NET / Entity         │
│        Framework             │
└──────────────┬───────────────┘
               │
┌──────────────▼───────────────┐
│       SQL Server Database   │
└──────────────────────────────┘
```

### Presentation Layer

The **Presentation Layer** is developed using **Windows Forms** and provides the graphical user interface for users to interact with the system.

### Business Logic Layer

The **Business Logic Layer (BLL)** handles business rules, validation, data processing, and application workflows.

### Data Access Layer

The **Data Access Layer (DAL)** is responsible for connecting the application to **SQL Server** and performing database operations using **ADO.NET and/or Entity Framework**.

## 🗃️ Database

The system uses **Microsoft SQL Server** as the relational database management system.

The database contains entities related to restaurant operations, such as:

* `Users`
* `Staff`
* `Category`
* `Products`
* `Invoices`
* `InvoiceDetails`
* and other supporting entities.

The database is designed with appropriate **primary keys, foreign keys, relationships, and constraints** to maintain data consistency.

## 💻 Technologies

| Technology           | Purpose                                    |
| -------------------- | ------------------------------------------ |
| **C#**               | Application development and business logic |
| **.NET**             | Application framework                      |
| **Windows Forms**    | Desktop user interface                     |
| **SQL Server**       | Relational database management             |
| **ADO.NET**          | Database connectivity and data access      |
| **Entity Framework** | ORM and database access                    |
| **Visual Studio**    | Development environment                    |

## 📂 Project Structure

```text
## 📂 Project Structure

```text
Restaurant-Management-System/
│
├── BS layer/                   # Business Logic Layer
├── DB layer/                   # Data Access Layer
├── Interface/                  # Windows Forms interfaces
├── Usercontrol/                # Reusable UserControls
├── Food/                       # Food and menu-related components
├── Picture/                    # Images and UI resources
├── Properties/                 # Project properties and resources
│
├── Database/                   # SQL Server scripts
├── Documentation/             # Project report and documentation
│
├── App.config                 # Application configuration
├── packages.config            # NuGet package configuration
├── Program.cs                 # Application entry point
│
├── Form1.cs
├── Form1.Designer.cs
├── frmMain.cs
├── frmMain.Designer.cs
├── frmMain.resx
│
├── Restaurant_Management_System.csproj
└── Restaurant_Management_System.sln
```

## ⚙️ How to Run

### 1. Clone the repository

```bash
git clone https://github.com/YenLinhHT26305/Restaurant-Management-System.git
```

### 2. Set up the database

Open **SQL Server Management Studio (SSMS)** and execute the SQL script located in:

```text
Database/
```

This will create the required database, tables, relationships, and initial data.


### 3. Open the project

Open the solution file using **Visual Studio**:

```text
RestaurantManagement.sln
```

Restore required packages, build the solution, and run the application.

## 📚 Documentation

Project documentation, system analysis, database design, UML/ERD diagrams, and the project report are available in:

```text
Documentation/
```

## 👩‍💻 My Contribution

My main responsibilities in this project included:

* Analyzed restaurant management requirements and business workflows.
* Designed the **relational database structure** and entity relationships.
* Implemented database operations using **SQL Server**.
* Developed application functionality using **C# and .NET**.
* Built and integrated **Windows Forms** interfaces.
* Implemented the connection between the application and SQL Server.
* Applied **Three-tier Architecture** to organize the application.
* Worked with **ADO.NET and Entity Framework** for database access.

## 🎓 Learning Outcomes

Through this project, I gained practical experience in:

* **C# / .NET application development**
* **Windows Forms**
* **Three-tier Architecture**
* **SQL Server and relational database design**
* **ADO.NET**
* **Entity Framework**
* Database CRUD operations
* Business logic implementation
* Software architecture and code organization

## 🚀 Project Focus

The main focus of this project is **C#/.NET desktop application development**, combined with **database management and three-tier software architecture**.

The development workflow can be summarized as:

**Requirements Analysis → Database Design → SQL Server → Data Access → Business Logic → C# Windows Forms**

---

**Tech Stack:** C# · .NET · Windows Forms · SQL Server · ADO.NET · Entity Framework · Visual Studio
