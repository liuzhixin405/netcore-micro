using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.DistributedId
{
    public static class GuidHelper
    {
        public static Guid NewGuid(SequentialGuidType guidType = SequentialGuidType.AtEnd)
        {
            
            byte[] randomBytes = RandomNumberGenerator.GetBytes(10);
            long timestamp = DateTime.UtcNow.Ticks / 10000L;
            byte[] timestampBytes = BitConverter.GetBytes(timestamp);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }
            byte[] guidBytes = new byte[16];
            switch (guidType)
            {
                case SequentialGuidType.AtBegin:
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);
                    if(guidType ==SequentialGuidType.AtBegin&&BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(guidBytes, 0, 4);
                        Array.Reverse(guidBytes, 4, 2);
                    }
                    break;
                case SequentialGuidType.AtEnd:
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                    break;
            }
            return new Guid(guidBytes);
        }
    }
    /// <summary>
    /// 序列排序
    /// </summary>
    public enum SequentialGuidType
    {
        /// <summary>
        /// 首部排序,适用于非sqlserver
        /// </summary>
        AtBegin,
        /// <summary>
        /// 尾部排序适用于sqlserver
        /// </summary>
        AtEnd
    }
}
