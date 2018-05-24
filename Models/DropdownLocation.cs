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
    public class DropdownLocation
    {
        [Required(ErrorMessage = "Please provide MSID")]
        [Key]
        [Display(Name = "MSID")]
        public string MSID { get; set; }

        [Required(ErrorMessage = "Please provide TCS Emp ID")]
        [Key]
        [Display(Name = "TCS Emp ID")]
        public string TcsEmpID { get; set; }

        [Required(ErrorMessage = "Please provide FirstName")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please provide LastName")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

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
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public List<Location> LocationList { get; set; }

        [Required(ErrorMessage = "Please provide Project")]
        [Display(Name = "Project")]
        public string Project { get; set; }


        [Required(ErrorMessage = "Please provide Supervisor")]
        [Display(Name = "Supervisor")]
        public string Supervisor { get; set; }
        public int SupervisorID { get; set; }
        public string SupervisorName { get; set; }
        public List<Supervisor> SupervisorList { get; set; }

        [Required(ErrorMessage = "Please provide Customer Manager")]
        [Display(Name = "Customer Manager")]
        public string CustomerManager { get; set; }
        public int CustomerManagerID { get; set; }
        public string CustomerManagerName { get; set; }
        public List<CustomerManager> CustomerManagerList { get; set; }


        [Display(Name = "Morgan Stanley Email ID")]
        public string EmailID { get; set; }


        [Required(ErrorMessage = "Please provide Branch")]
        [Display(Name = "Branch")]
        public string Branch { get; set; }


        [Required(ErrorMessage = "Please provide Sub Area")]
        [Display(Name = "Sub_Area")]
        public string SubArea { get; set; }
        public int SubAreaID { get; set; }
        public string SubAreaName { get; set; }
        public List<SubArea> SubAreaList { get; set; }

        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public List<Project> ProjectNameList { get; set; }
        
        public List<Register> userDetailsList { get; set; }

        [Required(ErrorMessage = "Please enter new Project Name")]
     
        [Display(Name = "Project Name")]
        public string newProjectName { get; set; }

        [Required(ErrorMessage = "Please provide Supervisor Name")]

        [Display(Name = "Supervisor Name")]
        public string newSupervisor { get; set; }


        [Required(ErrorMessage = "Please provide Supervisor Morgan Stanley Email address")]

        [Display(Name = "Supervisor Morgan Stanley Email Address")]
        public string newSupervisorEmailID { get; set; }


        [Required(ErrorMessage = "Please  provide Customer Manager Name")]
        [Display(Name = "Customer Manager Name")]
        public string newCustomerManager { get; set; }

    }
}