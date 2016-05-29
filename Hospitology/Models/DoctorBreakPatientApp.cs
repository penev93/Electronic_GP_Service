using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospitology.Models
{
    public class DoctorBreakPatientApp
    {
        //patientID is going to be used to check if the current user is appointed a Pregled for that day
        public string patientID { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string regularBreakStart { get; set; }

        public string regularBreakEnd { get; set; }

        public string extraBreakStart { get; set; }
        public string extraBreakEnd { get; set; }
        public string appointmentStart { get; set; }
        public string appointmentEnd { get; set; }
    }
}