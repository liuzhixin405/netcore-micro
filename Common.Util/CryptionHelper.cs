using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Utilities.Encoders;

namespace Common.Util
{
    public class CryptionHelper
    {
        public static string CalculateSHA3Hash(string input)
        {
            string data = input+"pwd";
            // 使用SHA-3-256算法
            IDigest digest = new Sha3Digest(256);

            // 将字符串转换为字节数组
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // 更新摘要
            digest.BlockUpdate(inputBytes, 0, inputBytes.Length);

            // 计算摘要值
            byte[] hashBytes = new byte[digest.GetDigestSize()];
            digest.DoFinal(hashBytes, 0);

            // 将字节数组转换为十六进制字符串
            return Hex.ToHexString(hashBytes);
        }
    }
}
