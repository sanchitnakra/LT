using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace sampleMVC.Models
{
    public class AdminRole
    {
        #region
        public string FirstName { get; set; }
        public string Reason { get; set; }
        public string _Status { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int MSID { get; set; }
        public string Branch { get; set; }
        public string ProjectName { get; set; }
        public string typeOfLeave { get; set; }
        public string IsActive { get; set; }




        public int unplanned { get; set; }
        public int planned { get; set; }
        public int sick { get; set; }

        public List<AdminRole> PAGwiseViewForSuperAdmin { get; set; }
        public List<AdminRole> ViewApprovedLeavesByAdminlist { get; set; }
        public List<AdminRole> ViewApprovedLeavesByAdminByProjNameList { get; set; }
        public List<AdminRole> AppliedLeavesList { get; set; }
        #endregion

        public List<AdprojectDetails> FetchProjectDetails()
        {

            // AdprojectDetails ProjName = new AdprojectDetails();
            List<AdprojectDetails> listofproject = new List<AdprojectDetails>();

            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select ProjectName from LTProject", conn);
                //   flag = Convert.ToBoolean(cmd.ExecuteScalar());
                SqlDataReader dr = cmd.ExecuteReader();



                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        AdprojectDetails ProjName1 = new AdprojectDetails();
                        ProjName1.ProjectName = dr["ProjectName"].ToString();
                        listofproject.Add(ProjName1);
                    }

                }
                return listofproject;
            }


        }


        public List<AdlocationDetails> FetchLocationDetails()
        {

            // AdprojectDetails ProjName = new AdprojectDetails();
            List<AdlocationDetails> listoflocations = new List<AdlocationDetails>();

            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select LocationName from LTLocation", conn);
                //   flag = Convert.ToBoolean(cmd.ExecuteScalar());
                SqlDataReader dr = cmd.ExecuteReader();



                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        AdlocationDetails locName = new AdlocationDetails();
                        locName.LocationName = dr["LocationName"].ToString();
                        listoflocations.Add(locName);
                    }

                }
                return listoflocations;
            }


        }



        public List<AdlocationDetails> FetchBranchDetails()
        {

            // AdprojectDetails ProjName = new AdprojectDetails();
            List<AdlocationDetails> listofbranchs = new List<AdlocationDetails>();

            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select BranchName from LTBranch", conn);
                //   flag = Convert.ToBoolean(cmd.ExecuteScalar());
                SqlDataReader dr = cmd.ExecuteReader();



                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        AdlocationDetails locName = new AdlocationDetails();
                        locName.BranchName = dr["BranchName"].ToString();
                        listofbranchs.Add(locName);
                    }

                }
                return listofbranchs;
            }


        }


        public int ApproveLeavesByAdmin(int id)
        {

            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("update LTLeaveDetails set _Status='Approved' where SequenceNo='" + id + "'", conn);
                //   flag = Convert.ToBoolean(cmd.ExecuteScalar());
                int rowaffected = cmd.ExecuteNonQuery();
                if (rowaffected > 0)
                {
                    return 1;
                }

                else
                {
                    return 0;
                }
            }
        }

        public int RejectLeavesByAdmin(int id)
        {

            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("update LTLeaveDetails set _Status='Rejected' where SequenceNo='" + id + "'", conn);
                int rowaffected = cmd.ExecuteNonQuery();
                if (rowaffected > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<AdminRole> ViewApprovedLeavesByAdmin(string Branch)
        {
            List<AdminRole> listofViewApprovedLeavesByAdmin = new List<AdminRole>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT  t2.Project,SUM(case when tol='Planned' then 1 else 0 end) as planned,
                                                  SUM(case when tol='Unplanned' then 1 else 0 end) as unplanned,
                                                  SUM(case when tol='sick' then 1 else 0 end) as sick
                                                  FROM LTLeaveDetails as t1 join LTLogin as t2 on  t2.MSID= t1.MSID 
                                                  where t1._Status='Approved'  and t2.Sub_Area='" + Branch+"' GROUP BY t2.Project", conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        AdminRole ViewApprovedLeavesByAdmin = new AdminRole();
                        ViewApprovedLeavesByAdmin.ProjectName = dr["Project"].ToString();
                        ViewApprovedLeavesByAdmin.unplanned = Convert.ToInt32(dr["planned"].ToString());
                        ViewApprovedLeavesByAdmin.planned = Convert.ToInt32(dr["unplanned"].ToString());
                        ViewApprovedLeavesByAdmin.sick = Convert.ToInt32(dr["sick"].ToString());
                        listofViewApprovedLeavesByAdmin.Add(ViewApprovedLeavesByAdmin);
                    }

                }
                return listofViewApprovedLeavesByAdmin;

            }

        }



        public List<AdminRole> ViewApprovedLeavesByAdmin(string ProjName, string TOL)
        {
            List<AdminRole> ViewApprovedLeavesByAdminlist = new List<AdminRole>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select t1.FirstName,t1.LastName,t1.Location,t1.Branch,t2.StartDate,t2.EndDate,t2.TOL,
                                                 t2.Reason,t2._Status from LTLogin as t1 join LTLeaveDetails as t2 on t1.MSID=t2.MSID 
                                                 where t2._Status='Approved' and t2.TOL='" + TOL + "' and t1.Project='" + ProjName + "'", conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        AdminRole ViewApprovedLeavesByAdmin = new AdminRole();
                        ViewApprovedLeavesByAdmin.FirstName = dr["FirstName"].ToString();
                        ViewApprovedLeavesByAdmin.LastName = (dr["LastName"].ToString());
                        ViewApprovedLeavesByAdmin.Location = (dr["Location"].ToString());
                        ViewApprovedLeavesByAdmin.Branch = (dr["Branch"].ToString());
                        ViewApprovedLeavesByAdmin.StartDate = (dr["StartDate"].ToString());
                        ViewApprovedLeavesByAdmin.EndDate = (dr["EndDate"].ToString());
                        ViewApprovedLeavesByAdmin.typeOfLeave = (dr["TOL"].ToString());
                        ViewApprovedLeavesByAdmin.Reason = (dr["Reason"].ToString());
                        ViewApprovedLeavesByAdmin._Status = (dr["_Status"].ToString());
                        ViewApprovedLeavesByAdmin.ProjectName = ProjName;
                        ViewApprovedLeavesByAdminlist.Add(ViewApprovedLeavesByAdmin);
                    }

                }
                return ViewApprovedLeavesByAdminlist;
            }

        }


        public List<AdminRole> ViewApprovedLeavesByAdminByProjName(string ProjName)
        {
            List<AdminRole> ViewApprovedLeavesByAdminByProjNamelist = new List<AdminRole>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select t1.FirstName,t1.LastName,t1.Location,t1.Branch,t2.StartDate,t2.EndDate,t2.TOL,
                                                t2.Reason,t2._Status from LTLogin as t1 join LTLeaveDetails as t2 on t1.MSID=t2.MSID 
                                                where t2._Status='Approved'and t1.Project='" + ProjName + "'", conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount != 0)
                {
                    while (dr.Read())
                    {
                        AdminRole ViewApprovedLeavesByAdmin = new AdminRole();
                        ViewApprovedLeavesByAdmin.FirstName = dr["FirstName"].ToString();
                        ViewApprovedLeavesByAdmin.LastName = (dr["LastName"].ToString());
                        ViewApprovedLeavesByAdmin.Location = (dr["Location"].ToString());
                        ViewApprovedLeavesByAdmin.Branch = (dr["Branch"].ToString());
                        ViewApprovedLeavesByAdmin.StartDate = (dr["StartDate"].ToString());
                        ViewApprovedLeavesByAdmin.EndDate = (dr["EndDate"].ToString());
                        ViewApprovedLeavesByAdmin.typeOfLeave = (dr["TOL"].ToString());
                        ViewApprovedLeavesByAdmin.Reason = (dr["Reason"].ToString());
                        ViewApprovedLeavesByAdmin._Status = (dr["_Status"].ToString());
                        ViewApprovedLeavesByAdmin.ProjectName = ProjName;
                        ViewApprovedLeavesByAdminByProjNamelist.Add(ViewApprovedLeavesByAdmin);
                    }

                }
                return ViewApprovedLeavesByAdminByProjNamelist;
            }

        }


        





    }
}