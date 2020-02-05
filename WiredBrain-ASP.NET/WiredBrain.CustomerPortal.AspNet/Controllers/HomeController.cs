using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using WiredBrain.CustomerPortal.Web.Models;
using WiredBrain.CustomerPortal.Web.Repositories;

namespace WiredBrain.CustomerPortal.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerRepository repo;

        public HomeController()
        {
            repo = new CustomerRepository();
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Enter loyalty number";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(int loyaltyNumber)
        {
            var customer = await repo.GetCustomerByLoyaltyNumber(loyaltyNumber);
            if (customer == null)
            {
                ModelState.AddModelError(string.Empty, "Unknown loyalty number");
                return View();
            }
            return RedirectToAction("LoyaltyOverview", new { loyaltyNumber });
        }

        public async Task<ActionResult> LoyaltyOverview(int loyaltyNumber)
        {
            ViewBag.Title = "Your points";
            var cookieName = "LoyaltyInfo";

            if (Request.Cookies[cookieName] != null)
            {
                var loyaltyInfo = JsonConvert.DeserializeObject<LoyaltyModel>(Request.Cookies[cookieName].Value);
                return View(loyaltyInfo);
            }
            var customer = await repo.GetCustomerByLoyaltyNumber(loyaltyNumber);
            var pointsNeeded = int.Parse(ConfigurationManager.AppSettings["CustomerPortalSettings:PointsNeeded"]);

            var loyaltyModel = LoyaltyModel.FromCustomer(customer, pointsNeeded);
            Response.Cookies.Add(new System.Web.HttpCookie("LoyaltyInfo", JsonConvert.SerializeObject(loyaltyModel)) {
                Expires = DateTime.Now.AddHours(2) });
            return View(loyaltyModel);
        }

        public async Task<ActionResult> EditFavorite(int loyaltyNumber)
        {
            ViewBag.Title = "Edit favorite";

            var customer = await repo.GetCustomerByLoyaltyNumber(loyaltyNumber);
            return View(new EditFavoriteModel
            {
                LoyaltyNumber = customer.LoyaltyNumber,
                Favorite = customer.FavoriteDrink
            });
        }

        [HttpPost]
        public async Task<ActionResult> EditFavorite(EditFavoriteModel model)
        {
            await repo.SetFavorite(model);
            return RedirectToAction("LoyaltyOverview", new { loyaltyNumber = model.LoyaltyNumber });
        }
    }
}
