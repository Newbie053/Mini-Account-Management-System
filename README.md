# Mini Account Management System

A modular web-based accounting system built with **ASP.NET Core Razor Pages** and **SQL Server (Stored Procedures only)**. This system supports secure authentication, role-based authorization, and financial operations such as Chart of Accounts and Voucher management.

---

## 🚀 Features

- ✔️ User registration & login with ASP.NET Identity
- ✔️ Role-based access control (Admin/User)
- ✔️ Chart of Accounts management (Add/Edit/Delete)
- ✔️ Voucher entry with multiple debit/credit lines
- ✔️ Stored procedures used for all data operations
- ✔️ Razor Pages-based UI with Bootstrap styling

---

## 📸 Screenshots

### 🏠 Home Page  
![Home](screenshots/home.png)

### 📘 Manage Roles
![Manage Roles](screenshots/manage-role.png)

### 📘 Manage Users
![Manage Users](screenshots/manage-user.png)

### 📥 Create Voucher  
![Create Voucher](screenshots/create-voucher.png)

### 📄 Voucher Details  
![Voucher Details](screenshots/voucher-details.png)

### 📘 Chart of Accounts  
![Chart of Accounts](screenshots/chart-of-accounts.png)

---

## 🧱 Technologies Used

- **ASP.NET Core Razor Pages**
- **SQL Server (Stored Procedures)**
- **Entity Framework Core (for Identity only)**
- **Bootstrap 5**
- **Visual Studio 2022**

---

## ⚙️ Setup Instructions

### 🛠 Prerequisites

- .NET 7 SDK
- SQL Server 2019+
- Visual Studio 2022+

### 📦 Database Setup

1. Create a database named `MiniAccountsDB`.
2. Run the stored procedures from the `/SQLScripts/` folder.
3. Update the `appsettings.json` with your SQL Server connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=MiniAccountsDB;Trusted_Connection=True;"
}



