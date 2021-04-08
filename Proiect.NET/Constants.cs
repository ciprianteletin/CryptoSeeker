using System.Configuration;
using System.Linq;

namespace Proiect.NET
{
    class Constants
    {
        public static readonly string connString = ConfigurationManager.ConnectionStrings["Proiect.NET.Properties.Settings.CryptoDBConnectionString"].ConnectionString;

    }
}
