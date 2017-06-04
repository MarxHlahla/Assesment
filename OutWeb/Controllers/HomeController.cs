using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OutWeb.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
		public test.Out.OutEntity.ActionController ActionContr
		{
		get { return new test.Out.OutEntity.ActionController(); }
		}
		[HttpGet]
		public JsonResult GetLoadData()
		{
			
			string scvFile = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["CSVfileLocation"].ToString());
			ActionContr.LoadData(scvFile);
			return Json(test.Out.OutEntity.ActionController.UserData, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public JsonResult GetFreq()
		{
			return Json(new {FrequentNames= ActionContr.FirstNameFreq(), FrequentNameSurFreq = ActionContr.FirstNameSurFreq() }, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public JsonResult GetAddress() 
		{
			return Json(ActionContr.GetAddresses(), JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public JsonResult WriteToFile(string Scenario)
		{
			string location= Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["FileLocation"].ToString()); 
			object msg = ActionContr.WriteToFile(Scenario,location);
			return Json(msg, JsonRequestBehavior.AllowGet);
		}
		public ActionResult About()
		{
			ViewBag.Message = "This is an Outsurance application";
			return View();
		}
		public ActionResult Contact()
		{
			ViewBag.Message = "Contact Marks";
			return View();
		}
	}
}