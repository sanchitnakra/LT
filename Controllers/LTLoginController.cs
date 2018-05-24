using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sampleMVC.Models;
using System.Web.Security;
using System.Net.Mail;

namespace sampleMVC.Controllers
{
    public class LTLoginController : Controller
    {
        // GET: LTLogin
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Index(login login)

        {
            if (ModelState.IsValid)
            {
                login log = new login();
                if (login.checkUser(login))
                {
                    log = login.getInfo(login);
                    if (log.Role.Equals("user"))
                    {
                        Session["MSID"] = login.MSID;
                        Session["FirstName"] = log.FirstName;
                        Session["LastName"] = log.LastName;
                        Session["Location"] = log.Location;
                        Session["Project"] = log.Project;
                        Session["Branch"] = log.Branch;
                        Session["SubArea"] = log.Sub_Area;
                        Session["Emp_ID"] = log.Emp_ID;
                        Session["Role"] = log.Role;
                        return RedirectToAction("ApplyLeaves", "ApplyLeave");
                    }
                    else if (log.Role.Equals("TL"))
                    {
                        Session["MSID"] = login.MSID;
                        Session["FirstName"] = log.FirstName;
                        Session["LastName"] = log.LastName;
                        Session["Location"] = log.Location;
                        Session["Project"] = log.Project;
                        Session["Branch"] = log.Branch;
                        Session["SubArea"] = log.Sub_Area;
                        Session["Emp_ID"] = log.Emp_ID;
                        Session["Role"] = log.Role;
                        return RedirectToAction("ApplyLeavesForTl", "ApplyLeave");

                    } 
                    else if (log.Role.Equals("admin"))
                    {
                        Session["MSID"] = login.MSID;
                        Session["FirstName"] = log.FirstName;
                        Session["LastName"] = log.LastName;
                        Session["Location"] = log.Location;
                        Session["Branch"] = log.Branch;
                        Session["Project"] = log.Project;
                        Session["SubArea"] = log.Sub_Area;
                        Session["Emp_ID"] = log.Emp_ID;
                        Session["Role"] = log.Role;
                        return RedirectToAction("AdminRole", "AdminRole");
                    }
                    else if (log.Role.Equals("superadmin"))
                    {
                        Session["MSID"] = login.MSID;
                        Session["Branch"] = log.Branch;
                        Session["Emp_ID"] = log.Emp_ID;
                        Session["Role"] = log.Role;
                        return RedirectToAction("GetPagViewBySuperAdmin", "SuperAdmin");
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid Username or Password";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult RegisterRedirect()
        {
            return RedirectToAction("RegisterUser", "Register");
        }


        public ActionResult Show()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "LTLogin");
        }


        public ActionResult checkPassword(string MSID)
        {
            bool? result = null;
            login toCheckPassword = new login();
            result = toCheckPassword.getPassword(MSID.ToUpper());
            if (result.Equals(false))
            {
                result = null;
            }
            return Content(result.ToString());
        }


        public ActionResult forgetPassword(string MSID)
        {
            //random number generate karo
            bool mailSent = false;
            string result = string.Empty;
            int tempPass = 0;
            List<string> dumy = new List<string>();
            int rowAffected = 0;
            string toGetEmailID = string.Empty;
            string tempFinalPass = string.Empty;
            Random tempPassGenerator = new Random();
            tempPass = tempPassGenerator.Next();
            tempFinalPass = MSID.ToUpper() + (tempPass.ToString()).Substring(0, 4);
            // db mein update karo for Provided MSID
            login tempPassToDB = new login();
            rowAffected = tempPassToDB.updateTempPassTODB(MSID.ToUpper(), tempFinalPass);
            // retrive email id from DB  using MSID and send random number to User
            if (rowAffected == 1)
            {
                toGetEmailID = tempPassToDB.toGetEmailIDFromDB(MSID.ToUpper());
                mailSent = emailToSupervisor(toGetEmailID, tempFinalPass);
            }
            //pop up to show that please check your email 
            result = "Your password is sent to your OutLook";
            return Content(result);
        }

        public static bool emailToSupervisor(string toGetEmailID, string tempFinalPass)
        {

            string fullName = string.Empty;
            string supervisorName = string.Empty;
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("mta-hub.ms.com");
            mail.From = new MailAddress(toGetEmailID);
            mail.To.Add(toGetEmailID);

            //foreach (KeyValuePair<string, string> nameDetails in toGetNameDetails)
            //{
            //    fullName = nameDetails.Key.ToString();
            //    supervisorName = nameDetails.Value.ToString();
            //}
            //mail.CC.Add(emailTO);


            mail.Subject = "Temporary Password For Leave Tracker";
            mail.IsBodyHtml = true;

            string emailBody = @"<table> <tr> Hi " + ",</tr>" +
                "<tr>  " +
                "<tr> Your Temporary Password for Leave Tracker is </tr>" +
                    " <tr> Password : " + tempFinalPass + "</tr> " +

                    "<tr>" +
                    "<tr>" +
                    "<tr>" + "</tr>" +

              "<tr>" + "Thanks and Regards</tr>" +
            "</tr>" +
             "<tr>" + "Leave Tracker Team </tr>" +
                 "</tr> " +
                 "</table>";
            mail.Body = emailBody;
            SmtpServer.Port = 25;
            SmtpServer.Credentials = new System.Net.NetworkCredential();
            SmtpServer.Send(mail);
            return true;
        }

        public ActionResult ChangePassword(string Pwd)
        {
            int updatePasswordByUserResult = 0;
            string pwdFromLogin = string.Empty;
            pwdFromLogin = Session["MSID"].ToString();
            login updatePasswordByUser = new login();
            updatePasswordByUserResult = updatePasswordByUser.updatePassword(pwdFromLogin.ToUpper(),Pwd);
            if (updatePasswordByUserResult == 1)
            {
                return Content("Password is updated Successfully");
            }
            else
            {
                return RedirectToAction("ApplyLeaves", "ApplyLeave");
            }
        }

        public ActionResult validateMSID(string MSID)
        {
            string result = string.Empty;
            string empty = string.Empty;
            login toCheckPassword = new login();
            result = toCheckPassword.validateMSID(MSID.ToUpper());
            if (result != MSID.ToUpper())
            {
                return Content("Invalid MSID Please Try again");
            }
            else
            {
                return Content(empty);
            }
        }

        public ActionResult logExtraHrs(string dateLogHrs, string hrs, string logReason)
        {
            int rowAffected = 0;
            String MSID = string.Empty;
            MSID = Session["MSID"].ToString();
            DateTime dateLogHrsFormat = Convert.ToDateTime(dateLogHrs);
            login logExtraHrs = new login();
            rowAffected = logExtraHrs.extraHrsLog(MSID, hrs, dateLogHrsFormat, logReason);
            if (rowAffected == 1)
            {
                return Content("Your Extra hours are logged Successfully!!");
            }
            else
            {
                return Content("Something Went Wrong!!");
            }
        }


       
    }
}