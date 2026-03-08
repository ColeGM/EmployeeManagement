# MOLA Enterprise Resource Planning (ERP)

A high-performance **Employee Management System** built with **ASP.NET Core MVC** and **MS SQL Server**, hosted on **Ubuntu 24.04**.

## 🚀 Key Features
- **Full CRUD Logic:** Add, view, and terminate employee records.
- **Enterprise Analytics:** Real-time payroll calculation and tenure tracking.
- **Modern UI/UX:** Glassmorphism design with a dynamic **Dark Mode** toggle.
- **Reporting:** Export full staff ledgers to CSV for administrative use.
- **Secure Backend:** Parameterized SQL queries to prevent SQL injection.

## 🛠️ Tech Stack
- **Framework:** .NET 8.0 (C#)
- **Database:** Microsoft SQL Server 2022 (Linux Edition)
- **Frontend:** Bootstrap 5, SVG Graphics, CSS3 Animations
- **OS:** Ubuntu 24.04 LTS (WSL2)

## 📸 How to Run
1. Ensure MSSQL is running on your Linux host.
2. Clone the repo: `git clone https://github.com/ColeGM/EmployeeManagement.git`
3. Update the connection string in `Controllers/EmployeeController.cs`.
4. Run `dotnet run` and navigate to `http://localhost:5016`.
