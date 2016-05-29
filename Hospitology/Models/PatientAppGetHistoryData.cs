using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospitology.Models
{
    public class PatientAppGetHistoryData
    {
        public string DoctorID { get; set; }
        public string PatientID { get;set; }
        public string dateFrom { get; set; }
        public string dateTo { get; set; }

    }
}