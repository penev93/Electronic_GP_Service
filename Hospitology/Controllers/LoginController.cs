using Hospitology.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Hospitology.Controllers
{
    public class LoginController : Controller
    {
        public dynamic UserCheck(User u)
        {
            using (SqlConnection conn = new SqlConnection(Connection.ConnStr()))
            {

                // TO CHECK IF IM SUPER USER AND REGISTER NEW DOCTORS
                SqlDataAdapter adapter = new SqlDataAdapter();
                string EncryptPassword = Encrypt(u.Password);

                string Q = "Use Hospitology;" +
                        "DECLARE @x int " +
                        "SET @x=(SELECT dbo.[User].isDoctor FROM dbo.[User] WHERE (dbo.[User].username=@userName) AND( dbo.[User].[password]=@pass)) " +
                             "IF(@x)=1 " +
                         "BEGIN " +
                         "SELECT * FROM dbo.Doctors WHERE dbo.Doctors.username=@userName " +
                         "END " +
                         "ELSE IF(@x)=0 " +
                        " BEGIN " +
                         "SELECT P.*,D.* FROM Patients as P INNER JOIN Doctors as D ON p.doctor_id=D.id_doctor WHERE p.username=@userName " +
                         "END " +
                         "ELSE IF(@x)=2 " +
                         "BEGIN " +
                         "SELECT * FROM dbo.[Administrator] WHERE dbo.[Administrator].username=@userName END; ";

 

                adapter.SelectCommand = new SqlCommand(Q, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@userName", u.Username);
                adapter.SelectCommand.Parameters.AddWithValue("@pass", EncryptPassword);
                DataTable tb = new DataTable();

                adapter.Fill(tb);

                PatientAndDoctor patient = new PatientAndDoctor();
                Doctor doctor = new Doctor();
                Addministrator admin = new Addministrator();

                foreach (DataRow user in tb.Rows)
                {
                    if (tb.Columns.Contains("EGN"))
                    {
                        //Patient Data
                        //EGN is the new username
                        patient.Username = user["username"].ToString();
                        patient.EGN = user["EGN"].ToString();
                        patient.Password = Decrypt(user["password"].ToString());
                        patient.Firstname = user["first_name"].ToString();
                        patient.Middlename = user["middle_name"].ToString();
                        
                        patient.Lastname = user["last_name"].ToString();
                        patient.MobilePhone = user["mobile_phone_number"].ToString();
                        patient.Phone = user["phone_number"].ToString();
                        patient.City = user["city"].ToString();
                        patient.Address = user["address"].ToString();
                        //doctor_id not username_doctor
                        patient.DoctorID = user["id_doctor"].ToString();
                        patient.Email = user["email"].ToString();
                        //Doctor info
                        patient.docLastname = user["last_name"].ToString();

                        //Start get the date string and convert it to differrent format
                            
                        DateTime s=Convert.ToDateTime(user["start_time"]);
                        patient.StartTime = s.ToString("HH:mm");
                        DateTime e=Convert.ToDateTime(user["end_time"]);
                        patient.EndTime = e.ToString("HH:mm");

                        //END get the date string and convert it to differrent format

                        patient.docMobilePhone = user["mobile_phone_number"].ToString();
                        patient.docPhone = user["phone_number"].ToString();
                        patient.docCity = user["city"].ToString();
                        patient.docAddress = user["address"].ToString();
                    }
                    else if (tb.Columns.Contains("id_doctor"))
                    {
                        //if its Doctor i should add query to add all the date for his current day Apppointments

                        //Doctor Data
                        doctor.IDDoctor = user["id_doctor"].ToString();
                       doctor.Username = user["username"].ToString();
                        doctor.Password = Decrypt(user["password"].ToString());
                        doctor.Specialization = user["specialization"].ToString();
                        doctor.Firstname = user["first_name"].ToString();
                        doctor.Middlename = user["middle_name"].ToString();
                        doctor.Lastname = user["last_name"].ToString();
                        doctor.StartTime = user["start_time"].ToString();
                        doctor.EndTime = user["end_time"].ToString();
                        doctor.MobilePhone = user["mobile_phone_number"].ToString();
                        doctor.Phone = user["phone_number"].ToString();
                        doctor.City = user["city"].ToString();
                        doctor.Address = user["address"].ToString();
                        doctor.Email = user["email"].ToString();
                     doctor.Picture = user["picture"].ToString();

                    }
                    else if (tb.Columns.Contains("Name"))
                    {
                        //AZ SUM ADMIN
                        admin.AdminName = user["adminstrator_username"].ToString();
                        admin.Password = Decrypt(user["Password"].ToString());
                    }

                }
                //I Don't want to return List<User> but for now i want to grind code
                //LATER TODO---->>>> CHECK BETTER WAY ! For no such user
                dynamic result = new List<User>();
                if (doctor.Firstname != null)
                {
                    result = doctor;
                }
                else if (patient.Username != null)
                {
                    result = patient;
                }
                else if (admin.AdminName != null)
                {
                    result = admin;
                }

                return result;
            }
        }


        [HttpPost]
        public JsonResult getUser(User user)
        {
            //TODO

            var userLog = UserCheck(user);
            /*
            if (userLog.IDDoctor != "")
            {
                userLog = userLog.AsQueryable().Select(DoctorViewModel.FromDoctor);
            }
            else if(userLog.EGN!="")
            {
                userLog = userLog.AsQueryable().Select(PatientViewModel.FromPatients);
            }
            else if(userLog.Name!="")
            {
                userLog = userLog.AsQueryable().Select(PatientViewModel.FromPatients);
            }
             */
            return this.Json(userLog, JsonRequestBehavior.AllowGet);
        }

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                //Gets or sets the secret key for the symmetric algorithm.
                encryptor.Key = pdb.GetBytes(32);
                //Gets or sets the initialization vector (IV) for the symmetric algorithm
                encryptor.IV = pdb.GetBytes(16);
                //Create the streams used for encryption. 
                using (MemoryStream ms = new MemoryStream())
                {
                    //CryptoStream->    Defines a stream that links data streams to cryptographic transformations.
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

    }
}
