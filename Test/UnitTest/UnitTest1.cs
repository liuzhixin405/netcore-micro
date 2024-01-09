namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // 获取当前的Unix时间戳
            long unixTimeMilliseconds = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // 将Unix时间戳转换为DateTimeOffset对象
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMilliseconds);

            // 格式化为 yyyy-MM-dd HH:mm:ss 字符串
            string formattedDateTime = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");


        }
    }
}