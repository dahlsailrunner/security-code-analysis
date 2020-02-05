using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.SystemConsole.Themes;
using WiredBrain.CustomerPortal.AspNet.Controllers;

namespace WiredBrain.CustomerPortal.AspNet
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                //.Enrich.WithHttpContextData()
                .Enrich.FromLogContext()
                .Enrich.WithHttpRequestId()
                .WriteTo.Console(outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Literate)
                .WriteTo.File(
 @"C:\Users\edahl\Source\Repos\WiredBrain.CustomerPortal.AspNet\WiredBrain.CustomerPortal.AspNet\logs\flatfile.json")
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                    IndexFormat = "wired-aspnet-{0:yyyy.MM.dd}",
                    BufferBaseFilename = 
@"C:\Users\edahl\Source\Repos\WiredBrain.CustomerPortal.AspNet\WiredBrain.CustomerPortal.AspNet\logs\buffer",
                    InlineFields = true
                })
                .CreateLogger();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            if (ex == null)
                return;
            var httpStatus = 500;
            if (ex is HttpException httpEx)
            {
                httpStatus = httpEx.GetHttpCode();
            }

            var httpContext = ((MvcApplication)sender).Context;
            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = httpStatus;
            httpContext.Response.TrySkipIisCustomErrors = true;
            string errorControllerAction;
            if (httpStatus == 404)
            {
                errorControllerAction = "NotFound";
            }
            else
            {
                Log.Error(ex, ex.GetBaseException().Message);
                errorControllerAction = "Index";
            }

            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = errorControllerAction;

            var controller = new ErrorController();
            ((IController)controller)
                .Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }
    }
}
