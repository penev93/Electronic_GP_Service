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
    class MyHashSet<T> : HashSet<T>
    {
        public T this[int index]
        {
            get
            {
                int i = 0;
                foreach (T t in this)
                {
                    if (i == index)
                        return t;
                    i++;
                }
                throw new IndexOutOfRangeException();
            }
        }
    }

    public class DoctorBreaksController : Controller
    {
        public List<string> getBreaks(PatientAppPostData user)
            {
                using (SqlConnection conn = new SqlConnection(Connection.ConnStr()))
                {
                    int counter = 0;
                    SqlDataAdapter adapter = new SqlDataAdapter();

                    MyHashSet<DoctorBreakPatientApp> breaks = new MyHashSet<DoctorBreakPatientApp>();


                    bool isPatient = false;
                    //да оправя заявката да изкарв заетите часове за съответния ден , почивните часове на доктора за съответния ден, и обичайните почивни часове

                    string Q = "USE Hospitology;" +
                        "SELECT d.start_time ,d.end_time, b.extra_break_start_time,b.extra_break_finish_time," +
                     "b.regular_break_start_time,b.regular_break_finish_time " +
                            "FROM dbo.Doctors_breaks b " +
                          "JOIN dbo.Doctors d ON (d.id_doctor=b.doctor_id) " +
                          "JOIN dbo.Patients p ON (d.id_doctor=p.doctor_id) " +
                          "WHERE  d.id_doctor=@DoctorID OR " +
                           "((b.extra_break_start_time BETWEEN @date AND @datePlusOne) OR " +
                           "(b.extra_break_finish_time BETWEEN  @date AND @datePlusOne)) OR " +
                           "(( b.regular_break_start_time BETWEEN '2000-10-10' AND '6666-10-10'));";

                    
                        /*"USE Hospitology;SELECT d.start_time ,d.end_time, b.extra_break_start_time,b.extra_break_finish_time," +
                     "b.regular_break_start_time,b.regular_break_finish_time,c.appointment_date,p.EGN_patient " +
                        " FROM dbo.Doctors_breaks b " +
                         " JOIN dbo.Doctors d ON (d.doctor_username=b.username_doctor)  JOIN dbo.Patients p ON (d.doctor_username=p.username_doctor) " +
                         " JOIN dbo.GP_Appointment_History c ON (d.doctor_username=c.username_doctor) " +
                            " WHERE d.doctor_username=@DoctorID AND ((b.extra_break_start_time BETWEEN @date AND @datePlusOne) AND (b.extra_break_finish_time BETWEEN @date AND @datePlusOne)) " +
                            "AND ((c.appointment_date BETWEEN @date AND @datePlusOne)) OR(( b.regular_break_start_time BETWEEN '2000-10-10' AND '6666-10-10'));";
                        */


                    adapter.SelectCommand = new SqlCommand(Q, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@date",user.AppDate);
                    adapter.SelectCommand.Parameters.AddWithValue("@DoctorID", user.DoctorID);
                    adapter.SelectCommand.Parameters.AddWithValue("@datePlusOne", user.AppDatePlusOne);
                    
                    DataTable tb = new DataTable();

                    adapter.Fill(tb);
                    foreach (DataRow breakItem in tb.Rows)
                    {
                        DoctorBreakPatientApp docBreak = new DoctorBreakPatientApp();
                       
                        docBreak.startTime = breakItem["start_time"].ToString();
                        docBreak.endTime = breakItem["end_time"].ToString();
                        docBreak.regularBreakStart = breakItem["regular_break_start_time"].ToString();
                        docBreak.regularBreakEnd = breakItem["regular_break_finish_time"].ToString();
                       
                        
                        docBreak.extraBreakStart = breakItem["extra_break_start_time"].ToString();
                        docBreak.extraBreakEnd = breakItem["extra_break_finish_time"].ToString();

                        //docBreak.patientID = breakItem["EGN_patient"].ToString();
                        
                        //add 20 min for one patient Appointmet TODO VALERIIIIIIIII
                        ///
                        //i have to rapair the query becouse it returns even the wrong appointment dates 
                        /*if (user.AppDate != breakItem["appointment_date"].ToString())
                        {
                            docBreak.appointmentStart = null;
                        }
                        else
                        {
                            docBreak.appointmentStart = breakItem["appointment_date"].ToString();
                        }
                        
                        */
                        breaks.Add(docBreak);
                    }

                    string secQ = "USE Hospitology;" +
                      "SELECT * FROM GP_Appointment_History WHERE doctor_id=@DoctorID AND appointment_date BETWEEN @date AND @datePlusOne";
                    adapter.SelectCommand = new SqlCommand(secQ, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@date", user.AppDate);
                    adapter.SelectCommand.Parameters.AddWithValue("@DoctorID", user.DoctorID);
                    adapter.SelectCommand.Parameters.AddWithValue("@datePlusOne", user.AppDatePlusOne);
                    tb = new DataTable();
                    adapter.Fill(tb);


                    foreach (DataRow breakItem in tb.Rows)
                    {
                        DoctorBreakPatientApp docBreak = new DoctorBreakPatientApp();
                        docBreak.appointmentStart = breakItem["appointment_date"].ToString();
                        docBreak.patientID = breakItem["patient_id"].ToString();
                        breaks.Add(docBreak);
                    }
                    
                     

                    //i WANT TO RETURN ONLY THE free hours for that Day if there are some
                    //start

                    DateTime workingDayStarts = Convert.ToDateTime(breaks[0].startTime);
                    DateTime workingDayEnd = Convert.ToDateTime(breaks[0].endTime);
                    
                    List<DateTime> dateList=new List<DateTime>();
                    counter = 0;
                    int dateCompareResult = DateTime.Compare(workingDayStarts, workingDayEnd);
                    //create list of working day scheduale
                    while ( dateCompareResult < 1 && dateCompareResult!=0 )
                    {
                        
                        if (counter==0)
                        {
                            dateList.Add(workingDayStarts);
                            ++counter;
                        }
                        else
                        {
                            workingDayStarts = workingDayStarts.AddMinutes(20);
                            dateCompareResult = DateTime.Compare(workingDayStarts, workingDayEnd);
                            if(dateCompareResult==0)
                            {
                                break;
                            }
                           
                            dateList.Add(workingDayStarts);
                        }
                        
                    }

                  
                   //Doctor regular break start
                    foreach (var item in breaks)
                    {


                        //regular breake Start

                        if (item.regularBreakStart==null || item.regularBreakStart=="")
                        {
                            continue ;
                        }
                        DateTime breakeS = Convert.ToDateTime(item.regularBreakStart);

                        int breakeSHours = breakeS.Hour * 60;
                        int breakeSMinutes = breakeS.Minute;
                        int breakeStart = breakeSHours + breakeSMinutes;

                        DateTime breakeE = Convert.ToDateTime(item.regularBreakEnd);

                        int breakeEHours = breakeE.Hour * 60;
                        int breakeEMinutes = breakeE.Minute;
                        int breakeEnd = breakeEHours + breakeEMinutes;

                        for (int i = dateList.Count - 1; i >= 0;i-- )
                        {
                            int currentTime = dateList[i].Hour * 60 + dateList[i].Minute;

                           
                            if ((currentTime >= breakeStart) && (breakeEnd > currentTime))
                            {

                                dateList.RemoveAt(i);
                            }

                            
                        }
                        
                        
                    }
                    //Doctor regular breake END



                                      
                   //Doctor regular break start
                    foreach (var item in breaks)
                    {

                        if(item.extraBreakStart==null || item.extraBreakStart=="")
                        {
                            continue;
                        }
                        //regular breake Start
                        DateTime breakeS = Convert.ToDateTime(item.extraBreakStart);

                        int breakeSHours = breakeS.Hour * 60;
                        int breakeSMinutes = breakeS.Minute;
                        int breakeStart = breakeSHours + breakeSMinutes;

                        DateTime breakeE = Convert.ToDateTime(item.extraBreakEnd);

                        int breakeEHours = breakeE.Hour * 60;
                        int breakeEMinutes = breakeE.Minute;
                        int breakeEnd = breakeEHours + breakeEMinutes;

                        for (int i = dateList.Count - 1; i >= 0;i-- )
                        {
                            int currentTime = dateList[i].Hour * 60 + dateList[i].Minute;

                           
                            if ((currentTime >= breakeStart) && (breakeEnd > currentTime))
                            {

                                dateList.RemoveAt(i);
                            }

                            
                        }
                        
                        
                    }
                    //Doctor regular breake END


                  

                    DateTime lastDate = new DateTime();
                    //Patient Appointment Start
                    foreach (var item in breaks)
                    {
                       

                        if(item.appointmentStart==null || item.appointmentStart=="")
                        {
                            continue;
                        }
                        DateTime appS = Convert.ToDateTime(item.appointmentStart);
                       
                        var date =appS.ToString("yyyy-MM-dd");
                        
                        if (user.Patient == item.patientID && user.AppDate==date)
                        {
                            isPatient = true;
                            return new List<string>();
                        }
                        
                        int appSHours = appS.Hour * 60;
                        int appSMinutes = appS.Minute;
                        int appStart = appSHours + appSMinutes;

                        DateTime appE = appS.AddMinutes(20);

                        int appEHours = appE.Hour * 60;
                        int appEMinutes = appE.Minute;
                        int appEnd = appEHours + appEMinutes;

                        for (int i = dateList.Count - 1; i >= 0; i--)
                        {
                            int currentTime = dateList[i].Hour * 60 + dateList[i].Minute;


                            if ((currentTime >= appStart) && (appEnd > currentTime))
                            {
                                if(lastDate==dateList[i])
                                {
                                    break;
                                }
                                dateList.RemoveAt(i);
                                lastDate = dateList[i];
                            }
                        }
                    }
                    //Patient Appointment End


                    List<string> dateStr = new List<string>();

                    for (int i = 0; i < dateList.Count; i++)
                    {
                        dateStr.Add(dateList[i].ToString("HH:mm"));
                    }

                    //end
                    if(isPatient)
                    {
                        //if the Patient have booked appointment
                        return new List<string>();
                    }
                    return dateStr;
                }

                
            }

        [HttpPost]
        public JsonResult getDoctorBreaks(PatientAppPostData user)
        {
            List<string> breaks = getBreaks(user);
            return this.Json(breaks, JsonRequestBehavior.AllowGet);
        }
    }
}
