namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? Designation { get; set; }
        public decimal Salary { get; set; }

        // LEGACY MAPPING: These now allow both Reading and Writing
        private string? _name;
        public string Name 
        { 
            get => string.IsNullOrEmpty(_name) ? $"{FirstName} {LastName}" : _name; 
            set => _name = value; 
        }

        private string? _pos;
        public string Position 
        { 
            get => string.IsNullOrEmpty(_pos) ? Designation ?? "Staff" : _pos; 
            set => _pos = value; 
        }

        public string? Department { get; set; }
    }
}
