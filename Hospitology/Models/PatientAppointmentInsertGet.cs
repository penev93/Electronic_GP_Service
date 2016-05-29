using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospitology.Models
{
    public class PatientAppointmentInsertGet
    {
        public string IDDoctor { get; set; }
        public string IDPatient { get; set; }
        public string appointment_date { get; set; }
        public string patient_paint { get; set; }
        public string description { get; set; }

    }
}