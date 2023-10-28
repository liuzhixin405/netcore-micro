namespace Customers.Center.Service.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="token">token</param>
    /// <param name="isSuccess">是否成功</param>
    /// <param name="message">消息</param>
    public record TokenDto(string token,bool isSuccess,string message);
    
    
}
