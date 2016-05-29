using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospitology.Models
{
    public class PatientAppPostData
    {
        
        public string DoctorID {get;set;}
        public string Patient { get; set; }
        public string AppDate { get; set; }
        public string AppDatePlusOne { get; set; }
        
    }
}