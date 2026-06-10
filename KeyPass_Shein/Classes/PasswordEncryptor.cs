using System.Security.Cryptography;
using System.Text;

namespace KeyPass_Shein.Classes
{
    public class PasswordEncryptor
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("KEYPASS_ENCRYPT_KEY_32BYTES!!!!!");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("KEYPASS_IV_16BYT");

        public static string Encrypt(string password)
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string encryptedPassword)
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
