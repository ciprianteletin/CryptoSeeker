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

        public static string formatTimer(int data)
        {
            if(data <= 9)
            {
                return "0" + data;
            }
            return data + "";
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
