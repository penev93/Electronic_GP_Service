using Hospitology.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hospitology.Controllers.PatientAppointment
{
    public class InsertAppointmentController : Controller
    {
        [HttpPost]
        public bool InsertAppointmentIntoDB(PatientAppointmentInsertGet app)
        {
            using (SqlConnection conn = new SqlConnection(Connection.ConnStr()))
            {
               
                conn.Open();
                SqlTransaction myTrans = conn.BeginTransaction();
                SqlDataAdapter adapter = new SqlDataAdapter();
                //i Have to insert date into future_medicalCards
                adapter.InsertCommand = new SqlCommand("Use Hospitology;" +
                 "INSERT INTO dbo.GP_Appointment_History(doctor_id,patient_id,appointment_date,[description],patient_pain)" +
                " VALUES(@idDoctor,@idPatient,@app_date,@pain,@descripton);"
               // "INSERT INTO dbo.Fututre_MedicalCards(doctor_id,patient_id,appointment_date)"+
                //"VALUES(@idDoctor,@idPatient,@app_date);"
                , conn);

                adapter.InsertCommand.Transaction = myTrans;
                string idDoctor = app.IDDoctor;
                string idPatient = app.IDPatient;
                string appDate = app.appointment_date;
                string Description = app.description;
                string pain = app.patient_paint;
                adapter.InsertCommand.Parameters.AddWithValue("@idDoctor", idDoctor);
                adapter.InsertCommand.Parameters.AddWithValue("@idPatient", idPatient);
                adapter.InsertCommand.Parameters.AddWithValue("@app_date", appDate);
                adapter.InsertCommand.Parameters.AddWithValue("@pain", pain);
                adapter.InsertCommand.Parameters.AddWithValue("@descripton", Description);
                try
                {
                    int affectedRows = adapter.InsertCommand.ExecuteNonQuery();
                    if (affectedRows == 0)
                    {
                        myTrans.Rollback("myTrans");
                    }
                    else
                    {

                        myTrans.Commit();
                    }
                    conn.Close();
                }
                catch (Exception)
                {
                    return false;

                }
                return true;
            }
        }
    }
}
