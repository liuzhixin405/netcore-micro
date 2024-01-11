using Paying.WebApi.Database;

namespace Paying.WebApi.Services
{
    public interface IPayingService
    {
        Task<bool> ChangeOrderStatus(long orderId,int status);
        Task<int> GetOrderStatus(long orderId);
        Task<bool> IsPay(long orderId);
        Task<bool> Add(long  orderId);
    }
}
