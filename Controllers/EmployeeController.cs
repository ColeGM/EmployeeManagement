using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using EmployeeManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace EmployeeManagement.Controllers {
    public class EmployeeController : Controller {
        string conn = "Server=localhost;Database=EmployeeDB;User Id=sa;Password=Mgag@4798;TrustServerCertificate=True;";

        // Simple Session-based Login (Check)
        private bool IsAuthorized() => !string.IsNullOrEmpty(HttpContext.Session.GetString("User"));

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string user, string pass) {
            if (user == "admin" && pass == "mola2026") { // Hardcoded for this demo
                HttpContext.Session.SetString("User", "admin");
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Invalid Credentials";
            return View();
        }

        public IActionResult Index(string searchId) {
            if (!IsAuthorized()) return RedirectToAction("Login");
            var list = GetEmployees();
            if (!string.IsNullOrEmpty(searchId)) list = list.Where(e => e.EmployeeID.ToString() == searchId).ToList();
            return View(list);
        }

        [HttpPost]
        public IActionResult Hire(Employee emp) {
            using (SqlConnection s = new SqlConnection(conn)) {
                s.Open();
                string sql = "INSERT INTO Employees (EmployeeID, Name, Department, Designation, Salary, HireDate) VALUES (@id, @n, @d, @p, @s, @h)";
                SqlCommand c = new SqlCommand(sql, s);
                c.Parameters.AddWithValue("@id", new Random().Next(100, 999));
                c.Parameters.AddWithValue("@n", emp.Name);
                c.Parameters.AddWithValue("@d", emp.Department);
                c.Parameters.AddWithValue("@p", emp.Position);
                c.Parameters.AddWithValue("@s", emp.Salary);
                c.Parameters.AddWithValue("@h", DateTime.Now);
                c.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Promote(int id) {
            using (SqlConnection s = new SqlConnection(conn)) {
                s.Open();
                SqlCommand c = new SqlCommand("UPDATE Employees SET Salary = Salary + 5000, Designation = 'Senior ' + Designation WHERE EmployeeID = @id", s);
                c.Parameters.AddWithValue("@id", id);
                c.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id) {
            using (SqlConnection s = new SqlConnection(conn)) {
                s.Open();
                SqlCommand c = new SqlCommand("DELETE FROM Employees WHERE EmployeeID = @id", s);
                c.Parameters.AddWithValue("@id", id);
                c.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        private List<Employee> GetEmployees() {
            List<Employee> list = new List<Employee>();
            using (SqlConnection s = new SqlConnection(conn)) {
                s.Open();
                SqlCommand c = new SqlCommand("SELECT * FROM Employees", s);
                using (SqlDataReader r = c.ExecuteReader()) {
                    while (r.Read()) {
                        list.Add(new Employee {
                            EmployeeID = (int)r["EmployeeID"],
                            Name = r["Name"].ToString(),
                            Department = r["Department"].ToString(),
                            Position = r["Designation"].ToString(),
                            Salary = Convert.ToDecimal(r["Salary"]),
                            HireDate = Convert.ToDateTime(r["HireDate"])
                        });
                    }
                }
            }
            return list;
        }
    }
}
