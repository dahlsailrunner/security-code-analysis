using System.Threading.Tasks;
using WiredBrain.CustomerPortal.Web.Data;
using WiredBrain.CustomerPortal.Web.Models;

namespace WiredBrain.CustomerPortal.Web.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomerByLoyaltyNumber(int loyaltyNumber);
        Task SetFavorite(EditFavoriteModel model);
    }
}