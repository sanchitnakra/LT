using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;

namespace sampleMVC.Models
{
    public class Register
    {
        #region variable declaration
        [Required(ErrorMessage = "Please provide MSID")]
        [Key]
        [Display(Name = "MSID")]
        public string MSID { get; set; }

        [Required(ErrorMessage = "Please provide TCS Emp ID")]
        [Key]
        [Display(Name = "TCS Emp ID")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Count must be a natural number")]
        public string TcsEmpID { get; set; }

        [Required(ErrorMessage = "Please provide FirstName")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please provide LastName")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email ID")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "Please provide Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please provide ConfirmPassword")]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Confirm password does not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please provide Location")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Please provide Project")]
        [Display(Name = "Project")]
        public string Project { get; set; }

        //[Required(ErrorMessage = "Please provide Branch")]
        //[Display(Name = "Branch")]
        //public string Branch { get; set; }

        [Required(ErrorMessage = "Please provide SubArea")]
        [Display(Name = "SubArea")]
        public string SubArea { get; set; }

        [Required(ErrorMessage = "Please provide Supervisor")]
        [Display(Name = "Supervisor")]
        public string Supervisor { get; set; }

        [Required(ErrorMessage = "Please provide CustomerManager")]
        [Display(Name = "CustomerManager")]
        public string CustomerManager { get; set; }


        public List<Register> getUserDetailslist;
        #endregion

        public bool RegisterUser(string MSID, string TcsEmpID, string FirstName, string LastName, string EmailID, string Password, string SubArea, string Supervisor, string CustomerManager, string Project, string Location)
        {
            bool flagForLTLogin = false;
            bool flagForEmail = false;
            string getLocation = string.Empty;
            string getProject = string.Empty;
            string getSubArea = string.Empty;
            string getSupervisor = string.Empty;
            string getCustomerManager = string.Empty;
            getLocation = getLocationByID(Location);
            getProject = getProjectByID(Project);
            getSubArea = getSubAreaByID(SubArea);
            getSupervisor = getSupervisorByID(Supervisor);
            getCustomerManager = getCustomerManagerByID(CustomerManager);

            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmdToLTLogin = new SqlCommand("insert into LTLogin values('" + MSID + "','" + FirstName + "','" + LastName + "','"

                    + Password + "','" + getProject + "','" + getLocation + "' , 'user' , 'FACT','" + EmailID + "' , '" + getSupervisor + "','" + TcsEmpID + "','"
                    + getCustomerManager + "' , '' ,'" + getSubArea + "')", conn);

                SqlCommand cmdToEmail = new SqlCommand("insert into LTEmailID values('" + MSID + "','" + EmailID + "')", conn);

                flagForLTLogin = Convert.ToBoolean(cmdToLTLogin.ExecuteNonQuery());
                flagForEmail = Convert.ToBoolean(cmdToEmail.ExecuteNonQuery());


                if (flagForLTLogin = flagForEmail)
                {
                    return true;
                }
                else { return false; }

            }
        }



        public string getLocationByID(string location)
        {
            string LocationFromDB = string.Empty;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {


                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select * from LTLocation where LocationID = " + location + "", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    LocationFromDB = sr["LocationName"].ToString();
                }
            }
            return LocationFromDB;
        }


        public string getProjectByID(string Project)
        {
            string ProjectFromDB = string.Empty;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {


                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select * from LTProject where ProjectId = " + Project + "", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    ProjectFromDB = sr["ProjectName"].ToString();
                }
            }
            return ProjectFromDB;
        }

        public string getSubAreaByID(string SubArea)
        {
            string ProjectFromDB = string.Empty;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select * from LTSubArea where LTSubAreaID = " + SubArea + "", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    ProjectFromDB = sr["LTSubAreaName"].ToString();
                }
            }
            return ProjectFromDB;
        }

        public string getSupervisorByID(string Supervisor)
        {
            string ProjectFromDB = string.Empty;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select * from LTSupervisor where SupervisorID= " + Supervisor + "", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    ProjectFromDB = sr["SupervisorName"].ToString();
                }
            }
            return ProjectFromDB;
        }

        public string getCustomerManagerByID(string CustomerManager)
        {
            string ProjectFromDB = string.Empty;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select * from LTCustMana where Cust_Mana_ID = " + CustomerManager + "", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    ProjectFromDB = sr["Cust_Mana_Name"].ToString();
                }
            }
            return ProjectFromDB;
        }


        public bool CheckUser(String MSID)
        {
            bool flag = false;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select count(*) from LTLogin where MSID='" + MSID + "'", conn);
                flag = Convert.ToBoolean(cmd.ExecuteScalar());
                return flag;

            }
        }

        public List<Location> getLocation()
        {
            //DropdownLocation dp = new DropdownLocation();

            List<Location> LocationList = new List<Location>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from  LTLocation order by LocationName asc", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    Location loc1 = new Location();
                    loc1.LocationID = Convert.ToInt32(sr["LocationID"]);
                    loc1.LocationName = sr["LocationName"].ToString();
                    LocationList.Add(loc1);
                }

            }
            //dp.LocationList = LocationList;
            return LocationList;
        }

        public List<SubArea> getSubArea()
        {
            //DropdownLocation dp = new DropdownLocation();

            List<SubArea> subAreaList = new List<SubArea>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from  LTSubArea order by LTSubAreaName", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    SubArea subArea = new SubArea();
                    subArea.SubAreaID = Convert.ToInt32(sr["LTSubAreaID"]);
                    subArea.SubAreaName = sr["LTSubAreaName"].ToString();
                    subAreaList.Add(subArea);
                }

            }
            // dp.ProjectNameList = ProjectList;
            return subAreaList;
        }
        public List<Project> getProject()
        {
            //DropdownLocation dp = new DropdownLocation();

            List<Project> ProjectList = new List<Project>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from  LTProject order by ProjectName asc", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    Project loc1 = new Project();
                    loc1.ProjectID = Convert.ToInt32(sr["ProjectID"]);
                    loc1.ProjectName = sr["ProjectName"].ToString();
                    ProjectList.Add(loc1);
                }

            }
            // dp.ProjectNameList = ProjectList;
            return ProjectList;
        }

        public List<Supervisor> getSupervisor()
        {
            //DropdownLocation dp = new DropdownLocation();

            List<Supervisor> SupervisorList = new List<Supervisor>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from  LTSupervisor order by SupervisorName", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    Supervisor supervisorlist = new Supervisor();
                    supervisorlist.SupervisorID = Convert.ToInt32(sr["SupervisorID"]);
                    supervisorlist.SupervisorName = sr["SupervisorName"].ToString();
                    SupervisorList.Add(supervisorlist);
                }

            }
            // dp.ProjectNameList = ProjectList;
            return SupervisorList;
        }

        public List<CustomerManager> getCustManaName()
        {
            //DropdownLocation dp = new DropdownLocation();

            List<CustomerManager> ManaList = new List<CustomerManager>();
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from  LTCustMana order by Cust_Mana_Name", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    CustomerManager CustMana = new CustomerManager();
                    CustMana.CustomerManagerID = Convert.ToInt32(sr["Cust_Mana_ID"]);
                    CustMana.CustomerManagerName = sr["Cust_Mana_Name"].ToString();
                    ManaList.Add(CustMana);
                }

            }
            // dp.ProjectNameList = ProjectList;
            return ManaList;
        }

        public string getAllUSerDetails(string msid)
        {
            string outEmailID = string.Empty;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {


                conn.Open();
                SqlCommand cmd = new SqlCommand(@"select t1.FirstName,t1.LastName,t1.Branch,t1.Project,t1.Location,t2.Email_ID 
                                                    from LTLogin as t1 join LTEmailID as t2 on t1.MSID = t2.MSID 
                                                    where t1.MSID = '" + msid + "'", conn);
                SqlDataReader sr = cmd.ExecuteReader();

                while (sr.Read())
                {
                    Register userDetails = new Register();
                    userDetails.FirstName = sr["FirstName"].ToString();
                    userDetails.LastName = sr["LastName"].ToString();
                    //  userDetails.Branch = sr["Branch"].ToString();
                    userDetails.Project = sr["Project"].ToString();
                    userDetails.Location = sr["Location"].ToString();
                    userDetails.EmailID = sr["Email_ID"].ToString();
                    outEmailID = userDetails.EmailID;
                }

            }


            //dp.LocationList = LocationList;
            return outEmailID;

        }

        public int newAddToDBProject(string project)
        {
            int rows = 0;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmdToNewLocation = new SqlCommand("insert into LTProject values('" + project + "')", conn);
                rows = cmdToNewLocation.ExecuteNonQuery();
            }
            return rows;
        }

        public int newAddToDBSupervisor(string supervisor, string supervisorID)
        {
            int rows = 0;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmdToNewLocation = new SqlCommand("insert into LTSupervisor values('" + supervisor + "','" + supervisorID + "')", conn);
                rows = cmdToNewLocation.ExecuteNonQuery();
            }
            return rows;

        }
        public int newAddToDBCustomerManager(string customermanager)
        {
            int rows = 0;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmdToNewLocation = new SqlCommand("insert into LTCustMana values('" + customermanager + "')", conn);
                rows = cmdToNewLocation.ExecuteNonQuery();
            }
            return rows;

        }

        public int checkRegisteration(string MSID)
        {
            int rows = 0;
            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from LTLogin where MSID = '" + MSID + "'", conn);
                SqlDataReader sr = cmd.ExecuteReader();
                if (sr.HasRows == true)
                {
                    rows = 1;
                }
                else
                {
                    rows = 0;
                }
            }
            return rows;
        }

        public bool editUserInfo(string TcsEmpId, string msid, string EmailID, string SubArea, string Supervisor, string CustomerManager, string Project, string Location)
        {
            bool flagForLTLoginUpdate = false;
            bool flagForEmail = false;
            string getLocation = string.Empty;
            string getProject = string.Empty;
            string getSubArea = string.Empty;
            string getSupervisor = string.Empty;
            string getCustomerManager = string.Empty;
            getLocation = getLocationByID(Location);
            getProject = getProjectByID(Project);
            getSubArea = getSubAreaByID(SubArea);
            getSupervisor = getSupervisorByID(Supervisor);
            getCustomerManager = getCustomerManagerByID(CustomerManager);

            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmdToEmail = new SqlCommand("update LTEmailID set Email_ID ='" + EmailID + "' where MSID ='" + msid + "'", conn);


                SqlCommand cmdToUpdate = new SqlCommand("update LTLogin set Emp_ID = @TcsEmpId, Email_ID = @EmailID, SupervisorName = @supervisorname "
                  + ", Sub_Area = @subArea, Project = @project, Location= @location,Cust_Mana_Name = @custmanaName where MSID ='" + msid + "'", conn);

                cmdToUpdate.Parameters.AddWithValue("@TcsEmpId", TcsEmpId);
                cmdToUpdate.Parameters.AddWithValue("@EmailID", EmailID);
                cmdToUpdate.Parameters.AddWithValue("@supervisorname", getSupervisor);
                cmdToUpdate.Parameters.AddWithValue("@subArea", getSubArea);
                cmdToUpdate.Parameters.AddWithValue("@project", getProject);
                //cmd.Parameters.AddWithValue("@Reason", Reason);
                cmdToUpdate.Parameters.AddWithValue("@location", getLocation);
                cmdToUpdate.Parameters.AddWithValue("@custmanaName", getCustomerManager);


                flagForLTLoginUpdate = Convert.ToBoolean(cmdToUpdate.ExecuteNonQuery());
                flagForEmail = Convert.ToBoolean(cmdToEmail.ExecuteNonQuery());


                if (flagForLTLoginUpdate = flagForEmail)
                {
                    return true;
                }
                else { return false; }

            }

        }


    }

}