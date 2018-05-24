using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;
using System.Web.Mvc;

namespace sampleMVC.Models
{
    public class ApplyLeave
    {
        #region
        [Required(ErrorMessage = "MSID is required")]
        [Display(Name = "MSID")]
        public string MSID { get; set; }

        public int sNo { get; set; }


        [Required(ErrorMessage = "Start Date is required")]
        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }


        [Required(ErrorMessage = "Please provide Type of Leave")]
        [Display(Name = "Type Of Leave")]
        public string TypeOfLeaves { get; set; }

        [Required(ErrorMessage = "Please provide Total Leaves")]
        [Display(Name = "Total Leaves")]
        public int? TotalLeaves { get; set; }

        [Display(Name = "Reason (Optional)")]
        public string Reason { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string supervisorName { get; set; }

        public string Status { get; set; }
        [Required(ErrorMessage = "Please upload Ultimatix leave Screenshot in .jpg or .png format")]
        public HttpPostedFileBase screenShot { get; set; }
        public string imageSrc { get; set; }



        public byte[] imageSrc11 { get; set; }

        [Display(Name = "Enter Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter new password")]
        public string Pwd { get; set; }

        [Display(Name = "Confirm new password")]
        [Required(ErrorMessage = "Enter Confirm Password")]
        public string PwdCnf { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Enter Address")]
        public string add { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [Display(Name = "Select Date")]
        [DataType(DataType.DateTime)]
        public DateTime dateForHrsLog { get; set; }

        [Display(Name = "Please log your Extra Hours")]
        public string logExtraHrs { get; set; }

        [Display(Name = "Activity/Reason")]
        [Required(ErrorMessage = "Please enter Reason")]
        public string ExtraHrsReason { get; set; }

        // MSID Finder
        [Display(Name = "Enter Name Please")]
        [Required(ErrorMessage = "Enter associate Name")]
        public string MSIDfinder { get; set; }
        public int emp_ID { get; set; }

        public string Location { get; set; }
        public string Branch { get; set; }
        public string ProjectName { get; set; }



        public string _Status { get; set; }
        public string custManaName { get; set; }
        public string expLevel { get; set; }
        public string subArea { get; set; }
        public List<ApplyLeave> editScreenShotTLList { get; set; }
        public List<ApplyLeave> ExportToExcelForTLList { get; set; }
        public List<ApplyLeave> ExportToExcelForUserList { get; set; }
        public List<ApplyLeave> MSIDList { get; set; }
        #endregion
        public ApplyLeave()
        {
            Status = "Approval Pending";
        }
        public IEnumerable<SelectListItem> TOL { get; set; }
        public List<ApplyLeave> userInfoDetails { get; set; }


        public List<ApplyLeave> LeaveList { get; set; }
        public static List<ApplyLeave> editLeaveList { get; set; }

        public bool ApplyLeaves(string MSID, DateTime StartDate, DateTime EndDate, string TypeOfLeaves, int? TotalLeaves, string Reason, string status, byte[] imageData)
        {
            bool flag = false;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                //SqlCommand cmd = new SqlCommand("insert into LTLeaveDetails values('" + MSID + "','" + StartDate + "','" + EndDate + "','" + TypeOfLeaves + "'," + TotalLeaves + ",'" + Reason + "','" + status + "',cast('"+ imageData +"' as varbinary(max)))", conn);
                SqlCommand cmd = new SqlCommand("insert into LTLeaveDetails values(@MSID,@StartDate,@EndDate,@TypeOfLeaves,@TotalLeaves,'" + Reason + "',@status,@imageData,GetDate())", conn);
                cmd.Parameters.AddWithValue("@MSID", MSID);
                cmd.Parameters.AddWithValue("@StartDate", StartDate);
                cmd.Parameters.AddWithValue("@EndDate", EndDate);
                cmd.Parameters.AddWithValue("@TypeOfLeaves", TypeOfLeaves);
                cmd.Parameters.AddWithValue("@TotalLeaves", TotalLeaves);
                //cmd.Parameters.AddWithValue("@Reason", Reason);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@imageData", imageData);
                flag = Convert.ToBoolean(cmd.ExecuteNonQuery());
                return flag;

            }
        }


        public bool ApplyLeavesSS(int id, string Reason, byte[] imageData)
        {
            bool flag = false;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                //SqlCommand cmd = new SqlCommand("insert into LTLeaveDetails values('" + MSID + "','" + StartDate + "','" + EndDate + "','" + TypeOfLeaves + "'," + TotalLeaves + ",'" + Reason + "','" + status + "',cast('"+ imageData +"' as varbinary(max)))", conn);
                SqlCommand cmd = new SqlCommand("update LTLeaveDetails set TOL = 'Sick/UnPlanned', Reason = '" + Reason + "' , screenShot = @imageData,modifiedDate = GetDate() where SequenceNo = " + id + "", conn);
                //    update LTLeaveDetails set StartDate = @StartDate, EndDate = @EndDate "
                //    + ", TOL = @TypeOfLeaves, Reason = '" + Reason + "', _Status = 'Approval Pending', screenShot = @imageData where SequenceNo =" + id + ""
                //cmd.Parameters.AddWithValue("@Reason", Reason);

                cmd.Parameters.AddWithValue("@imageData", imageData);

                flag = Convert.ToBoolean(cmd.ExecuteNonQuery());
                return flag;

            }
        }

        public List<ApplyLeave> getLeave(string Branch)
        {
            List<ApplyLeave> list = new List<ApplyLeave>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"select t2.SequenceNo,t1.FirstName, t1.LastName, t2.StartDate, 
                                                t2.EndDate, t2.Reason, t2.TOL, t2.TotalDays,t2.screenShot
                                                from LTLogin as t1 inner join LTLeaveDetails as t2 
                                                on t1.MSID = t2.MSID where t2._Status = 'Approval Pending' and t1.Sub_Area='" + Branch + "' order by t2.StartDate desc", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    ApplyLeave obj = new ApplyLeave();
                    obj.sNo = Convert.ToInt32(sr["SequenceNo"]);
                    obj.FirstName = sr["FirstName"].ToString();
                    obj.LastName = sr["LastName"].ToString();
                    obj.StartDate = Convert.ToDateTime(sr["StartDate"]);
                    obj.EndDate = Convert.ToDateTime(sr["EndDate"]);
                    obj.TypeOfLeaves = sr["TOL"].ToString();
                    obj.TotalLeaves = Convert.ToInt16(sr["TotalDays"]);
                    obj.Reason = sr["Reason"].ToString();
                    obj.imageSrc11 = sr["screenShot"] as byte[] ?? null;
                    list.Add(obj);
                }

            }
            return list;
        }


        public List<ApplyLeave> getLeaveList(string MSID)
        {
            List<ApplyLeave> list = new List<ApplyLeave>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select * from LTLeaveDetails where MSID='" + MSID + "' order by StartDate desc ", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    ApplyLeave obj = new ApplyLeave();
                    obj.StartDate = Convert.ToDateTime(sr["StartDate"]);
                    obj.EndDate = Convert.ToDateTime(sr["EndDate"]);
                    obj.TypeOfLeaves = sr["TOL"].ToString();
                    obj.TotalLeaves = Convert.ToInt16(sr["TotalDays"]);
                    obj.Reason = sr["Reason"].ToString();
                    obj.sNo = Convert.ToInt32(sr["SequenceNo"]);
                    obj.imageSrc11 = sr["screenShot"] as byte[] ?? null;
                    obj.Status = sr["_Status"].ToString();
                    list.Add(obj);
                }

            }
            return list;
        }

        public static ApplyLeave getLeaveInfo(int id)
        {
            ApplyLeave obj = new ApplyLeave();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from LTLeaveDetails where SequenceNo=" + id + "", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {

                    obj.imageSrc11 = sr["screenShot"] as byte[] ?? null;
                    obj.StartDate = Convert.ToDateTime(sr["StartDate"]);
                    obj.EndDate = Convert.ToDateTime(sr["EndDate"]);
                    obj.Status = sr["_Status"].ToString();
                }

            }
            return obj;
        }



        public static ApplyLeave getLeaveInfoForEditing(int id)
        {
            ApplyLeave obj = new ApplyLeave();
            List<ApplyLeave> listForLeaveEditing = new List<ApplyLeave>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from LTLeaveDetails where SequenceNo=" + id + "", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    obj.MSID = sr["MSID"].ToString();
                    obj.imageSrc11 = sr["screenShot"] as byte[] ?? null;
                    obj.StartDate = Convert.ToDateTime(sr["StartDate"]);
                    obj.EndDate = Convert.ToDateTime(sr["EndDate"]);
                    obj.TypeOfLeaves = sr["TOL"].ToString();
                    obj.TotalLeaves = Convert.ToInt32(sr["TotalDays"]);
                    obj.Status = sr["_Status"].ToString();
                    listForLeaveEditing.Add(obj);
                }

            }
            return obj;
        }

        public static List<ApplyLeave> editScreenSHotForEditing(int id)
        {
            ApplyLeave obj = new ApplyLeave();
            List<ApplyLeave> listForLeaveEditing = new List<ApplyLeave>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from LTLeaveDetails where SequenceNo=" + id + "", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    obj.MSID = sr["MSID"].ToString();
                    obj.imageSrc11 = sr["screenShot"] as byte[] ?? null;
                    obj.StartDate = Convert.ToDateTime(sr["StartDate"]);
                    obj.EndDate = Convert.ToDateTime(sr["EndDate"]);
                    obj.TypeOfLeaves = sr["TOL"].ToString();
                    obj.TotalLeaves = Convert.ToInt32(sr["TotalDays"]);
                    obj.Status = sr["_Status"].ToString();
                    listForLeaveEditing.Add(obj);
                }

            }
            return listForLeaveEditing;
        }

        public ApplyLeave UpdateeditScreenShot(int id)
        {
            ApplyLeave obj = new ApplyLeave();
            List<ApplyLeave> listForLeaveEditing = new List<ApplyLeave>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from LTLeaveDetails where SequenceNo=" + id + "", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    obj.MSID = sr["MSID"].ToString();
                    obj.imageSrc11 = sr["screenShot"] as byte[] ?? null;
                    obj.StartDate = Convert.ToDateTime(sr["StartDate"]);
                    obj.EndDate = Convert.ToDateTime(sr["EndDate"]);
                    obj.TypeOfLeaves = sr["TOL"].ToString();
                    obj.TotalLeaves = Convert.ToInt32(sr["TotalDays"]);
                    obj.Status = sr["_Status"].ToString();

                }

            }
            return obj;
        }

        public static bool deleteLeave(int id)
        {
            bool flag = false;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Delete LTLeaveDetails where SequenceNo= " + id + "", conn);
                flag = Convert.ToBoolean(cmd.ExecuteNonQuery());
                return flag;
            }
        }


        public IEnumerable<string> getTypeOfLeave()
        {
            return new List<string>
            {
                "Planned",
                "Unplanned",
                "Sick"
            };
        }

        public IEnumerable<SelectListItem> GetSelectListItem(IEnumerable<string> parameters)
        {
            var seletList = new List<SelectListItem>();
            foreach (var param in parameters)
            {
                seletList.Add(new SelectListItem
                {
                    Value = param,
                    Text = param
                });
            }
            return seletList;
        }

        public int gettotoal(DateTime sd, DateTime ed)
        {

            return 5;
        }

        public Dictionary<string, string> toGetEmailDetails(string MSID)
        {
            //  Dictionary<Dictionary<string, string>, Dictionary<string, string>> completeInfo = new Dictionary<Dictionary<string, string>, Dictionary<string, string>>();
            Dictionary<string, string> toGetEmailIDDetailsFromDB = new Dictionary<string, string>();
            //  Dictionary<string, string> toGetNameDetailsFromDB = new Dictionary<string, string>();
            string ToMail = string.Empty;
            string FromMail = string.Empty;

            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select  c.Email_ID,b.SupervisorEmailID
                                    from LTLogin as a join LTSupervisor as b
                                     on a.SupervisorName = b.SupervisorName
                                    join LTEmailID as c on c.MSID = a.MSID
                                    where a.MSID = '" + MSID + "'", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    FromMail = Convert.ToString(sr["Email_ID"]);
                    ToMail = Convert.ToString(sr["SupervisorEmailID"]);

                }
                toGetEmailIDDetailsFromDB.Add(FromMail, ToMail);

            }
            return toGetEmailIDDetailsFromDB;
        }

        public Dictionary<string, string> toGetNameDetails(string MSID)
        {
            //  Dictionary<Dictionary<string, string>, Dictionary<string, string>> completeInfo = new Dictionary<Dictionary<string, string>, Dictionary<string, string>>();
            Dictionary<string, string> toGetNameDetailsFromDB = new Dictionary<string, string>();
            //  Dictionary<string, string> toGetNameDetailsFromDB = new Dictionary<string, string>();

            string fullName = string.Empty;
            string supervisorName = string.Empty;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select a.FirstName,a.LastName,a.SupervisorName
                                    from LTLogin as a join LTSupervisor as b
                                    on a.SupervisorName = b.SupervisorName where a.MSID = '" + MSID + "'", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    fullName = Convert.ToString(sr["FirstName"]) + " " + Convert.ToString(sr["LastName"]);
                    supervisorName = Convert.ToString(sr["SupervisorName"]);
                }
                toGetNameDetailsFromDB.Add(fullName, supervisorName);

            }
            return toGetNameDetailsFromDB;
        }


        public Dictionary<string, string> toGetNameEmailSupDetails(string MSID)
        {
            //  Dictionary<Dictionary<string, string>, Dictionary<string, string>> completeInfo = new Dictionary<Dictionary<string, string>, Dictionary<string, string>>();
            Dictionary<string, string> toGetNameDetailsFromDB = new Dictionary<string, string>();
            //  Dictionary<string, string> toGetNameDetailsFromDB = new Dictionary<string, string>();

            string fullName = string.Empty;
            string supervisorEmailID = string.Empty;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select a.FirstName,a.LastName,a.SupervisorName,b.SupervisorEmailID
                                    from LTLogin as a join LTSupervisor as b
                                    on a.SupervisorName = b.SupervisorName where a.MSID ='" + MSID + "'", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    fullName = Convert.ToString(sr["FirstName"]) + " " + Convert.ToString(sr["LastName"]);
                    supervisorEmailID = Convert.ToString(sr["SupervisorEmailID"]);
                }
                toGetNameDetailsFromDB.Add(fullName, supervisorEmailID);

            }
            return toGetNameDetailsFromDB;
        }

        public Dictionary<string, string> toGetEmailandNameDetailsFromDB(string MSID)
        {
            //  Dictionary<Dictionary<string, string>, Dictionary<string, string>> completeInfo = new Dictionary<Dictionary<string, string>, Dictionary<string, string>>();
            Dictionary<string, string> toGetEmailandNameDetailsFromDB = new Dictionary<string, string>();
            //  Dictionary<string, string> toGetNameDetailsFromDB = new Dictionary<string, string>();

            string fullName = string.Empty;
            string emailID = string.Empty;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select a.FirstName,a.LastName,b.Email_ID
                                    from LTLogin as a join LTEmailID as b
                                    on a.MSID = b.MSID where a.MSID = '" + MSID + "'", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    fullName = Convert.ToString(sr["FirstName"]) + Convert.ToString(sr["LastName"]);
                    emailID = Convert.ToString(sr["Email_ID"]);
                }
                toGetEmailandNameDetailsFromDB.Add(fullName, emailID);

            }
            return toGetEmailandNameDetailsFromDB;
        }
        public Dictionary<string, string> toGetSelfEmailIDOOO(string MSID)
        {
            //  Dictionary<Dictionary<string, string>, Dictionary<string, string>> completeInfo = new Dictionary<Dictionary<string, string>, Dictionary<string, string>>();
            Dictionary<string, string> toGetEmailandNameDetailsFromDB = new Dictionary<string, string>();
            //  Dictionary<string, string> toGetNameDetailsFromDB = new Dictionary<string, string>();

            string fullName = string.Empty;
            string emailID = string.Empty;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select a.FirstName,a.LastName,b.Email_ID
                                    from LTLogin as a join LTEmailID as b
                                    on a.MSID = b.MSID where b.MSID ='" + MSID + "'", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    fullName = Convert.ToString(sr["FirstName"]) + " " + Convert.ToString(sr["LastName"]);
                    emailID = Convert.ToString(sr["Email_ID"]);
                }
                toGetEmailandNameDetailsFromDB.Add(fullName, emailID);

            }
            return toGetEmailandNameDetailsFromDB;
        }


        public static int updateLeavesToDB(string MSID, DateTime StartDate, DateTime EndDate, string TypeOfLeaves, int? TotalLeaves, string Reason, string status, byte[] imageData, int id)
        {
            int rowAffected = 0;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                //SqlCommand cmd = new SqlCommand("insert into LTLeaveDetails values('" + MSID + "','" + StartDate + "','" + EndDate + "','" + TypeOfLeaves + "'," + TotalLeaves + ",'" + Reason + "','" + status + "',cast('"+ imageData +"' as varbinary(max)))", conn);
                //  SqlCommand cmd = new SqlCommand("update LTLeaveDetails values(@MSID,@StartDate,@EndDate,@TypeOfLeaves,@TotalLeaves,'" + Reason + "',@status,@imageData)", conn);

                //SqlCommand cmd = new SqlCommand("update LTLeaveDetails set StartDate = '"+ StartDate + "', EndDate = '@EndDate'" + EndDate  + "'"+
                //    ", TOL = '"+ TypeOfLeaves + "', Reason = '"+ Reason +"', _Status = 'Approval Pending', screenShot = '"+ imageData +"' where SequenceNo ='" + id + "'");

                SqlCommand cmd = new SqlCommand("update LTLeaveDetails set StartDate = @StartDate, EndDate = @EndDate "
                    + ", TOL = @TypeOfLeaves, Reason = '" + Reason + "', _Status = 'Approval Pending', screenShot = @imageData,modifiedDate = GetDate() where SequenceNo =" + id + "", conn);

                cmd.Parameters.AddWithValue("@MSID", MSID);
                cmd.Parameters.AddWithValue("@StartDate", StartDate);
                cmd.Parameters.AddWithValue("@EndDate", EndDate);
                cmd.Parameters.AddWithValue("@TypeOfLeaves", TypeOfLeaves);
                cmd.Parameters.AddWithValue("@TotalLeaves", TotalLeaves);
                //cmd.Parameters.AddWithValue("@Reason", Reason);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@imageData", imageData);

                rowAffected = cmd.ExecuteNonQuery();
                return rowAffected;

            }

        }

        public List<ApplyLeave> ExportToExcelForTL()
        {
            List<ApplyLeave> ExportToExcelForTLlist = new List<ApplyLeave>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select t1.Emp_ID,t1.MSID,t1.FirstName,t1.LastName,t2.StartDate,t2.EndDate,t1.Branch,
                                                  t1.Sub_Area,t1.Location,t1.Project,t2.TOL,t2.Reason,
                                                   t2.TotalDays,t1.SupervisorName,t2._Status,t1.Cust_Mana_Name,t1.Exp_Level
                                                    from LTLogin as t1 Join LTLeaveDetails as t2 on t1.MSID=t2.MSID", conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        ApplyLeave ExportToExcelfortl = new ApplyLeave();
                        ExportToExcelfortl.emp_ID = Convert.ToInt32(dr["Emp_ID"]);
                        ExportToExcelfortl.MSID = dr["MSID"].ToString();
                        ExportToExcelfortl.FirstName = dr["FirstName"].ToString();
                        ExportToExcelfortl.LastName = (dr["LastName"].ToString());
                        ExportToExcelfortl.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                        ExportToExcelfortl.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                        ExportToExcelfortl.Branch = (dr["Branch"].ToString());
                        ExportToExcelfortl.subArea = dr["Sub_Area"].ToString();
                        ExportToExcelfortl.Location = (dr["Location"].ToString());
                        ExportToExcelfortl.ProjectName = (dr["Project"].ToString());
                        ExportToExcelfortl.TypeOfLeaves = (dr["TOL"].ToString());
                        ExportToExcelfortl.Reason = (dr["Reason"].ToString());
                        ExportToExcelfortl.TotalLeaves = Convert.ToInt32(dr["TotalDays"]);
                        ExportToExcelfortl.supervisorName = (dr["SupervisorName"].ToString());
                        ExportToExcelfortl._Status = (dr["_Status"].ToString());
                        ExportToExcelfortl.custManaName = dr["Cust_Mana_Name"].ToString();
                        ExportToExcelfortl.expLevel = dr["Exp_Level"].ToString();
                        ExportToExcelForTLlist.Add(ExportToExcelfortl);
                    }

                }

            }
            return ExportToExcelForTLlist;
        }

        public static bool toCheckLeave_OOO(string MSID, string startDate)
        {
            bool count = false;

            List<ApplyLeave> ExportToExcelForTLlist = new List<ApplyLeave>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from LTLeaveDetails where MSID ='" + MSID + "' and StartDate ='" + startDate + "' and (TOL='Sick/UnPlanned' or Reason='Applied By Admin')", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                count = dr.HasRows;
                return count;
            }
        }


        public List<ApplyLeave> SearchMSIDByName(string Name)
        {
            List<ApplyLeave> MSIDlist = new List<ApplyLeave>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select MSID,FirstName,LastName from LTLogin where FirstName like '%" + Name + "%'", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        ApplyLeave appLeaveObj = new ApplyLeave();
                        appLeaveObj.MSID = dr["MSID"].ToString();
                        appLeaveObj.FirstName = dr["FirstName"].ToString();
                        appLeaveObj.LastName = dr["LastName"].ToString();
                        MSIDlist.Add(appLeaveObj);
                    }
                }
                return MSIDlist;
            }
        }

        public List<ApplyLeave> ExportToExcelForUser()
        {
            List<ApplyLeave> ExportToExcelForUserList = new List<ApplyLeave>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select MSID,FirstName,LastName from LTLogin", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        ApplyLeave ExportToExcelforuser = new ApplyLeave();
                        ExportToExcelforuser.MSID = dr["MSID"].ToString();
                        ExportToExcelforuser.FirstName = dr["FirstName"].ToString();
                        ExportToExcelforuser.LastName = (dr["LastName"].ToString());
                        ExportToExcelForUserList.Add(ExportToExcelforuser);
                    }
                }
            }
            return ExportToExcelForUserList;
        }

        public List<ApplyLeave> viewUserInfo(string MSID)
        {
            List<ApplyLeave> userInfo = new List<ApplyLeave>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select a.Emp_ID,b.Email_ID,a.Sub_Area,a.SupervisorName,a.Cust_Mana_Name,a.Project,a.Location 
                                                    from LTLogin as a inner join LTEmailID as b
                                                    on a.MSID=b.MSID where a.MSID='" + MSID + "'", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        ApplyLeave userInfoObj = new ApplyLeave();
                        userInfoObj.emp_ID = Convert.ToInt32(dr["Emp_ID"].ToString());
                        userInfoObj.add = dr["Email_ID"].ToString();
                        userInfoObj.subArea = dr["Sub_Area"].ToString();
                        userInfoObj.supervisorName = dr["SupervisorName"].ToString();
                        userInfoObj.custManaName = dr["Cust_Mana_Name"].ToString();
                        userInfoObj.ProjectName = dr["Project"].ToString();
                        userInfoObj.Location = dr["Location"].ToString();
                        userInfo.Add(userInfoObj);
                    }
                }
            }
            return userInfo;
        }

        //public string findSupervisorByMSID(string msid)
        //{
        //    string supervisorName = string.Empty;
        //    string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //    using (SqlConnection conn = new SqlConnection(con))
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand(@"", conn);
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        if (dr.FieldCount != 0)
        //        {
        //            while (dr.Read())
        //            {
                       
        //            }
        //        }
        //    }
        //    return supervisorName;
        //}

    }
}



