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
    public class GetPatientAppHistoryController : Controller
    {
        public static
        List<PatientAppointmentInsertGet> getPatientAppHistoryData(PatientAppGetHistoryData data)
        {
            using (SqlConnection conn = new SqlConnection(Connection.ConnStr()))
            {
                //close it
                conn.Open();
                
                SqlDataAdapter adapter = new SqlDataAdapter();

                List<PatientAppointmentInsertGet> appList = new List<PatientAppointmentInsertGet>();



                string Q = "USE Hospitology;" +
                "SELECT d.last_name, h.patient_id,h.appointment_date,h.[description],h.[patient_pain] " +
                "FROM dbo.GP_Appointment_History h " +
                "FULL OUTER JOIN dbo.Doctors d ON d.id_doctor=h.doctor_id " +
                "WHERE patient_id=@PatientID and " +
                "(appointment_date BETWEEN @from AND @to)";

                adapter.SelectCommand = new SqlCommand(Q, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@from", data.dateFrom);
                adapter.SelectCommand.Parameters.AddWithValue("@to", data.dateTo);
                
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
        public JsonResult getPatientAppHistory(PatientAppGetHistoryData user)
        {
            List<PatientAppointmentInsertGet> history = getPatientAppHistoryData(user);
            return this.Json(history, JsonRequestBehavior.AllowGet);
        }

    }
}
