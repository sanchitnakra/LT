using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sampleMVC.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Dynamic;

namespace sampleMVC.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register

        public ActionResult RegisterUser()
        {
            Register reg = new Register();
            DropdownLocation dp = new DropdownLocation();
            dp.LocationList = reg.getLocation();
            dp.ProjectNameList = reg.getProject();
            dp.SupervisorList = reg.getSupervisor();
            dp.CustomerManagerList = reg.getCustManaName();
            dp.SubAreaList = reg.getSubArea();
            return View(dp);
        }


        [HttpPost]
        public ActionResult RegisterUser(Register Register)
        {
            Register reg = new Register();
            DropdownLocation dp = new DropdownLocation();
            dp.LocationList = reg.getLocation();
            dp.ProjectNameList = reg.getProject();
            dp.SupervisorList = reg.getSupervisor();
            dp.CustomerManagerList = reg.getCustManaName();
            dp.SubAreaList = reg.getSubArea();
            if (ModelState.IsValid)
            {
                if (Register.CheckUser(Register.MSID))
                {
                    ViewBag.Message = "The Mentioned MSID is already registered";
                    return View(dp);
                }
                else if (Register.RegisterUser(Register.MSID.ToUpper(), Register.TcsEmpID, Register.FirstName, Register.LastName, Register.EmailID, Register.Password, Register.SubArea, Register.Supervisor, Register.CustomerManager, Register.Project, Register.Location))
                {
                    return RedirectToAction("Index", "LTLogin");
                }
                else
                    return View(dp);
            }
            else
            {
                if (Register.editUserInfo(Register.TcsEmpID, Session["MSID"].ToString(), Register.EmailID, Register.SubArea, Register.Supervisor, Register.CustomerManager, Register.Project, Register.Location))
                {
                    return RedirectToAction("checkRole", "ApplyLeave");
                }
                else
                {
                    return View(dp);
                }

            }

        }


        public ActionResult getDetails(string MSID)
        {
            Register getUserDetails = new Register();
            string emailID = getUserDetails.getAllUSerDetails(MSID);
            return Content(emailID);
        }


        public ActionResult addNewProject(string project)
        {
            int rowAffectedForNewProject = 0;
            Register addNewProjectobj = new Register();
            rowAffectedForNewProject = addNewProjectobj.newAddToDBProject(project);
            if (rowAffectedForNewProject > 0)
            {
                return Content("Project has been added Successfully");
            }
            else
            {
                return Content("");
            }
        }

        public ActionResult addNewSupervisor(string supervisor, string supervisorID)
        {
            int rowAffectedForNewSupervisor = 0;
            Register addNewSupervisorobj = new Register();
            rowAffectedForNewSupervisor = addNewSupervisorobj.newAddToDBSupervisor(supervisor, supervisorID);
            if (rowAffectedForNewSupervisor > 0)
            {
                return Content("Supervisor has been added Successfully");
            }
            else
            {
                return Content("");
            }
        }

        public ActionResult addNewCustomerManager(string customermanager)
        {
            int rowAffectedNewCustomerManager = 0;
            Register addNewCustomerManagerobj = new Register();
            rowAffectedNewCustomerManager = addNewCustomerManagerobj.newAddToDBCustomerManager(customermanager);
            if (rowAffectedNewCustomerManager > 0)
            {
                return Content("Customer Manager has been added Successfully");
            }
            else
            {
                return Content("");
            }
        }

        public ActionResult checkMSIDFormat(string MSID)
        {
            string verified = string.Empty;
            int check = 0;
            Register toCheckRegisteration = new Register();
            check = toCheckRegisteration.checkRegisteration(MSID);
            bool containsInt = MSID.Any(char.IsDigit) ? true : false;
            if (containsInt == false)
            {
                verified = "Please enter your MSID not Short Name";
                return Content(verified);
            }
            else if (check == 1)
            {
                verified = "MSID Already Registered Click on Forgot Password to get the new password and try login";
                return Content(verified);
            }
            else { return Content(verified); }
        }

        public ActionResult checkEmailID(string EmailID)
        {
            string verified = string.Empty;
            bool flag = false;
            flag = EmailID.Contains("ms") ? false : true;
            if (flag == false)
            {
                verified = "Please enter Morgan Stanley EmailID in FirstName.LastName@morganstanley.com format";

                return Content(verified);
            }
            else { return Content(verified); }
        }

    }
}