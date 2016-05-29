using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospitology.Models
{
    public class Patient
    {
        public string EGN { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string MobilePhone { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string DoctorID { get; set; }
        public string Email { get; set; }
    }
}