using System.Text;


namespace Proiect.NET
{
    class Utils
    {
        public static string EncryptPassword(string Password)
        {
            byte[] data = Encoding.ASCII.GetBytes(Password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            return Encoding.ASCII.GetString(data);
        }
    }
}
