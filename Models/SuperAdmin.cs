using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace sampleMVC.Models
{
    public class SuperAdmin
    {
        #region

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDateBySuperAdmin { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDateBySuperAdmin { get; set; }

        public string selectMonth { get; set; }
        public string FirstName { get; set; }
        public string Reason { get; set; }
        public string _Status { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string MSID { get; set; }
        public string Branch { get; set; }
        public string ProjectName { get; set; }
        public string typeOfLeave { get; set; }
        public string IsActive { get; set; }
        public int unplanned { get; set; }
        public int planned { get; set; }
        public int sick { get; set; }
        public DateTime dt_startdate = DateTime.Now;
        public DateTime dt_enddate = DateTime.Now;
        public int totalLeaves { get; set; }
        public byte[] imageSrc11 { get; set; }
        public int sNo { get; set; }
        public string fetchYear { get; set; }

        public string expLevel { get; set; }
        public string subArea { get; set; }
        public int emp_ID { get; set; }

        int getMonth = 0;
        string getYear = string.Empty;
        string monthName = string.Empty;

        public string custManaName { get; set; }
        [Display(Name = "Enter Name to Search")]
        public string searchTextBox { get; set; }

        //[Display(ByNameByLocByTOL = "Enter Name or Loc or TOL")]
        [Display(Name = "Enter Name or Loc or TOL")]
        public string ByNameByLocByTOL { get; set; }


        public List<SuperAdmin> PAGwiseViewForSuperAdmin { get; set; }
        public List<SuperAdmin> PAGWiseViewForSuperAdminByPAGNameList { get; set; }
        public List<SuperAdmin> SearchByNameForSuperAdminList { get; set; }
        public List<SuperAdmin> PAGWiseViewForSuperAdminByProjNameList { get; set; }
        public List<SuperAdmin> PAGWiseViewForSuperAdminByProjNameByTOLList { get; set; }
        public List<SuperAdmin> PAGWiseViewForSuperAdminByPAGNameByTOLList { get; set; }
        public List<SuperAdmin> ExportToExcelForSuperAdminList { get; set; }

        public List<SuperAdmin> ExportToExcelByPAGNameForSuperAdminList { get; set; }

        public List<SuperAdmin> ExportToExcelForSuperAdminListByDate { get; set; }

        public IEnumerable<SelectListItem> selectMonthIM { get; set; }
        #endregion

        public List<SuperAdmin> GetPagViewBySuperAdmin()
        {
            List<SuperAdmin> GetPagViewBySuperAdminlist = new List<SuperAdmin>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT  t2.Branch,SUM(case when tol='Planned' then 1 else 0 end) as planned,
                                                  SUM(case when tol='Unplanned' then 1 else 0 end) as unplanned,
                                                  SUM(case when tol='sick' then 1 else 0 end) as sick
                                                  FROM LTLeaveDetails as t1 join LTLogin as t2 on  t2.MSID = t1.MSID 
                                                  where t1._Status='Approved' Group by t2.Branch", conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        SuperAdmin GetPagViewBySuperAdmin = new SuperAdmin();
                        GetPagViewBySuperAdmin.Branch = dr["Branch"].ToString();
                        GetPagViewBySuperAdmin.unplanned = Convert.ToInt32(dr["planned"].ToString());
                        GetPagViewBySuperAdmin.planned = Convert.ToInt32(dr["unplanned"].ToString());
                        GetPagViewBySuperAdmin.sick = Convert.ToInt32(dr["sick"].ToString());
                        GetPagViewBySuperAdmin.totalLeaves = GetPagViewBySuperAdmin.unplanned + GetPagViewBySuperAdmin.planned + GetPagViewBySuperAdmin.sick;
                        GetPagViewBySuperAdminlist.Add(GetPagViewBySuperAdmin);
                    }

                }
                return GetPagViewBySuperAdminlist;
            }

        }

        public List<SuperAdmin> PAGWiseViewForSuperAdminBYPAGName(string PAGName)
        {
            List<SuperAdmin> SuperAdminByPAGNamelist = new List<SuperAdmin>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@" SELECT  t2.Project,SUM(case when tol='Planned' then 1 else 0 end) as planned,
                                                  SUM(case when tol='Unplanned' then 1 else 0 end) as unplanned,
                                                  SUM(case when tol='sick' then 1 else 0 end) as sick
                                                  FROM LTLeaveDetails as t1 join LTLogin as t2 on  t2.MSID= t1.MSID 
                                                  where t1._Status='Approved' and t2.Branch='" + PAGName + "'GROUP BY t2.Project", conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        SuperAdmin SuperAdminByPAG = new SuperAdmin();
                        SuperAdminByPAG.ProjectName = dr["Project"].ToString();
                        SuperAdminByPAG.planned = Convert.ToInt32(dr["planned"].ToString());
                        SuperAdminByPAG.unplanned = Convert.ToInt32(dr["unplanned"].ToString());
                        SuperAdminByPAG.sick = Convert.ToInt32(dr["sick"].ToString());
                        SuperAdminByPAG.totalLeaves = SuperAdminByPAG.planned + SuperAdminByPAG.unplanned + SuperAdminByPAG.sick;
                        SuperAdminByPAG.Branch = PAGName;
                        SuperAdminByPAGNamelist.Add(SuperAdminByPAG);
                    }

                }
                return SuperAdminByPAGNamelist;
            }

        }


        public List<SuperAdmin> PAGWiseViewForSuperAdminBYPAGNameDetailed(string PAGName)
        {
            List<SuperAdmin> SuperAdminByPAGNamelist = new List<SuperAdmin>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select t1.FirstName,t1.LastName,t1.Project,t1.Location,t2.StartDate,t2.EndDate,t2.TOL,
                                                t2.Reason,t2._Status from LTLogin as t1 join LTLeaveDetails as t2 on t1.MSID=t2.MSID 
                                                where t2._Status='Approved'and t1.Branch='" + PAGName + "'", conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        SuperAdmin SuperAdminByPAG = new SuperAdmin();
                        SuperAdminByPAG.FirstName = dr["FirstName"].ToString();
                        SuperAdminByPAG.LastName = (dr["LastName"].ToString());
                        SuperAdminByPAG.Location = (dr["Location"].ToString());
                        // ViewApprovedLeavesByAdmin.Branch = (dr["Branch"].ToString());
                        SuperAdminByPAG.StartDate = (dr["StartDate"].ToString());
                        SuperAdminByPAG.EndDate = (dr["EndDate"].ToString());
                        SuperAdminByPAG.typeOfLeave = (dr["TOL"].ToString());
                        SuperAdminByPAG.Reason = (dr["Reason"].ToString());
                        SuperAdminByPAG._Status = (dr["_Status"].ToString());
                        SuperAdminByPAG.Branch = PAGName;
                        SuperAdminByPAGNamelist.Add(SuperAdminByPAG);
                    }

                }
                return SuperAdminByPAGNamelist;
            }

        }

        public List<SuperAdmin> PAGWiseViewForSuperAdminBYPAGNameByTOL(string PAGName, string TOL)
        {
            List<SuperAdmin> PAGWiseViewForSuperAdminBYPAGNameByTOLlist = new List<SuperAdmin>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select t1.FirstName,t1.LastName,t1.Project,t1.Location,t1.Branch,t2.StartDate,t2.EndDate,t2.TOL,
                                                 t2.Reason,t2._Status from LTLogin as t1 join LTLeaveDetails as t2 on t1.MSID=t2.MSID 
                                                 where t2._Status='Approved' and t2.TOL='" + TOL + "' and t1.Branch='" + PAGName + "'", conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        SuperAdmin ViewApprovedLeavesByAdminlist = new SuperAdmin();
                        ViewApprovedLeavesByAdminlist.FirstName = dr["FirstName"].ToString();
                        ViewApprovedLeavesByAdminlist.LastName = (dr["LastName"].ToString());
                        ViewApprovedLeavesByAdminlist.Location = (dr["Location"].ToString());
                        ViewApprovedLeavesByAdminlist.ProjectName = (dr["Project"].ToString());
                        ViewApprovedLeavesByAdminlist.StartDate = (dr["StartDate"].ToString());
                        ViewApprovedLeavesByAdminlist.EndDate = (dr["EndDate"].ToString());
                        ViewApprovedLeavesByAdminlist.typeOfLeave = (dr["TOL"].ToString());
                        ViewApprovedLeavesByAdminlist.Reason = (dr["Reason"].ToString());
                        ViewApprovedLeavesByAdminlist._Status = (dr["_Status"].ToString());
                        ViewApprovedLeavesByAdminlist.Branch = PAGName;
                        PAGWiseViewForSuperAdminBYPAGNameByTOLlist.Add(ViewApprovedLeavesByAdminlist);
                    }

                }
                return PAGWiseViewForSuperAdminBYPAGNameByTOLlist;
            }

        }

        public List<SuperAdmin> PAGWiseViewForSuperAdminByProjName(string PAGName, string ProjName)
        {
            List<SuperAdmin> PAGWiseViewForSuperAdminByProjNamelist = new List<SuperAdmin>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select t2.SequenceNo,t1.FirstName,t1.LastName,t1.Location,t1.Branch,t2.StartDate,t2.EndDate,t2.TOL,
                                                t2.Reason,t2.screenShot ,t2._Status from LTLogin as t1 join LTLeaveDetails as t2 on t1.MSID=t2.MSID 
                                                where t2._Status='Approved'and t1.Branch='" + PAGName + "'and t1.Project='" + ProjName + "'", conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        SuperAdmin AdminByProj = new SuperAdmin();
                        AdminByProj.FirstName = dr["FirstName"].ToString();
                        AdminByProj.LastName = (dr["LastName"].ToString());
                        AdminByProj.Location = (dr["Location"].ToString());
                        AdminByProj.Branch = (dr["Branch"].ToString());
                        AdminByProj.StartDate = (dr["StartDate"].ToString());
                        AdminByProj.EndDate = (dr["EndDate"].ToString());
                        AdminByProj.typeOfLeave = (dr["TOL"].ToString());
                        AdminByProj.Reason = (dr["Reason"].ToString());
                        AdminByProj._Status = (dr["_Status"].ToString());
                        AdminByProj.ProjectName = ProjName;
                        AdminByProj.imageSrc11 = dr["screenShot"] as byte[] ?? null;
                        AdminByProj.sNo = Convert.ToInt32(dr["SequenceNo"]);
                        PAGWiseViewForSuperAdminByProjNamelist.Add(AdminByProj);
                    }

                }
                return PAGWiseViewForSuperAdminByProjNamelist;
            }

        }

        public List<SuperAdmin> PAGWiseViewForSuperAdminByProjNameByTOL(string ProjName, string TOL)
        {
            List<SuperAdmin> PAGWiseViewForSuperAdminByProjNameByTOLlist = new List<SuperAdmin>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select t2.SequenceNo,t1.FirstName,t1.LastName,t1.Location,t1.Branch,t2.StartDate,t2.EndDate,t2.TOL,
                                                 t2.Reason,t2.screenShot,t2._Status from LTLogin as t1 join LTLeaveDetails as t2 on t1.MSID=t2.MSID 
                                                 where t2._Status='Approved' and t2.TOL='" + TOL + "' and t1.Project='" + ProjName + "'", conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        SuperAdmin AdminByProjByTOL = new SuperAdmin();
                        AdminByProjByTOL.FirstName = dr["FirstName"].ToString();
                        AdminByProjByTOL.LastName = (dr["LastName"].ToString());
                        AdminByProjByTOL.Location = (dr["Location"].ToString());
                        AdminByProjByTOL.Branch = (dr["Branch"].ToString());
                        AdminByProjByTOL.StartDate = (dr["StartDate"].ToString());
                        AdminByProjByTOL.EndDate = (dr["EndDate"].ToString());
                        AdminByProjByTOL.typeOfLeave = (dr["TOL"].ToString());
                        AdminByProjByTOL.Reason = (dr["Reason"].ToString());
                        AdminByProjByTOL._Status = (dr["_Status"].ToString());
                        AdminByProjByTOL.ProjectName = ProjName;
                        AdminByProjByTOL.imageSrc11 = dr["screenShot"] as byte[] ?? null;
                        AdminByProjByTOL.sNo = Convert.ToInt32(dr["SequenceNo"]);
                        PAGWiseViewForSuperAdminByProjNameByTOLlist.Add(AdminByProjByTOL);
                    }

                }
                return PAGWiseViewForSuperAdminByProjNameByTOLlist;
            }

        }

        public List<SuperAdmin> ExportToExcelForSuperAdmin()
        {
            List<SuperAdmin> ExportToExcelForSuperAdminlist = new List<SuperAdmin>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select t1.Emp_ID,t1.MSID,t1.FirstName,t1.LastName,t1.Branch,t1.Location,t1.Project,
                                                   t2.StartDate,t2.EndDate,t2.TOL,t2.Reason,
                                                   t2.TotalDays,t2._Status,t1.Cust_Mana_Name,t1.Exp_Level,t1.Sub_Area
                                                    from LTLogin as t1 Join LTLeaveDetails as t2 on t1.MSID=t2.MSID", conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        SuperAdmin ExportToExcelforSuperAdmin = new SuperAdmin();
                        ExportToExcelforSuperAdmin.emp_ID = Convert.ToInt32(dr["Emp_ID"]);
                        ExportToExcelforSuperAdmin.MSID = dr["MSID"].ToString(); 
                        ExportToExcelforSuperAdmin.FirstName = dr["FirstName"].ToString();
                        ExportToExcelforSuperAdmin.LastName = (dr["LastName"].ToString());
                        ExportToExcelforSuperAdmin.Location = (dr["Location"].ToString());
                        ExportToExcelforSuperAdmin.Branch = (dr["Branch"].ToString());
                        ExportToExcelforSuperAdmin.ProjectName = (dr["Project"].ToString());
                        dt_startdate = Convert.ToDateTime((dr["StartDate"]).ToString());
                        dt_enddate = Convert.ToDateTime((dr["EndDate"]).ToString());
                        getMonth = dt_startdate.Month;
                        monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(getMonth);
                        getYear = dt_startdate.Year.ToString();
                        ExportToExcelforSuperAdmin.fetchYear = monthName + getYear;
                        ExportToExcelforSuperAdmin.StartDate = dt_startdate.ToShortDateString().ToString();
                        ExportToExcelforSuperAdmin.EndDate = dt_enddate.ToShortDateString().ToString();
                        ExportToExcelforSuperAdmin.typeOfLeave = (dr["TOL"].ToString());
                        ExportToExcelforSuperAdmin.Reason = (dr["Reason"].ToString());
                        ExportToExcelforSuperAdmin.totalLeaves = Convert.ToInt32(dr["TotalDays"]);
                        ExportToExcelforSuperAdmin._Status = (dr["_Status"].ToString());
                        ExportToExcelforSuperAdmin.custManaName = dr["Cust_Mana_Name"].ToString() ;
                        ExportToExcelforSuperAdmin.expLevel = dr["Cust_Mana_Name"].ToString();
                        ExportToExcelforSuperAdmin.expLevel = dr["Exp_Level"].ToString();
                        ExportToExcelforSuperAdmin.subArea = dr["Sub_Area"].ToString();
                        ExportToExcelForSuperAdminlist.Add(ExportToExcelforSuperAdmin);
                    }

                }
                return ExportToExcelForSuperAdminlist;
            }

        }

        public List<SuperAdmin> ExportToExcelForSuperAdminByPAGName(string PAGName)
        {
            List<SuperAdmin> ExportToExcelForSuperAdminlist = new List<SuperAdmin>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select t1.Emp_ID,t1.MSID,t1.FirstName,t1.LastName,t1.Branch,t1.Location,t1.Project,
                                                   t2.StartDate,t2.EndDate,t2.TOL,t2.Reason,
                                                   t2.TotalDays,t2._Status,t1.Cust_Mana_Name,t1.Exp_Level,t1.Sub_Area
                                                    from LTLogin as t1 Join LTLeaveDetails as t2 on t1.MSID=t2.MSID where Branch = '" + PAGName + "'", conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        SuperAdmin ExportToExcelforSuperAdmin = new SuperAdmin();
                        ExportToExcelforSuperAdmin.emp_ID = Convert.ToInt32(dr["Emp_ID"]);
                        ExportToExcelforSuperAdmin.MSID = dr["MSID"].ToString();
                        ExportToExcelforSuperAdmin.FirstName = dr["FirstName"].ToString();
                        ExportToExcelforSuperAdmin.LastName = (dr["LastName"].ToString());
                        ExportToExcelforSuperAdmin.ProjectName = (dr["Project"].ToString());
                        ExportToExcelforSuperAdmin.Location = (dr["Location"].ToString());
                        ExportToExcelforSuperAdmin.Branch = (dr["Branch"].ToString());
                        dt_startdate = Convert.ToDateTime((dr["StartDate"]).ToString());
                        dt_enddate = Convert.ToDateTime((dr["EndDate"]).ToString());
                        getMonth = dt_startdate.Month;
                        monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(getMonth);
                        getYear = dt_startdate.Year.ToString();
                        ExportToExcelforSuperAdmin.fetchYear = monthName + getYear;
                        ExportToExcelforSuperAdmin.StartDate = dt_startdate.ToShortDateString().ToString();
                        ExportToExcelforSuperAdmin.EndDate = dt_enddate.ToShortDateString().ToString();
                        ExportToExcelforSuperAdmin.typeOfLeave = (dr["TOL"].ToString());
                        ExportToExcelforSuperAdmin.Reason = (dr["Reason"].ToString());
                        ExportToExcelforSuperAdmin._Status = (dr["_Status"].ToString());
                        ExportToExcelforSuperAdmin.custManaName = dr["Cust_Mana_Name"].ToString();
                        ExportToExcelforSuperAdmin.expLevel = dr["Exp_Level"].ToString();
                        ExportToExcelforSuperAdmin.subArea = dr["Sub_Area"].ToString();
                        ExportToExcelForSuperAdminlist.Add(ExportToExcelforSuperAdmin);
                    }

                }
                return ExportToExcelForSuperAdminlist;
            }

        }

        public List<SuperAdmin> ExportToExcelForSuperAdminByPAGNameByProj(string PAGName,string ProjName,string TOL)
        {
            List<SuperAdmin> ExportToExcelForSuperAdminlist = new List<SuperAdmin>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                if(ProjName.Equals(""))
                { 
                 cmd = new SqlCommand(@"select t1.Emp_ID,t1.MSID,t1.FirstName,t1.LastName,t1.Branch,t1.Location,t1.Project,
                                                   t2.StartDate,t2.EndDate,t2.TOL,t2.Reason,
                                                   t2.TotalDays,t2._Status,t1.Cust_Mana_Name,t1.Exp_Level,t1.Sub_Area
                                                    from LTLogin as t1 Join LTLeaveDetails as t2 on t1.MSID=t2.MSID
                                                    where t2._Status='Approved' and Branch = '" + PAGName + "'  and t2.TOL='"+ TOL +"'", conn);
                }

                else
                { 
                 cmd = new SqlCommand(@"select t1.Emp_ID,t1.MSID,t1.FirstName,t1.LastName,t1.Branch,t1.Location,t1.Project,
                                                   t2.StartDate,t2.EndDate,t2.TOL,t2.Reason,
                                                   t2.TotalDays,t2._Status,t1.Cust_Mana_Name,t1.Exp_Level,t1.Sub_Area
                                                    from LTLogin as t1 Join LTLeaveDetails as t2 on t1.MSID=t2.MSID
                                                    where  t2._Status='Approved' and Branch = '" + PAGName + "'" 
                                                    + " and t1.Project = '" + ProjName + "'"
                                                    + " and t2.TOL = '"+ TOL +"'", conn);
                }
                SqlDataReader dr = cmd.ExecuteReader();
               
                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        SuperAdmin ExportToExcelforSuperAdmin = new SuperAdmin();
                        ExportToExcelforSuperAdmin.emp_ID = Convert.ToInt32(dr["Emp_ID"]);
                        ExportToExcelforSuperAdmin.MSID = dr["MSID"].ToString();
                        ExportToExcelforSuperAdmin.FirstName = dr["FirstName"].ToString();
                        ExportToExcelforSuperAdmin.LastName = (dr["LastName"].ToString());
                        ExportToExcelforSuperAdmin.ProjectName = (dr["Project"].ToString());
                        ExportToExcelforSuperAdmin.Location = (dr["Location"].ToString());
                        ExportToExcelforSuperAdmin.Branch = (dr["Branch"].ToString());
                        dt_startdate = Convert.ToDateTime((dr["StartDate"]).ToString());
                        dt_enddate = Convert.ToDateTime((dr["EndDate"]).ToString());
                        ExportToExcelforSuperAdmin.StartDate = dt_startdate.ToShortDateString().ToString();
                        ExportToExcelforSuperAdmin.EndDate = dt_enddate.ToShortDateString().ToString();
                        getMonth = dt_startdate.Month;
                        monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(getMonth);
                        getYear = dt_startdate.Year.ToString();
                        ExportToExcelforSuperAdmin.fetchYear = monthName + getYear;
                        ExportToExcelforSuperAdmin.totalLeaves = Convert.ToInt32(dr["TotalDays"]);
                        ExportToExcelforSuperAdmin.typeOfLeave = (dr["TOL"].ToString());
                        ExportToExcelforSuperAdmin.Reason = (dr["Reason"].ToString());
                        ExportToExcelforSuperAdmin._Status = (dr["_Status"].ToString());
                        ExportToExcelforSuperAdmin.custManaName = dr["Cust_Mana_Name"].ToString();
                        ExportToExcelforSuperAdmin.expLevel = dr["Exp_Level"].ToString();
                        ExportToExcelforSuperAdmin.subArea = dr["Sub_Area"].ToString();
                        ExportToExcelForSuperAdminlist.Add(ExportToExcelforSuperAdmin);
                    }

                }
                return ExportToExcelForSuperAdminlist;
            }

        }




        public List<SuperAdmin> ExportToExcelForSuperAdminByDate(string ProjName, string sD, string eD)
        {
            List<SuperAdmin> ExportToExcelForSuperAdminlist = new List<SuperAdmin>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select t1.Emp_ID,t1.MSID,t1.FirstName,t1.LastName,t1.Branch,t1.Location,t1.Project,
                                                   t2.StartDate,t2.EndDate,t2.TOL,t2.Reason,
                                                   t2.TotalDays,t2._Status,t1.Cust_Mana_Name,t1.Exp_Level,t1.Sub_Area
                                                    from LTLogin as t1 Join LTLeaveDetails as t2 on t1.MSID=t2.MSID
                                                    where Project = '" + ProjName + "' and (StartDate between '" + sD + "' and '" + eD + "'" +
                                                    " and EndDate between '" + sD + "' and '" + eD + "')", conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        SuperAdmin ExportToExcelforSuperAdmin = new SuperAdmin();
                        ExportToExcelforSuperAdmin.emp_ID = Convert.ToInt32(dr["Emp_ID"]);
                        ExportToExcelforSuperAdmin.MSID = dr["MSID"].ToString();
                        ExportToExcelforSuperAdmin.FirstName = dr["FirstName"].ToString();
                        ExportToExcelforSuperAdmin.LastName = (dr["LastName"].ToString());
                        ExportToExcelforSuperAdmin.ProjectName = (dr["Project"].ToString());
                        ExportToExcelforSuperAdmin.Location = (dr["Location"].ToString());
                        ExportToExcelforSuperAdmin.Branch = (dr["Branch"].ToString()); ;
                        dt_startdate = Convert.ToDateTime((dr["StartDate"]).ToString());
                        dt_enddate = Convert.ToDateTime((dr["EndDate"]).ToString());
                        ExportToExcelforSuperAdmin.StartDate = dt_startdate.ToShortDateString().ToString();
                        ExportToExcelforSuperAdmin.EndDate = dt_enddate.ToShortDateString().ToString();
                        getMonth = dt_startdate.Month;
                        monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(getMonth);
                        getYear = dt_startdate.Year.ToString();
                        ExportToExcelforSuperAdmin.fetchYear = monthName + getYear;
                        ExportToExcelforSuperAdmin.typeOfLeave = (dr["TOL"].ToString());
                        ExportToExcelforSuperAdmin.Reason = (dr["Reason"].ToString());
                        ExportToExcelforSuperAdmin._Status = (dr["_Status"].ToString());
                        ExportToExcelforSuperAdmin.custManaName = dr["Cust_Mana_Name"].ToString();
                        ExportToExcelforSuperAdmin.expLevel = dr["Exp_Level"].ToString();
                        ExportToExcelforSuperAdmin.subArea = dr["Sub_Area"].ToString();
                        ExportToExcelForSuperAdminlist.Add(ExportToExcelforSuperAdmin);
                    }

                }
                return ExportToExcelForSuperAdminlist;
            }

        }

        public List<SuperAdmin> PAGWiseViewForSuperAdminByProjNameRefinedByDate(SuperAdmin superAdmin, string sD, string eD)
        {
            List<SuperAdmin> PAGWiseViewForSuperAdminByProjNamelist = new List<SuperAdmin>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select t1.FirstName,t1.LastName,t1.Location,t1.Branch,t2.StartDate,t2.EndDate,t2.TOL,
                                                t2.Reason,t2._Status from LTLogin as t1 join LTLeaveDetails as t2 on t1.MSID=t2.MSID 
                                                where t2._Status='Approved'and t1.Branch='" + superAdmin.Branch +
                                                "'and t1.Project='" + superAdmin.ProjectName + "'" +
                                                " and StartDate between '" + sD + "' and '" + eD + "'" +
                                                " and EndDate between '" + sD + "' and '" + eD + "'", conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        SuperAdmin AdminByProj = new SuperAdmin();
                        AdminByProj.FirstName = dr["FirstName"].ToString();
                        AdminByProj.LastName = (dr["LastName"].ToString());
                        AdminByProj.Location = (dr["Location"].ToString());
                        AdminByProj.Branch = (dr["Branch"].ToString());
                        AdminByProj.StartDate = (dr["StartDate"].ToString());
                        AdminByProj.EndDate = (dr["EndDate"].ToString());
                        AdminByProj.typeOfLeave = (dr["TOL"].ToString());
                        AdminByProj.Reason = (dr["Reason"].ToString());
                        AdminByProj._Status = (dr["_Status"].ToString());
                        AdminByProj.ProjectName = superAdmin.ProjectName;
                        PAGWiseViewForSuperAdminByProjNamelist.Add(AdminByProj);
                    }

                }
                return PAGWiseViewForSuperAdminByProjNamelist;
            }

        }
        
             public List<SuperAdmin> PAGWiseViewForSuperAdminByProjNameRefinedByMonth(SuperAdmin superAdmin, string sD, string eD,string fetchingYear)
        {
            List<SuperAdmin> PAGWiseViewForSuperAdminByProjNamelist = new List<SuperAdmin>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select t1.FirstName,t1.LastName,t1.Location,t1.Branch,t2.StartDate,t2.EndDate,t2.TOL,
                                                t2.Reason,t2._Status from LTLogin as t1 join LTLeaveDetails as t2 on t1.MSID=t2.MSID 
                                                where t2._Status='Approved'and t1.Branch='" + superAdmin.Branch +
                                                "'and t1.Project='" + superAdmin.ProjectName + "'" +
                                                " and StartDate between '" + sD + "' and '" + eD + "'" +
                                                " and EndDate between '" + sD + "' and '" + eD + "'", conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        SuperAdmin AdminByProj = new SuperAdmin();
                        AdminByProj.FirstName = dr["FirstName"].ToString();
                        AdminByProj.LastName = (dr["LastName"].ToString());
                        AdminByProj.Location = (dr["Location"].ToString());
                        AdminByProj.Branch = (dr["Branch"].ToString());
                        AdminByProj.StartDate = (dr["StartDate"].ToString());
                        AdminByProj.EndDate = (dr["EndDate"].ToString());
                        AdminByProj.typeOfLeave = (dr["TOL"].ToString());
                        AdminByProj.Reason = (dr["Reason"].ToString());
                        AdminByProj._Status = (dr["_Status"].ToString());
                        AdminByProj.ProjectName = superAdmin.ProjectName;
                        getMonth = dt_startdate.Month;
                        monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(getMonth);
                        getYear = dt_startdate.Year.ToString();
                        AdminByProj.fetchYear = monthName + getYear;
                        PAGWiseViewForSuperAdminByProjNamelist.Add(AdminByProj);
                    }

                }
                return PAGWiseViewForSuperAdminByProjNamelist;
            }

        }

        public IEnumerable<string> getSelectMonth()
        {
            return new List<string>
            {
                "Jan",
                "Feb",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "Sep",
                "Oct",
                "Nov",
                "Dec"
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

        public List<SuperAdmin> SearchByNameForSuperAdminDetails(string Name)
        {

            List<SuperAdmin> SearchByNameForSuperAdminDetailslist = new List<SuperAdmin>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select t2.SequenceNo,t1.FirstName,t1.LastName,t1.Project,t1.Branch,t1.Location,
                                                   t2.StartDate,t2.EndDate,t2.TOL,t2.Reason,t2.screenShot,t2._Status
                                                    from LTLogin as t1 Join LTLeaveDetails as t2 on t1.MSID=t2.MSID where t1.FirstName like '%" + Name + "%'", conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        SuperAdmin SearchByNameForSuperAdmin = new SuperAdmin();
                        SearchByNameForSuperAdmin.FirstName = dr["FirstName"].ToString();
                        SearchByNameForSuperAdmin.LastName = (dr["LastName"].ToString());
                        SearchByNameForSuperAdmin.ProjectName = (dr["Project"].ToString());
                        SearchByNameForSuperAdmin.Location = (dr["Location"].ToString());
                        SearchByNameForSuperAdmin.Branch = (dr["Branch"].ToString());
                        SearchByNameForSuperAdmin.StartDate = (Convert.ToDateTime(dr["StartDate"])).ToString();
                        SearchByNameForSuperAdmin.EndDate = (Convert.ToDateTime(dr["EndDate"])).ToString();
                        SearchByNameForSuperAdmin.StartDate = dt_startdate.ToShortDateString().ToString();
                        SearchByNameForSuperAdmin.EndDate = dt_enddate.ToShortDateString().ToString();
                        SearchByNameForSuperAdmin.typeOfLeave = (dr["TOL"].ToString());
                        SearchByNameForSuperAdmin.Reason = (dr["Reason"].ToString());
                        SearchByNameForSuperAdmin._Status = (dr["_Status"].ToString());
                        SearchByNameForSuperAdmin.imageSrc11 = dr["screenShot"] as byte[] ?? null;
                        SearchByNameForSuperAdmin.sNo = Convert.ToInt32(dr["SequenceNo"]);
                        SearchByNameForSuperAdminDetailslist.Add(SearchByNameForSuperAdmin);
                    }

                }
                return SearchByNameForSuperAdminDetailslist;
            }


        }



        public List<SuperAdmin> SearchByNMByLocByTOLForSuperAdmin(string dummyVariable, string PAGName, string ProjName)
        {
            string Query = string.Empty;
            string QueryBase = string.Empty;
            string QueryTemp = string.Empty;
            List<SuperAdmin> SearchByNMByLocByTOLForSuperAdminlist = new List<SuperAdmin>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            QueryBase = @"select t2.SequenceNo,t1.FirstName,t1.LastName,t1.Project,t1.Branch,t1.Location,
                            t2.StartDate,t2.EndDate,t2.TOL,t2.Reason,t2.screenShot,t2._Status
                                from LTLogin as t1 Join LTLeaveDetails as t2 on t1.MSID = t2.MSID where
                            t1.Branch = '" + PAGName + "' and t1.Project = '" + ProjName + "' and  ";
            if (dummyVariable.Equals("Planned") || dummyVariable.Equals("Unplanned") || dummyVariable.Equals("Sick"))
            {
                QueryTemp = "t2.TOL = '" + dummyVariable + "'";
            }
            else if (dummyVariable.Equals("Pune") || dummyVariable.Equals("Bangalore") || dummyVariable.Equals("Chennai"))
            {
                QueryTemp = "t1.Location = '" + dummyVariable + "'";
            }
            else 
            {
                QueryTemp = "t1.FirstName = '" + dummyVariable + "'";
            }
            Query = QueryBase + QueryTemp;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(Query, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        SuperAdmin SearchByNMByLocByTOLForSuperAdmin = new SuperAdmin();
                        SearchByNMByLocByTOLForSuperAdmin.FirstName = dr["FirstName"].ToString();
                        SearchByNMByLocByTOLForSuperAdmin.LastName = (dr["LastName"].ToString());
                        SearchByNMByLocByTOLForSuperAdmin.ProjectName = (dr["Project"].ToString());
                        SearchByNMByLocByTOLForSuperAdmin.Location = (dr["Location"].ToString());
                        SearchByNMByLocByTOLForSuperAdmin.Branch = (dr["Branch"].ToString());
                        SearchByNMByLocByTOLForSuperAdmin.StartDate = (Convert.ToDateTime(dr["StartDate"])).ToString();
                        SearchByNMByLocByTOLForSuperAdmin.EndDate = (Convert.ToDateTime(dr["EndDate"])).ToString();
                        SearchByNMByLocByTOLForSuperAdmin.StartDate = dt_startdate.ToShortDateString().ToString();
                        SearchByNMByLocByTOLForSuperAdmin.EndDate = dt_enddate.ToShortDateString().ToString();
                        SearchByNMByLocByTOLForSuperAdmin.typeOfLeave = (dr["TOL"].ToString());
                        SearchByNMByLocByTOLForSuperAdmin.Reason = (dr["Reason"].ToString());
                        SearchByNMByLocByTOLForSuperAdmin._Status = (dr["_Status"].ToString());
                        SearchByNMByLocByTOLForSuperAdmin.imageSrc11 = dr["screenShot"] as byte[] ?? null;
                        SearchByNMByLocByTOLForSuperAdmin.sNo = Convert.ToInt32(dr["SequenceNo"]);
                        SearchByNMByLocByTOLForSuperAdminlist.Add(SearchByNMByLocByTOLForSuperAdmin);
                    }

                }
                return SearchByNMByLocByTOLForSuperAdminlist;
            }


        }


        public  SuperAdmin getLeaveInfo(int id)
        {
            SuperAdmin obj = new SuperAdmin();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from LTLeaveDetails where SequenceNo=" + id + "", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {

                    obj.imageSrc11 = sr["screenShot"] as byte[] ?? null;
                  
                }

            }
            return obj;
        }


    }
}