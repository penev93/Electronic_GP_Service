using Hospitology.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hospitology.Controllers
{
    public class GetFutureAppointmentsController : Controller
    {
       
       
   
   
        List<PatientAppointmentInsertGet> getPatientAppFutureData(PatientAppGetHistoryData data)
        {
            using (SqlConnection conn = new SqlConnection(Connection.ConnStr()))
            {
                //close it
                conn.Open();
                
                SqlDataAdapter adapter = new SqlDataAdapter();

                List<PatientAppointmentInsertGet> appList = new List<PatientAppointmentInsertGet>();

                DateTime now=DateTime.Now;
                string nowString = now.ToString("yyyy-MM-dd");
                string Q = "USE Hospitology;" +
                "SELECT d.last_name, h.patient_id,h.appointment_date,h.[description],h.[patient_pain] " +
                "FROM dbo.GP_Appointment_History h " +
                "FULL OUTER JOIN dbo.Doctors d ON d.id_doctor=h.doctor_id " +
                "WHERE patient_id=@PatientID and " +
                "(appointment_date >= @today)";

                adapter.SelectCommand = new SqlCommand(Q, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@today", nowString);
                
                
                adapter.SelectCommand.Parameters.AddWithValue("@PatientID", data.PatientID);
                

                DataTable tb = new DataTable();

                adapter.Fill(tb);
                foreach (DataRow appItem in tb.Rows)
                {
                    PatientAppointmentInsertGet app = new PatientAppointmentInsertGet();
                    app.IDDoctor = appItem["last_name"].ToString();
                    DateTime appTime = Convert.ToDateTime(appItem["appointment_date"]);
                    app.appointment_date = appTime.ToString("dd-MM-yyyy HH:mm");
                    app.description = appItem["description"].ToString();
                    app.patient_paint = appItem["patient_pain"].ToString();

                    appList.Add(app);
                }
                return appList;
            }
        }

        [HttpPost]
        public JsonResult getPatientFutureApp(PatientAppGetHistoryData user)
        {
            List<PatientAppointmentInsertGet> future = getPatientAppFutureData(user);
            return this.Json(future, JsonRequestBehavior.AllowGet);
        }

    }
}

 