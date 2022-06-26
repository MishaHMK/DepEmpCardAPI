using System;
using System.Collections.Generic;

#nullable disable

namespace DepEmpCardAPI.Models
{
    public class Employee
    {
        public Employee()
        {
            PaymentDetails = new HashSet<PaymentDetail>();
        }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string PhotoFileName { get; set; }

        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
    }
}
