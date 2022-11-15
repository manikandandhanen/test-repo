using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Entities
{
    public class Dashbord
    {
        public Dashbord()
        {

        }
        public int UserId { get; set; }

        public Status DocumentRequestStatus { get; set; }
        public Status EmployeeRequstStatus { get; set; }
    }

    public enum Status
    {
        None = 0,
        Pending = 1,
        Inprogress = 2,
        Submitted = 3
    }
}
