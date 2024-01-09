using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public static string Encrypt(string plainText, string key, string nonce)
        {
            using (AesGcm aesGcm = new AesGcm(Encoding.UTF8.GetBytes(key)))
            {
                byte[] plaintext = Encoding.UTF8.GetBytes(plainText);
                byte[] ciphertext = new byte[plaintext.Length];
                byte[] tag = new byte[16]; // GCM authentication tag is 16 bytes

                aesGcm.Encrypt(Encoding.UTF8.GetBytes(nonce), plaintext, ciphertext, tag);

                // Combine nonce, ciphertext, and tag into a single byte array
                byte[] encryptedBytes = new byte[nonce.Length + ciphertext.Length + tag.Length];
                Buffer.BlockCopy(Encoding.UTF8.GetBytes(nonce), 0, encryptedBytes, 0, nonce.Length);
                Buffer.BlockCopy(ciphertext, 0, encryptedBytes, nonce.Length, ciphertext.Length);
                Buffer.BlockCopy(tag, 0, encryptedBytes, nonce.Length + ciphertext.Length, tag.Length);

                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public static string Decrypt(string encryptedString, string key, string nonce)
        {
            var encryptedBytes=Convert.FromBase64String(encryptedString);
            using (AesGcm aesGcm = new AesGcm(Encoding.UTF8.GetBytes(key)))
            {
                byte[] nonceBytes = new byte[12]; // GCM nonce is 12 bytes
                Buffer.BlockCopy(encryptedBytes, 0, nonceBytes, 0, nonceBytes.Length);

                byte[] ciphertext = new byte[encryptedBytes.Length - nonceBytes.Length - 16]; // 16 bytes for GCM tag
                Buffer.BlockCopy(encryptedBytes, nonceBytes.Length, ciphertext, 0, ciphertext.Length);

                byte[] tag = new byte[16];
                Buffer.BlockCopy(encryptedBytes, nonceBytes.Length + ciphertext.Length, tag, 0, tag.Length);

                byte[] decryptedBytes = new byte[ciphertext.Length];
                aesGcm.Decrypt(nonceBytes, ciphertext, tag, decryptedBytes);

                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
