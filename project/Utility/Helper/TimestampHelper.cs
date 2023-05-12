namespace project.Utility.Helper
{
    public static class TimestampHelper
    {
        private static readonly long _minTimestamp;

        private static readonly long _maxTimestamp;

        //
        // 摘要:
        //     转为Unix时间戳
        //
        // 参数:
        //   dateTime:
        public static long ToUnixTimeMilliseconds(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Local);
            }

            return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
        }

        //
        // 摘要:
        //     Unix时间戳转为DateTimeOffset
        //
        // 参数:
        //   timestamp:
        public static DateTimeOffset FromUnixTimeMilliseconds(long timestamp)
        {
            if (timestamp < _minTimestamp)
            {
                timestamp = _minTimestamp;
            }
            else if (timestamp > _maxTimestamp)
            {
                timestamp = _maxTimestamp;
            }

            return DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
        }

        static TimestampHelper()
        {
            DateTimeOffset minValue = DateTimeOffset.MinValue;
            _minTimestamp = minValue.ToUnixTimeMilliseconds();
            minValue = DateTimeOffset.MaxValue;
            _maxTimestamp = minValue.ToUnixTimeMilliseconds();
        }
    }
}
