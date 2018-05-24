using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sampleMVC.Models;
using System.IO;
using System.Web.Security;
using System.Net.Mail;
namespace sampleMVC.Controllers
{
    public class ApplyLeaveController : Controller
    {

        bool mailSent = false;
        // GET: ApplyLeave


        public ActionResult checkRole()
        {
            if (Session["Role"].ToString().Equals("admin"))
            {
                return RedirectToAction("ApplyLeavesForAdmin");
            }

            else if (Session["Role"].ToString().Equals("TL"))
            {
                return RedirectToAction("ApplyLeavesForTl");
            }
            else
            {
                return RedirectToAction("ApplyLeaves");
            }
        }
        public ActionResult ApplyLeaves()
        {
            ApplyLeave obj = new ApplyLeave();
            obj.MSID = Session["MSID"].ToString();
            var list = obj.getTypeOfLeave();
            obj.TOL = obj.GetSelectListItem(list);
            return View(obj);
        }

        public ActionResult ApplyLeavesForTl()
        {
            ApplyLeave obj = new ApplyLeave();
            obj.MSID = Session["MSID"].ToString();
            var list = obj.getTypeOfLeave();
            obj.TOL = obj.GetSelectListItem(list);
            return View(obj);
        }
        public ActionResult ApplyLeavesForAdmin()
        {
            ApplyLeave obj = new ApplyLeave();
            obj.MSID = Session["MSID"].ToString();
            var list = obj.getTypeOfLeave();
            obj.TOL = obj.GetSelectListItem(list);
            return View(obj);
        }


        [HttpPost]
        public ActionResult ApplyLeaves(ApplyLeave applyLeave, HttpPostedFileBase file)
        {
            byte[] imageData = null;
            ApplyLeave obj = new ApplyLeave();
            var list = obj.getTypeOfLeave();
            obj.TOL = obj.GetSelectListItem(list);
            if (file != null)
            {
                string name = Path.GetExtension(file.FileName).ToLower();
                applyLeave.MSID = Session["MSID"].ToString();
                if (file.ContentLength > 0 && (name.Equals(".jpg") || name.Equals(".jpeg") || name.Equals(".png")))
                {

                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(file.ContentLength);
                    }

                    if (applyLeave.ApplyLeaves(applyLeave.MSID, applyLeave.StartDate, applyLeave.EndDate, applyLeave.TypeOfLeaves, applyLeave.TotalLeaves, applyLeave.Reason, applyLeave.Status, imageData))
                    {
                        ViewBag.Message = "Success";
                        ApplyLeave applyLeaves = new ApplyLeave();
                        Dictionary<string, string> toGetEmailDetails = applyLeaves.toGetEmailDetails(applyLeave.MSID);
                        Dictionary<string, string> toGetNameDetails = applyLeaves.toGetNameDetails(applyLeave.MSID);
                        mailSent = emailToSupervisor(toGetEmailDetails, toGetNameDetails, applyLeave);
                        return RedirectToAction("ViewLeaves", "ApplyLeave");
                    }
                    else
                    {
                        ViewBag.Message = "Invalid input";
                        return View(obj);
                    }
                }

                else
                {
                    return View(obj);
                }

            }
            else
            {
                byte[] dummy = new byte[0];
                applyLeave.TotalLeaves = SampleOther(applyLeave.StartDate, applyLeave.EndDate);
                if (applyLeave.ApplyLeaves(applyLeave.MSID, applyLeave.StartDate, applyLeave.EndDate, "Planned", applyLeave.TotalLeaves, applyLeave.Reason, applyLeave.Status, dummy))
                {
                    bool result = false;
                    string msidOwn = Session["MSID"].ToString();
                    Dictionary<string, string> toGetEmailandNameDetails = applyLeave.toGetEmailandNameDetailsFromDB(applyLeave.MSID);
                    Dictionary<string, string> toGetSupervisorEmailandNameDetail = applyLeave.toGetNameEmailSupDetails(applyLeave.MSID);
                    Dictionary<string, string> msidOwnEmail = applyLeave.toGetSelfEmailIDOOO(msidOwn);
                    result = emailForPlannedLeave(toGetEmailandNameDetails, toGetSupervisorEmailandNameDetail, msidOwnEmail, applyLeave);
                    if (result == true)
                    {
                        return RedirectToAction("ViewLeaves");
                    }
                    else
                    {
                        return Content("Something went Wrong");
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid input";
                    return View(obj);
                }
            }

        }

