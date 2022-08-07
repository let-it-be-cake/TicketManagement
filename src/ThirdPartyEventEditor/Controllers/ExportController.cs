using System;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using ThirdPartyEventEditor.Filters;

namespace ThirdPartyEventEditor.Controllers
{
    [ExecutionTime]
    public class ExportController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("GetEvents");
        }

        public ActionResult GetEventsFile()
        {
            return File(Server.MapPath(@"..\\App_Data\\") + "Events.json", "application/json", "Events.json");
        }

        public ActionResult GetEventsResponse()
        {
            try
            {
                var result = new JsonResult()
                {
                    Data = System.IO.File.ReadAllText(Server.MapPath(@"..\\App_Data\\") + "Events.json"),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                };
                return result;
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
                throw;
            }
        }
    }
}