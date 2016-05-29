using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Hospitology.Models
{
    public class DoctorViewModel
    {
        public static Expression<Func<Doctor, DoctorViewModel>> FromDoctor
        {
            get
            {
                return doctor => new DoctorViewModel
                {

                    IDDoctor = doctor.IDDoctor,
                    Username = doctor.Username,
                    Password = doctor.Password,
                    Firstname = doctor.Firstname,
                    Middlename = doctor.Middlename,
                    Lastname = doctor.Lastname,
                    MobilePhone = doctor.MobilePhone,
                    Phone = doctor.Phone,
                    City = doctor.City,
                    Address = doctor.Address,
                    Email = doctor.Email,

                };
            }
         
        }
        public string IDDoctor { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Specialization { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string MobilePhone { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        public string Email { get; set; }
    }
}