using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using E_Handel;

namespace E_Handel
{
    public class EncryptionHelper
    {

        private const byte Key = 0x42; // 66 bytes

        public static string Encrypt(string text)
        {

            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(text);

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ Key);
            }

            return Convert.ToBase64String(bytes);
        }
        public static string Decrypt(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            try
            {
                var bytes = Convert.FromBase64String(text);

                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = (byte)(bytes[i] ^ Key);
                }

                return Encoding.UTF8.GetString(bytes);
            }
            catch (FormatException)
            {
                // Texten är inte Base64 → alltså inte krypterad.
                // Returnera originalvärdet så programmet inte kraschar.
                return text;
            }
        }
    }
}
