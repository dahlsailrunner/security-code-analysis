using System.Web.Mvc;
using SerilogWeb.Classic.Enrichers;

namespace WiredBrain.CustomerPortal.AspNet.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            HttpRequestIdEnricher.TryGetCurrentHttpRequestId(out var guid);
            HttpContext.Items["RequestId"] = guid.ToString();
            return View();
        }
        public ActionResult NotFound()
        {
            return View();
        }
    }
}
