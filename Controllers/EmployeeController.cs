using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using EmployeeManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace EmployeeManagement.Controllers {
    public class EmployeeController : Controller {
        string conn = "Server=localhost;Database=EmployeeDB;User Id=sa;Password=Mgag@4798;TrustServerCertificate=True;";

        public IActionResult Index(string searchId) {
            var list = GetEmployees();
            if (!string.IsNullOrEmpty(searchId)) list = list.Where(e => e.EmployeeID.ToString() == searchId).ToList();
            return View(list);
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
