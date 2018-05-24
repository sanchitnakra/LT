using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sampleMVC.Models;
using System.Web.Security;
//using QuizProject.Entities;
//using QuizProject.DAL;
//using QuizProject.Service;

namespace sampleMVC.Controllers
{
    public class CompetencyController : Controller
    {
        // GET: Competency 
        // phele wala 
        public ActionResult viewCompetency()
        {
            string MSID = string.Empty;
            MSID = Session["MSID"].ToString();
            Competency getCompetency = new Competency();
            getCompetency.getCoursesFromDBList = getCompetency.getCompentencyByMSID(MSID);
            if (getCompetency.getCoursesFromDBList.Count > 0)
            {
                return View(getCompetency);
            }
            else { return View(getCompetency); }
           
        }
        public ActionResult addCourses()
        {
            Competency compObj = new Competency();
            var list = compObj.getCompLevel();
            compObj.selectCompLevel = compObj.GetSelectListItem(list);
            return View(compObj);
        }

        [HttpPost]
        public ActionResult getCourses(Competency competency)
        {
            string compLevel = competency.compLevel;
            Competency compObj = new Competency();
            Session["compLevel"] = compObj.compLevel;
            compObj.getCoursesFromDBList = compObj.getCoursesFromDB(compLevel);
            return View(compObj);
        }

        public ActionResult enrollCourse(int id)
        {
            int rowAffected = 0;
            string quizID = string.Empty;
            string compLevelFromSession = string.Empty;
            Session["compLevel"] = compLevelFromSession;
            Competency compObj = new Competency();
            //generate QuizID
            List<string> courseDetails = new List<string>();
            courseDetails = compObj.getCourseLevelandNameByID(id);
            quizID = courseDetails[0] + courseDetails[1];
            rowAffected = compObj.enrollCourse(Convert.ToInt32(Session["Emp_ID"]), Session["MSID"].ToString(), id, quizID);
            return RedirectToAction("viewCompetency");
        }

        public ActionResult dropCourse(int id)
        {
            Competency compObj = new Competency();
            int rowAffected = 0;
            rowAffected = compObj.dropCourse(id);
            return RedirectToAction("viewCompetency");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "LTLogin");
        }

        //public ActionResult GetCompetencyList()
        //{
        //    //  CompetencyService compServ = new CompetencyService();
        //    // compServ.Equals(competencyLevel);
        //    return View();
        //}

        //public JsonResult GetCompetencyListById(string id)
        //{
        //    IEnumerable<Competency> objCompetency;
        //    CompetencyService compServ = new CompetencyService();
        //    objCompetency = compServ.GetCompetencyList(id);
        //    return Json(objCompetency.ToList(), JsonRequestBehavior.AllowGet);
        //}

    }
}