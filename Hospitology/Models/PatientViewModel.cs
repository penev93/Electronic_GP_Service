using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Hospitology.Models
{
    public class PatientViewModel
    {
        public static Expression<Func<PatientAndDoctor, PatientViewModel>> FromPatients
        {
            get
            {
                return patient => new PatientViewModel
                {
                    EGN = patient.EGN,
                    DoctorID = patient.DoctorID,
                    Username = patient.Username,
                    Password = patient.Password,
                    Firstname = patient.Firstname,
                    Middlename = patient.Middlename,
                    Lastname = patient.Lastname,
                    MobilePhone = patient.MobilePhone,
                    Phone = patient.Phone,
                    City = patient.City,
                    Address = patient.Address,
                    Email = patient.Email,
                    
                };
            }
        }
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