        [HttpPost]
        public ActionResult ApplyLeavesForSS(HttpPostedFileBase file)
        {
            byte[] imageData = null;
            int id = Convert.ToInt32(Session["ID"]);
            ApplyLeave obj = new ApplyLeave();
            var list = obj.getTypeOfLeave();
            obj.TOL = obj.GetSelectListItem(list);
            string name = Path.GetExtension(file.FileName).ToLower();
            obj = obj.UpdateeditScreenShot(id);
            Session["MSID"] = obj.MSID;
            if (file.ContentLength > 0 && (name.Equals(".jpg") || name.Equals(".jpeg") || name.Equals(".png")))
            {

                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    imageData = binaryReader.ReadBytes(file.ContentLength);
                }
                //applyLeave.MSID = Session["MSID"].ToString();
                //applyLeave.StartDate = Convert.ToDateTime(Session["StartDate"]);
                //applyLeave.EndDate = Convert.ToDateTime(Session["EndDate"]);
                //applyLeave.TypeOfLeaves = Session["TypeOfLeaves"].ToString();
                //applyLeave.TotalLeaves = Convert.ToInt32(Session["TotalLeaves"]);
                //if (ModelState.IsValid)
                //{
                if (obj.ApplyLeavesSS(id, obj.Reason, imageData))
                //if (obj.ApplyLeavesSS(ob imageData))
                {
                    ViewBag.Message = "Success";
                    ApplyLeave applyLeaves = new ApplyLeave();
                    Dictionary<string, string> toGetEmailDetails = applyLeaves.toGetEmailDetails(obj.MSID);
                    Dictionary<string, string> toGetNameDetails = applyLeaves.toGetNameDetails(obj.MSID);
                    mailSent = emailToSupervisor(toGetEmailDetails, toGetNameDetails, obj);
                    return RedirectToAction("ViewLeaves", "ApplyLeave");
                }
                else
                {
                    ViewBag.Message = "Invalid input";
                    return View(obj);
                }
            }
            else
            {
                return View(obj);
            }
            //}

            //else
            //    return View(obj);
        }


        public ActionResult ViewLeaves()
        {
            int i = 0;
            ApplyLeave obj = new ApplyLeave();
            //ViewData["MSID"] = Session["MSID"];
            string MSID = Session["MSID"].ToString();
            obj.LeaveList = obj.getLeaveList(Session["MSID"].ToString());
            //obj.LeaveList = obj.getLeaveList(MSID);
            return View(obj);
        }

        public ActionResult ViewLeaves1()
        {

            ApplyLeave obj = new ApplyLeave();
            obj.LeaveList = obj.getLeaveList(Session["MSID"].ToString());
            TempData["Success"] = "Leave Updated Successfully";
            return View(obj);
        }

        public ActionResult deleteLeaves(int id)
        {
            ApplyLeave obj = new ApplyLeave();
            obj = ApplyLeave.getLeaveInfo(id);
            if (obj.StartDate.Date > DateTime.Now.Date || obj.Status == "Approval Pending")
            {

                if (ApplyLeave.deleteLeave(id))
                {
                    return RedirectToAction("ViewLeaves", "ApplyLeave");
                }
                else
                {
                    return RedirectToAction("Index", "LTLogin");
                }
            }
            else
            {
                return RedirectToAction("ViewLeaves", "ApplyLeave");

            }
        }

