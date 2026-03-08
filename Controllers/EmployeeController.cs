using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Controllers {
    public class EmployeeController : Controller {
        string conn = "Server=localhost;Database=EmployeeDB;User Id=sa;Password=Mgag@4798;TrustServerCertificate=True;";
        private bool IsAuthorized() => !string.IsNullOrEmpty(HttpContext.Session.GetString("User"));

        public IActionResult Login() => View();
        [HttpPost]
        public IActionResult Login(string user, string pass) {
            if (user == "admin" && pass == "mola2026") {
                HttpContext.Session.SetString("User", "admin");
                LogAction("Login", "Admin user accessed the system");
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Index() {
            if (!IsAuthorized()) return RedirectToAction("Login");
            return View(GetEmployees());
        }

        public IActionResult Analytics() {
            if (!IsAuthorized()) return RedirectToAction("Login");
            var logs = new List<string>();
            using (SqlConnection s = new SqlConnection(conn)) {
                s.Open();
                var c = new SqlCommand("SELECT TOP 10 Action, Details, Timestamp FROM AdminLogs ORDER BY Timestamp DESC", s);
                using (var r = c.ExecuteReader()) {
                    while (r.Read()) logs.Add($"{r["Timestamp"]:HH:mm} - {r["Action"]}: {r["Details"]}");
                }
            }
            ViewBag.Logs = logs;
            return View(GetEmployees());
        }

        [HttpPost]
        public IActionResult Hire(Employee emp) {
            ExecuteSql("INSERT INTO Employees (EmployeeID, Name, Department, Designation, Salary, HireDate) VALUES (" + new Random().Next(100,999) + ", '" + emp.Name + "', '" + emp.Department + "', '" + emp.Position + "', " + emp.Salary + ", GETDATE())");
            LogAction("Hire", $"Added new employee: {emp.Name}");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Promote(int id) {
            ExecuteSql($"UPDATE Employees SET Salary = Salary + 5000 WHERE EmployeeID = {id}");
            LogAction("Promotion", $"Gave $5k raise to ID #{id}");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id) {
            ExecuteSql($"DELETE FROM Employees WHERE EmployeeID = {id}");
            LogAction("Termination", $"Removed employee ID #{id}");
            return RedirectToAction("Index");
        }

        private void LogAction(string action, string details) {
            using (SqlConnection s = new SqlConnection(conn)) {
                s.Open();
                var c = new SqlCommand("INSERT INTO AdminLogs (Action, Details) VALUES (@a, @d)", s);
                c.Parameters.AddWithValue("@a", action);
                c.Parameters.AddWithValue("@d", details);
                c.ExecuteNonQuery();
            }
        }

        private void ExecuteSql(string sql) {
            using (SqlConnection s = new SqlConnection(conn)) { s.Open(); new SqlCommand(sql, s).ExecuteNonQuery(); }
        }

        private List<Employee> GetEmployees() {
            var list = new List<Employee>();
            using (SqlConnection s = new SqlConnection(conn)) {
                s.Open();
                var r = new SqlCommand("SELECT * FROM Employees", s).ExecuteReader();
                while (r.Read()) list.Add(new Employee { EmployeeID = (int)r["EmployeeID"], Name = r["Name"].ToString(), Department = r["Department"].ToString(), Position = r["Designation"].ToString(), Salary = (decimal)r["Salary"] });
            }
            return list;
        }
    }
}
