using System.Linq;
using System.Threading.Tasks;
using WiredBrain.CustomerPortal.Web.Data;
using WiredBrain.CustomerPortal.Web.Models;

namespace WiredBrain.CustomerPortal.Web.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerPortalDbContext dbContext;

        public CustomerRepository()
        {
            dbContext = new CustomerPortalDbContext();
        }

        public Task<Customer> GetCustomerByLoyaltyNumber(int loyaltyNumber)
        {
            var customers = dbContext.Customers
                .SqlQuery("SELECT * FROM CustomersXXX where LoyaltyNumber = " + loyaltyNumber).ToList();
            var customer = customers.FirstOrDefault();
            return Task.FromResult(customer);
        }

        public async Task SetFavorite(EditFavoriteModel model)
        {
            var customer = dbContext.Customers.SingleOrDefault(c => c.LoyaltyNumber == model.LoyaltyNumber);

            customer.FavoriteDrink = model.Favorite;
            await dbContext.SaveChangesAsync();
        }
    }
}
