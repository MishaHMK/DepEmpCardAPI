using System;

namespace DepEmpCardAPI.Models
{
    public class EmployeeDTO
    {
       public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string PhotoFileName { get; set; }
    }
}
