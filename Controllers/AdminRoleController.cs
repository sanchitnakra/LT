using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sampleMVC.Models;
using System.Web.Security;

namespace sampleMVC.Controllers
{
    public class AdminRoleController : Controller


    {


        // GET: AdminRole
        public ActionResult AdminRole()
        {
            return View();
        }

        public ActionResult AdminMoreFeature()
        {
            return View();
        }



        public ActionResult ApplyLeavesforAdmins()
        {
            return View();
        }

        public ActionResult FetchProjectDetailsAdmin(AdminRole adminrole)


        {
            AdminRole list1 = new AdminRole();
            List<AdprojectDetails> list2 = new List<AdprojectDetails>();
            list2 = list1.FetchProjectDetails();
            ViewBag.list = list2;
            return View();
        }

        [HttpPost]
        public ActionResult InsertProjectToDB(AdprojectDetails AdprojectDetails)
        {
            int i = 0;
            AdProjectByTL sample = new AdProjectByTL();
            i = sample.InsertProjectToDB(AdprojectDetails);
            if (i == 1)
            {
                return RedirectToAction("FetchProjectDetailsAdmin", "AdminRole");
            }
            else
            {
                ViewBag.msg = "Project is not added";
                return View();
            }
        }

        public ActionResult FetchBranchDetailsByAdmin()
        {
            AdminRole list1 = new AdminRole();
            List<AdlocationDetails> list2 = new List<AdlocationDetails>();
            list2 = list1.FetchBranchDetails();
            ViewBag.list = list2;
            return View();

        }


        [HttpPost]
        public ActionResult InsertToDBBranchDetailsByAdmin(AdlocationDetails AdlocationDetails)
        {
            int i = 0;
            AdProjectByTL sample = new AdProjectByTL();
            i = sample.InsertBranchToDB(AdlocationDetails);
            if (i == 1)
            {
                return RedirectToAction("FetchBranchDetailsByAdmin", "AdminRole");
            }
            else
            {
                ViewBag.msg = "Project is not added";
                return View();
            }
        }

        public ActionResult FetchLocationDetails()
        {
            AdminRole list1 = new AdminRole();
            List<AdlocationDetails> list2 = new List<AdlocationDetails>();
            list2 = list1.FetchLocationDetails();
            ViewBag.list = list2;
            return View();

        }



        [HttpPost]
        public ActionResult InsertlocationToDB(AdlocationDetails AdlocationDetails)
        {
            int i = 0;
            AdProjectByTL sample = new AdProjectByTL();
            i = sample.InsertLocationToDB(AdlocationDetails);
            if (i == 1)
            {
                return RedirectToAction("FetchLocationDetails", "AdminRole");
            }
            else
            {
                ViewBag.msg = "Location is not added";
                return View();
            }
        }

        public ActionResult ViewLeavesByAdmin()
        {
            ApplyLeave obj = new ApplyLeave();
            string Branch = Session["Branch"].ToString();
            // added by me for testing 
            string Project = Session["Project"].ToString();
            string SubArea = Session["SubArea"].ToString();
            string MSID = Session["MSID"].ToString();
          //  string MSIDsupervisor = obj.findSupervisorByMSID(MSID);
            obj.LeaveList = obj.getLeave(SubArea);
            //ApproveLeavesByAdmin(obj);
            return View(obj);
        }


        public ActionResult ScreenShot(int Id)
        {
            ApplyLeave obj = new ApplyLeave();
            obj = ApplyLeave.getLeaveInfo(Id);
            return View("ScreenShot", obj);
        }


        public ActionResult ApproveLeavesByAdmin(int id)
        {


            AdminRole toApproveLeaveByAdmin = new AdminRole();
            int i = toApproveLeaveByAdmin.ApproveLeavesByAdmin(id);
            if (i > 0)
            {

                return RedirectToAction("ViewLeavesByAdmin", "AdminRole");
            }
            else
                return RedirectToAction("ViewLeavesByAdmin", "AdminRole");
        }


        public ActionResult RejectLeavesByAdmin(int id)
        {
            AdminRole toRejectLeaveByAdmin = new AdminRole();
            int i = toRejectLeaveByAdmin.RejectLeavesByAdmin(id);
            if (i > 0)
            {

                return RedirectToAction("ViewLeavesByAdmin", "AdminRole");
            }
            else
                return RedirectToAction("ViewLeavesByAdmin", "AdminRole");
        }


        public ActionResult ViewApprovedLeavesByAdmin()
        {
            AdminRole approvedLeavesByAdmin = new AdminRole();
            string Branch = Session["Branch"].ToString();
            string Project = Session["Project"].ToString();
            string SubArea = Session["SubArea"].ToString();
            approvedLeavesByAdmin.AppliedLeavesList = approvedLeavesByAdmin.ViewApprovedLeavesByAdmin(SubArea);
            return View(approvedLeavesByAdmin);
        }

        // View to list all type of leaves when he click on Project Name
        public ActionResult ViewApprovedLeavesByAdminByProjName(string ProjName)
        {
            AdminRole ViewApprovedLeavesByAdminByProjName = new AdminRole();
            ViewApprovedLeavesByAdminByProjName.ViewApprovedLeavesByAdminByProjNameList = ViewApprovedLeavesByAdminByProjName.ViewApprovedLeavesByAdminByProjName(ProjName);
            return View(ViewApprovedLeavesByAdminByProjName);
        }


        // View to list all Unplanned leaves when he click on Data in Table
        public ActionResult ViewApprovedLeavesByAdminByProjNameByTOL(string ProjName, string TOL)
        {
            AdminRole ViewApprovedLeavesByAdmin = new AdminRole();
            ViewApprovedLeavesByAdmin.ViewApprovedLeavesByAdminlist = ViewApprovedLeavesByAdmin.ViewApprovedLeavesByAdmin(ProjName, TOL);
            return View(ViewApprovedLeavesByAdmin);
        }

        // View to list all Planned leaves when he click on Data in Table
        //public ActionResult TOLandPRojectForplannedByAdmin(string ProjName)
        //{
        //    AdminRole TOLandPRojectForplannedByAdmin = new AdminRole();
        //    TOLandPRojectForplannedByAdmin.TOLandPRojectForplannedByAdminList = TOLandPRojectForplannedByAdmin.TOLandPRojectForPlannedByAdmin(ProjName);
        //    return View(TOLandPRojectForplannedByAdmin);
        //}

        // View to list all Sick leaves when he click on Data in Table
        //public ActionResult TOLandPRojectForSickByAdmin(string ProjName)
        //{
        //    AdminRole TOLandPRojectForSickByAdmin = new AdminRole();
        //    TOLandPRojectForSickByAdmin.TOLandPRojectForSickByAdminList = TOLandPRojectForSickByAdmin.TOLandPRojectForSickByAdmin(ProjName);
        //    return View(TOLandPRojectForSickByAdmin);
        //}

        public ActionResult Logout()
        {
            //Session.Remove("MSID");
            //Session.Clear();
            //Session.Abandon();

            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "LTLogin");
        }



    }
}