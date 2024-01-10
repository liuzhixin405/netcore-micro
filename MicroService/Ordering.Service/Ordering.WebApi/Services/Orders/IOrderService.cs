namespace Ordering.WebApi.Services.Orders
{
    public interface IOrderService
    {
        Task<bool> ChangeOrderStaus(long id, int status);
    }
}
