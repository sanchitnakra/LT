using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebUI.Controllers
{
    public class QuizController : Controller
    {
        // GET: Quiz
        public ActionResult Index(int id)
        {
            Session["Competency_ID"] = id;
            ViewBag.CompetencyId = id;
            return View();
        }

        //public JsonResult GetQuestionsAnswers()
        //{
        //    IEnumerable<QuestionsAnswers> objQuestionAns;
        //    CompetencyService compServ = new CompetencyService();          
        //    objQuestionAns = compServ.GetQuestionsAnswerList(Convert.ToInt32(Session["Competency_ID"]));
        //    Session["StartTime"] = DateTime.Now;
        //    return Json(objQuestionAns.ToList(), JsonRequestBehavior.AllowGet);           
        //}

        //public JsonResult GetCompetencyDetailsByID()
        //{
        //    IEnumerable<Competency> objCompetencyDetails;
        //    CompetencyService compServ = new CompetencyService();
        //    objCompetencyDetails = compServ.GetCompetencyDetailsByID(Convert.ToInt32(Session["Competency_ID"]));
        //    return Json(objCompetencyDetails.ToList(), JsonRequestBehavior.AllowGet);   
        //}

        // [HttpPost]
        // [Route("Quiz/UpdateResult/{score}/{competencyId}")]
        //public ActionResult UpdateResult(float score,int competencyId)
        //{
        //    CompetencyService compServ = new CompetencyService();
        //    QuizResult objResult = new QuizResult();
        //    objResult.MSID = Convert.ToString(Session["MSID"]);
        //    objResult.Score = Convert.ToString(score);
        //    objResult.StartTime = Convert.ToDateTime(Session["StartTime"]);
        //    objResult.Compentecy_ID = competencyId;
        //   // objResult.Quiz_ID = "E0AngularJS";
        //    bool result= compServ.UpdateQuizResult(objResult);

        //    return View();
        
        //}

    }
}