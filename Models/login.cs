using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sampleMVC.Models
{
    public class login
    {

        [Required(ErrorMessage = "MSID is required")]
        [Key]
        [Display(Name = "MSID")]
        public string MSID { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        public string pwd { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Project { get; set; }
        public string Location { get; set; }
        public string Branch { get; set; }
        public string Role { get; set; }

        public string Sub_Area { get; set; }
        public string supervisorName { get; set; }

        public string extraHrsLogReason { get; set; }

        public Int64 Emp_ID { get; set; }


        public bool checkUser(login log)
        {
            bool flag = false;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select count(*) from LTLogin where MSID='" + log.MSID + "' and pwd='" + log.pwd + "'", conn);
                flag = Convert.ToBoolean(cmd.ExecuteScalar());
                return flag;
            }
        }

        public login getInfo(login log)
        {

            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from LTLogin where MSID='" + log.MSID.ToUpper() + "' and pwd='" + log.pwd.ToUpper() + "'", conn);

                SqlDataReader sr = cmd.ExecuteReader();
                login objLogin = new login();
                if (sr.FieldCount != 0)
                {
                    while (sr.Read())
                    {
                        objLogin.FirstName = sr["FirstName"].ToString();
                        objLogin.LastName = sr["LastName"].ToString();
                        objLogin.Location = sr["Location"].ToString();
                        objLogin.Role = sr["Role"].ToString();
                        objLogin.Project = sr["Project"].ToString();
                        objLogin.Branch = sr["Branch"].ToString();
                        objLogin.Sub_Area = sr["Sub_Area"].ToString();
                        objLogin.Emp_ID = Convert.ToInt64(sr["Emp_ID"]);
                    }

                }
                return objLogin;
            }

        }

        public bool getPassword(string msid)
        {
            bool result = false;
            string checkPass = string.Empty;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select pwd from LTLogin where MSID ='" + msid + "'", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                login objLogin = new login();
                if (sr.Read())
                {
                    checkPass = sr["pwd"].ToString();
                    result = (checkPass.Equals("")) ? false : true;
                }
            }
            return result;
        }

        public int updateTempPassTODB(string msid, string tempPass)
        {
            int rowsAffected = 0;
            DataTable dt = new DataTable();
            string query = string.Empty;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                query = "update LTLogin set pwd = '" + tempPass + "' where MSID = '" + msid + "'";
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                cmd.CommandText = query;
                //cmd.CommandType = CommandType.Text;
                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected;
        }


        public string toGetEmailIDFromDB(string msid)
        {
            string result = string.Empty; ;

            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select Email_ID from LTEmailID where MSID='" + msid + "'", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                login objLogin = new login();
                if (sr.Read())
                {
                    result = sr["Email_ID"].ToString();
                }
            }
            return result;
        }


        public int updatePassword(string msid, string passwordToUpdate)
        {
            int rowsAffected = 0;
            DataTable dt = new DataTable();
            string query = string.Empty;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                query = "update LTLogin set pwd = '" + passwordToUpdate + "' where MSID = '" + msid + "'";
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                cmd.CommandText = query;
                //cmd.CommandType = CommandType.Text;
                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected;
        }

        public string validateMSID(string msid)
        {
            string result = string.Empty; ;

            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select MSID from LTLogin where MSID='" + msid + "'", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                login objLogin = new login();
                if (sr.Read())
                {
                    result = sr["MSID"].ToString();
                }
            }
            return result;
        }

        public int extraHrsLog(string MSID, string hrs, DateTime dateLogHrsFormat,string Reason)
        {
            int rowAffected = 0;
            decimal hrsExtra = 0;
            decimal totalHrs = 0;
            login objLogin = new login();
            hrsExtra = Convert.ToDecimal(hrs);
            totalHrs = 8 + hrsExtra;
            objLogin = toGetDetails(MSID);
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmdToInsert = new SqlCommand("insert into LTLogHrs values(@MSID,@FirstName,@LastName,@logDate,@extraHours,@totalHrs,@Project,@Location,@Branch,@SupervisorName,@Reason)", conn);
                cmdToInsert.Parameters.AddWithValue("@MSID", MSID);
                cmdToInsert.Parameters.AddWithValue("@FirstName", objLogin.FirstName);
                cmdToInsert.Parameters.AddWithValue("@LastName", objLogin.LastName);
                cmdToInsert.Parameters.AddWithValue("@logDate", dateLogHrsFormat);
                cmdToInsert.Parameters.AddWithValue("@extraHours", hrsExtra);
                cmdToInsert.Parameters.AddWithValue("@totalHrs", totalHrs);
                cmdToInsert.Parameters.AddWithValue("@Project", objLogin.Project);
                cmdToInsert.Parameters.AddWithValue("@Location", objLogin.Location);
                cmdToInsert.Parameters.AddWithValue("@Branch", objLogin.Branch);
                cmdToInsert.Parameters.AddWithValue("@Reason", Reason);
                cmdToInsert.Parameters.AddWithValue("@SupervisorName", objLogin.supervisorName);

                //cmd.Parameters.AddWithValue("@Reason", Reason);



                rowAffected = cmdToInsert.ExecuteNonQuery();

            }
            return rowAffected;
        }

        public login toGetDetails(string msid)
        {
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmdToSelect = new SqlCommand("select MSID,FirstName,LastName,Project,Location,Branch,SupervisorName from LTLogin where MSID='" + msid + "'", conn);
                SqlDataReader sr = cmdToSelect.ExecuteReader();
                login objLogin = new login();
                if (sr.Read())
                {
                    objLogin.MSID = sr["MSID"].ToString();
                    objLogin.FirstName = sr["FirstName"].ToString();
                    objLogin.LastName = sr["LastName"].ToString();
                    objLogin.Project = sr["Project"].ToString();
                    objLogin.Location = sr["Location"].ToString();
                    objLogin.Branch = sr["Branch"].ToString();
                    objLogin.supervisorName = sr["SupervisorName"].ToString();

                }
                return objLogin;
            }

        }
    }
}
