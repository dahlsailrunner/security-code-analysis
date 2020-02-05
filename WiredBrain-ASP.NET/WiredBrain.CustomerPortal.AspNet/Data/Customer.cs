using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WiredBrain.CustomerPortal.Web.Data
{
    public class Customer
    {
        public int Id { get; set; }
        public int LoyaltyNumber { get; set; }

        [StringLength(200)]
        public string Name { get; set; }
        public int Points { get; set; }
        public int FreeCoffees { get; set; }

        [StringLength(400)]
        public string FavoriteDrink { get; set; }
    }
}
