using System;
using WiredBrain.CustomerPortal.Web.Data;

namespace WiredBrain.CustomerPortal.Web.Models
{
    public class LoyaltyModel
    {
        public int CustomerId { get; set; }
        public int LoyaltyNumber { get; set; }
        public string CustomerName { get; set; }
        public int Points { get; set; }
        public int PercentageAchieved { get; set; }
        public int PointsToGo { get; set; }
        public int FreeCoffees { get; set; }
        public string FavoriteDrink { get; set; }

        public static LoyaltyModel FromCustomer(Customer customer, int pointsNeeded)
        {
            var percentageAchieved = (customer.Points / (double)pointsNeeded) * 100;
            return new LoyaltyModel
            {
                CustomerId = customer.Id,
                CustomerName = customer.Name,
                FavoriteDrink = customer.FavoriteDrink,
                FreeCoffees = customer.FreeCoffees,
                LoyaltyNumber = customer.LoyaltyNumber,
                PercentageAchieved = (int)percentageAchieved,
                Points = customer.Points,
                PointsToGo = pointsNeeded - customer.Points
            };
        }
    }
}
