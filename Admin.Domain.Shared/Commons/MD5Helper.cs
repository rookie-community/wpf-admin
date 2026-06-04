using System.Security.Cryptography;
using System.Text;

namespace Admin.Commons
{
    public class MD5Helper
    {
        public static string GetMD5(string cl)
        {
            var bytes = MD5.HashData(Encoding.UTF8.GetBytes(cl));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
