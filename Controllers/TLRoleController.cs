using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using sampleMVC.Models;

namespace sampleMVC.Controllers
{
    public class TLRoleController : Controller
    {
        // GET: TLRole
        public ActionResult TLMainPage()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index", "LTLogin");

        }

        public ActionResult TLMoreFeature()
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
                return RedirectToAction("FetchProjectDetailsAdmin", "TLRole");
            }
            else
            {
                ViewBag.msg = "Project is not added";
                return View();
            }
        }


    }
}