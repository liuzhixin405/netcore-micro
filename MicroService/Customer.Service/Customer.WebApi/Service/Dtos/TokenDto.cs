namespace Customers.Center.Service.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="token">token</param>
    /// <param name="isSuccess">是否成功</param>
    /// <param name="message">消息</param>
    public record TokenDto
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public TokenDto(bool isSuccess, string token, string message)
        {
            IsSuccess = isSuccess;
            Token = token;
            Message = message;  
        }
    }
    
    
}
