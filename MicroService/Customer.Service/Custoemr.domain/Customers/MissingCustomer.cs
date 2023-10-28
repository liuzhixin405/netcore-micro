namespace Customers.Domain.Customers;

public class MissingCustomer:Customer
{
    public static readonly MissingCustomer Instance = new MissingCustomer();
}