        public ActionResult editLeaves(int id)
        {
            Session["id"] = id;
            ApplyLeave obj = new ApplyLeave();
            obj = ApplyLeave.getLeaveInfoForEditing(id);
            var list = obj.getTypeOfLeave();
            obj.TOL = obj.GetSelectListItem(list);
            Session["MSID"] = obj.MSID;
            Session["FirstName"] = obj.FirstName;
            Session["LastName"] = obj.LastName;

            return View(obj);
        }


        public ActionResult editLeavesScreenShot(int id)
        {

            ApplyLeave obj = new ApplyLeave();
            obj.editScreenShotTLList = ApplyLeave.editScreenSHotForEditing(id);
            var list = obj.getTypeOfLeave();
            obj.TOL = obj.GetSelectListItem(list);
            Session["MSID"] = obj.MSID;
            //Session["FirstName"] = obj.FirstName;
            //Session["LastName"] = obj.LastName;
            Session["StartDate"] = obj.StartDate;
            Session["EndDate"] = obj.EndDate;
            Session["TypeOfLeaves"] = obj.TypeOfLeaves;
            Session["TotalLeaves"] = obj.TotalLeaves;
            Session["ID"] = id;
            return View(obj);
        }

        [HttpPost]
        public ActionResult updateLeaves(ApplyLeave applyLeave, HttpPostedFileBase file)
        {
            int id = 0;
            id = Convert.ToInt32(Session["id"]);
            int rowAffected = 0;
            byte[] imageData = null;
            ApplyLeave obj = new ApplyLeave();
            var list = obj.getTypeOfLeave();
            obj.TOL = obj.GetSelectListItem(list);
            string name = Path.GetExtension(file.FileName).ToLower();
            if (file.ContentLength > 0 && (name.Equals(".jpg") || name.Equals(".jpeg") || name.Equals(".png")))
            {

                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    imageData = binaryReader.ReadBytes(file.ContentLength);
                }
                applyLeave.MSID = Session["MSID"].ToString();
                rowAffected = ApplyLeave.updateLeavesToDB(applyLeave.MSID, applyLeave.StartDate, applyLeave.EndDate, applyLeave.TypeOfLeaves, applyLeave.TotalLeaves, applyLeave.Reason, applyLeave.Status, imageData, id);
            }
            if (rowAffected == 1)
            {
                ViewBag.Message = "Success";
                ApplyLeave applyLeaves = new ApplyLeave();
                Dictionary<string, string> toGetEmailDetails = applyLeaves.toGetEmailDetails(applyLeave.MSID);
                Dictionary<string, string> toGetNameDetails = applyLeaves.toGetNameDetails(applyLeave.MSID);
                mailSent = emailToSupervisor(toGetEmailDetails, toGetNameDetails, applyLeave);
                var message = " Leave Updated Successfully ";

                return RedirectToAction("ViewLeaves1", "ApplyLeave", new { whatever = message });
            }
            else

                return RedirectToAction("ViewLeaves", "ApplyLeave");
        }

        public ActionResult ScreenShot(int Id)
        {
            ApplyLeave obj = new ApplyLeave();
            obj = ApplyLeave.getLeaveInfo(Id);
            return View("ScreenShot", obj);
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "LTLogin");
        }

        public static bool emailToSupervisor(Dictionary<string, string> toGetEmailDetails, Dictionary<string, string> toGetNameDetails, ApplyLeave applyLeave)
        {

            string fullName = string.Empty;
            string supervisorName = string.Empty;
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("mta-hub.ms.com");


            foreach (KeyValuePair<string, string> emailDetails in toGetEmailDetails)
            {
                mail.From = new MailAddress(emailDetails.Key);
                mail.To.Add(emailDetails.Value);
                mail.CC.Add("Sanchit.Nakra@morganstanley.com");
            }

            foreach (KeyValuePair<string, string> nameDetails in toGetNameDetails)
            {
                fullName = nameDetails.Key.ToString();
                supervisorName = nameDetails.Value.ToString();
            }
            mail.CC.Add("Sanchit.Nakra@morganstanley.com");

            mail.Subject = "Request to Approve my Leaves";
            mail.IsBodyHtml = true;

            string emailBody = @"<table> <tr> Hi " + supervisorName + ",</tr>" +
                "<tr>  " +
                "<tr> Could you please approve my leaves? </tr>" +
                    " <tr> MSID : " + applyLeave.MSID + "</tr> " +
                    "<tr> Start Date : " + applyLeave.StartDate.ToShortDateString() + "  </tr> " +
                    "<tr> End Date : " + applyLeave.EndDate.ToShortDateString() + "  </tr> " +
                    "<tr> Total Days : " + applyLeave.TotalLeaves + "  </tr> " +
                      "<tr> Reason : " + applyLeave.Reason + "  </tr>" +
                      "</ tr > " +
                       "<tr> Leave Tracker link :  http://vmvpc091397.msad.ms.com:8080/sampleMVC/ </tr>" +
                  "<tr>  " +
                   "<tr> Key points to note before you start using this ‘Leave Tracker Application’: </tr>" +
                    "<tr>  " +
                       "<tr> •	Please read the user guide attached before applying leave sent before. </tr>" +
                   "<tr> •	This application works best in Google Chrome. </tr>" +
                   "<tr> •	Please attach Ultimatix leave snapshot while attaching the file which is mandatory i.e.apply leaves in Ultimatix first </tr>" +
                   "<tr>•	You can reach out to Sanchit Nakra(sanchn) for any app related issues. </tr>" +
                    "<tr>  " +
                   "<tr> This is automated generated email, Please don't reply to this !! </tr>" +
                    "<tr>" +
                    "<tr>" +
                    "<tr>" + fullName + "</tr>" +

              "<tr>" + "Thanks and Regards</tr>" +
            "</tr>" +
                 "</tr> " +
                 "</table>";
            mail.Body = emailBody;
            SmtpServer.Port = 25;
            SmtpServer.Credentials = new System.Net.NetworkCredential();
            SmtpServer.Send(mail);
            return true;
        }

        public static bool emailForOOO(Dictionary<string, string> toGetEmailandNameDetails, Dictionary<string, string> toGetSupervisorEmailandNameDetails, Dictionary<string, string> toGetMSIDOwn, ApplyLeave applyLeave)
        {
            string fullName = string.Empty, supervisorName = string.Empty;
            string todayDate = DateTime.Now.ToShortDateString();
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("mta-hub.ms.com");


            foreach (KeyValuePair<string, string> emailDetails in toGetEmailandNameDetails)
            {
                fullName = emailDetails.Key;
                mail.From = new MailAddress("Sanchit.Nakra@morganstanley.com");
                mail.To.Add(emailDetails.Value);
            }
            foreach (KeyValuePair<string, string> emailSupervisorDetails in toGetSupervisorEmailandNameDetails)
            {
                supervisorName = emailSupervisorDetails.Key;
                mail.CC.Add(emailSupervisorDetails.Value);
                mail.CC.Add("Sanchit.Nakra@morganstanley.com");
            }

            foreach (KeyValuePair<string, string> MSIDOwn in toGetMSIDOwn)
            {
                mail.CC.Add(MSIDOwn.Value);
            }


            //  mail.CC.Add(emailTO);
            //mail.CC.Add("Himanshu.R.Jain@morganstanley.com");
            //mail.CC.Add("Amol.Tahakik@morganstanley.com");
            //mail.CC.Add("Sanchit.Nakra@morganstanley.com");

            mail.Subject = fullName + " <OOO>";
            mail.IsBodyHtml = true;

            string emailBody = @"<table> <tr> Hi " + fullName + ",</tr>" +
                "<tr>  " +
                 "<tr>  " +
                "<tr> Your Sick leave has been applied for " + todayDate + " in Leave Tracker application. </tr>" +
                 "<tr> Please update the leave with Screenshot. </tr>" +
                 "<tr>  " +
                 "<tr> Leave Tracker link :  http://vmvpc091397.msad.ms.com:8080/sampleMVC/ </tr>" +
                  "<tr>  " +
                   "<tr> Key points to note before you start using this ‘Leave Tracker Application’: </tr>" +
                    "<tr>  " +
                       "<tr> •	Please read the user guide attached before applying leave sent before. </tr>" +
                   "<tr> •	This application works best in Google Chrome. </tr>" +
                   "<tr> •	Please attach Ultimatix leave snapshot while attaching the file which is mandatory i.e.apply leaves in Ultimatix first </tr>" +
                   "<tr>•	You can reach out to Sanchit Nakra(sanchn) for any app related issues. </tr>" +
                    "<tr>  " +
                   "<tr> This is automated generated email, Please don't reply to this !! </tr>" +
                    "<tr>  " +
                    "<tr>  " +
              "<tr>" + "Thanks and Regards</tr>" +
            "</tr>" +
             "<tr>" + "Sanchit Nakra</tr>" +
             "</tr>" +
             "<tr>" + "Leave Tracker Team</tr>" +
                 "</tr> " +
                 "</table>";
            mail.Body = emailBody;
            SmtpServer.Port = 25;
            SmtpServer.Credentials = new System.Net.NetworkCredential();
            SmtpServer.Send(mail);
            return true;
        }



        public static bool emailForPlannedLeave(Dictionary<string, string> toGetEmailandNameDetails, Dictionary<string, string> toGetSupervisorEmailandNameDetails, Dictionary<string, string> toGetMSIDOwn, ApplyLeave applyLeave)
        {
            string fullName = string.Empty, supervisorName = string.Empty;
            string todayDate = DateTime.Now.ToShortDateString();
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("mta-hub.ms.com");


            foreach (KeyValuePair<string, string> emailDetails in toGetEmailandNameDetails)
            {
                fullName = emailDetails.Key;
                mail.From = new MailAddress("Sanchit.Nakra@morganstanley");
                mail.To.Add(emailDetails.Value);
            }
            foreach (KeyValuePair<string, string> emailSupervisorDetails in toGetSupervisorEmailandNameDetails)
            {
                supervisorName = emailSupervisorDetails.Key;
                mail.CC.Add(emailSupervisorDetails.Value);
                mail.CC.Add("Sanchit.Nakra@morganstanley.com");
            }

            foreach (KeyValuePair<string, string> MSIDOwn in toGetMSIDOwn)
            {
                mail.CC.Add(MSIDOwn.Value);
            }


            //  mail.CC.Add(emailTO);
            //mail.CC.Add("Himanshu.R.Jain@morganstanley.com");
            //mail.CC.Add("Amol.Tahakik@morganstanley.com");
            //mail.CC.Add("Sanchit.Nakra@morganstanley.com");

            mail.Subject = fullName + " <Planned Leave>";
            mail.IsBodyHtml = true;

            string emailBody = @"<table> <tr> Hi " + fullName + ",</tr>" +
                "<tr>  " +
                 "<tr>  " +
                "<tr> Your Planned leave has been applied from " + applyLeave.StartDate.ToShortDateString() + " to " + applyLeave.EndDate.ToShortDateString() + " in Leave Tracker application. </tr>" +
                 "<tr> Please update the Planned leave with Screenshot. </tr>" +
                 "<tr>  " +
                 "<tr> Leave Tracker link :  http://vmvpc091397.msad.ms.com:8080/sampleMVC/ </tr>" +
                  "<tr>  " +
                   "<tr> Key points to note before you start using this ‘Leave Tracker Application’: </tr>" +
                    "<tr>  " +
                       "<tr> •	Please read the user guide attached before applying leave sent before. </tr>" +
                   "<tr> •	This application works best in Google Chrome. </tr>" +
                   "<tr> •	Please attach Ultimatix leave snapshot while attaching the file which is mandatory i.e.apply leaves in Ultimatix first </tr>" +
                   "<tr>•	You can reach out to Sanchit Nakra(sanchn) for any app related issues. </tr>" +
                    "<tr>  " +
                   "<tr> This is automated generated email, Please don't reply to this !! </tr>" +
                    "<tr>  " +
                    "<tr>  " +
              "<tr>" + "Thanks and Regards</tr>" +
            "</tr>" +
             "<tr>" + "Sanchit Nakra</tr>" +
             "</tr>" +
             "<tr>" + "Leave Tracker Team</tr>" +
                 "</tr> " +
                 "</table>";
            mail.Body = emailBody;
            SmtpServer.Port = 25;
            SmtpServer.Credentials = new System.Net.NetworkCredential();
            SmtpServer.Send(mail);
            return true;
        }


        public ActionResult Sample(string SD, string ED)
        {
            DateTime SD1 = DateTime.Now;
            DateTime ED1 = DateTime.Now;
            DateTime temp = DateTime.Now;
            int count = 0;
            SD1 = Convert.ToDateTime(SD);
            ED1 = Convert.ToDateTime(ED);
            temp = SD1;
            string DayofWeek = "";

            while (temp <= ED1)
            {
                DayofWeek = temp.DayOfWeek.ToString();
                if (DayofWeek == "Monday" || DayofWeek == "Tuesday" || DayofWeek == "Wednesday" || DayofWeek == "Thursday" || DayofWeek == "Friday")
                {
                    count++;
                }
                temp = temp.AddDays(1);
            }


            return Content(count.ToString());
        }

        public ActionResult validatePass(string PwdTest, string PwdCnf)
        {
            string dummy = string.Empty;
            if (PwdTest.ToUpper() != PwdCnf.ToUpper())
            {
                return Content("Password Doesn't Match");
            }
            else
            {
                return Content(dummy);
            }
        }

        public ActionResult applyOOOForOthers(string MSID)
        {
            string msidOwn = Session["MSID"].ToString();
            bool flag = false;
            bool result = false;
            byte[] dummy = new byte[0];
            DateTime todayData = DateTime.Now;
            ApplyLeave applyLeaveObj = new ApplyLeave();
            flag = applyLeaveObj.ApplyLeaves(MSID, todayData, todayData, "Sick/UnPlanned", 1, "", "Approval Pending", dummy);
            Dictionary<string, string> toGetEmailandNameDetails = applyLeaveObj.toGetEmailandNameDetailsFromDB(MSID);
            Dictionary<string, string> toGetSupervisorEmailandNameDetail = applyLeaveObj.toGetNameEmailSupDetails(MSID);
            Dictionary<string, string> msidOwnEmail = applyLeaveObj.toGetSelfEmailIDOOO(msidOwn);
            result = emailForOOO(toGetEmailandNameDetails, toGetSupervisorEmailandNameDetail, msidOwnEmail, applyLeaveObj);
            if (flag == true && result == true)
            {
                return Content("Leave Applied Succesfully");
            }
            else
            {
                return Content("Something went Wrong");
            }
        }

        public ActionResult applyOOOForOthersUser(string msad)
        {
            string msidOwn = Session["MSID"].ToString();
            bool flag = false;
            bool result = false;
            byte[] dummy = new byte[0];
            DateTime todayData = DateTime.Now;
            ApplyLeave applyLeaveObj = new ApplyLeave();
            flag = applyLeaveObj.ApplyLeaves(msad, todayData, todayData, "Sick/UnPlanned", 1, "", "Approval Pending", dummy);
            Dictionary<string, string> toGetEmailandNameDetails = applyLeaveObj.toGetEmailandNameDetailsFromDB(msad);
            Dictionary<string, string> toGetSupervisorEmailandNameDetail = applyLeaveObj.toGetNameEmailSupDetails(msad);
            Dictionary<string, string> msidOwnEmail = applyLeaveObj.toGetSelfEmailIDOOO(msidOwn);
            result = emailForOOO(toGetEmailandNameDetails, toGetSupervisorEmailandNameDetail, msidOwnEmail, applyLeaveObj);
            if (flag == true && result == true)
            {
                return Content("Leave Applied Succesfully");
            }
            else
            {
                return Content("Something went Wrong");
            }
        }

        [HttpPost]
        public ActionResult addScreenShot(ApplyLeave applyLeave, HttpPostedFileBase file)
        {
            byte[] imageData = null;
            ApplyLeave obj = new ApplyLeave();
            //var list = obj.getTypeOfLeave();
            //obj.TOL = obj.GetSelectListItem(list);
            string name = Path.GetExtension(file.FileName).ToLower();
            if (file.ContentLength > 0 && (name.Equals(".jpg") || name.Equals(".jpeg") || name.Equals(".png")))
            {

                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    imageData = binaryReader.ReadBytes(file.ContentLength);
                }
                applyLeave.MSID = Session["MSID"].ToString();

            }
            return View();
        }


        public ActionResult ExportToExcelForTL()
        {
            // string ProjName = Session["ProjName"].ToString();
            ApplyLeave applyLeaveObj = new ApplyLeave();
            applyLeaveObj.ExportToExcelForTLList = applyLeaveObj.ExportToExcelForTL();

            //Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            //string filename = string.Empty;
            //string PresentFilename = string.Empty;
            //filename = DateTime.Today.ToString();
            //PresentFilename = filename.Substring(0,5) + " Leave Tracker Report";
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Today.ToString().Substring(0, 5) + " Leave Tracker Report" + "");
            Response.ContentType = "application/excel";

            return View(applyLeaveObj);
        }

        public ActionResult checkLeave(string SD)
        {
            string MSID = Session["MSID"].ToString();
            if (ApplyLeave.toCheckLeave_OOO(MSID, SD) == true)
            {
                return Content("Your Leave is already applied. Go to view leave and update the screenshot");
            }
            else { return Content(""); }

        }

        public ActionResult applyOtherPlannedLeave()
        {

            ApplyLeave obj = new ApplyLeave();
            obj.MSID = Session["MSID"].ToString();
            var list = obj.getTypeOfLeave();
            obj.TOL = obj.GetSelectListItem(list);
            return View(obj);
        }

        public int SampleOther(DateTime SD1, DateTime ED1)
        {
            DateTime SD2 = DateTime.Now;
            DateTime ED2 = DateTime.Now;
            DateTime temp = DateTime.Now;
            int count = 0;
            SD2 = Convert.ToDateTime(SD1);
            ED2 = Convert.ToDateTime(ED1);
            temp = SD2;
            string DayofWeek = "";

            while (temp <= ED2)
            {
                DayofWeek = temp.DayOfWeek.ToString();
                if (DayofWeek == "Monday" || DayofWeek == "Tuesday" || DayofWeek == "Wednesday" || DayofWeek == "Thursday" || DayofWeek == "Friday")
                {
                    count++;
                }
                temp = temp.AddDays(1);
            }


            return count;
        }

        public ActionResult EditInfo()
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
        public JsonResult MSIDfinder(string Prefix)
        {
            ApplyLeave app = new ApplyLeave();
            var MSIDList = app.SearchMSIDByName(Prefix);
            return Json(MSIDList, JsonRequestBehavior.AllowGet);

        }

        public ActionResult MSIDFinderView()
        {
            return View();
        }

        public ActionResult ExportToExcelForUser()
        {
            // string ProjName = Session["ProjName"].ToString();
            ApplyLeave applyLeaveObj = new ApplyLeave();
            applyLeaveObj.ExportToExcelForUserList = applyLeaveObj.ExportToExcelForUser();

            //Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            //string filename = string.Empty;
            //string PresentFilename = string.Empty;
            //filename = DateTime.Today.ToString();
            //PresentFilename = filename.Substring(0,5) + " Leave Tracker Report";
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Today.ToString().Substring(0, 5) + " MSID FirstName and LastName list" + "");
            Response.ContentType = "application/excel";

            return View(applyLeaveObj);
        }

        public ActionResult viewInfo()
        {
            string MSID = Session["MSID"].ToString();
            ApplyLeave viewInfoObj = new ApplyLeave();
            viewInfoObj.userInfoDetails = viewInfoObj.viewUserInfo(MSID);
            return View(viewInfoObj);
        }
    }
}

