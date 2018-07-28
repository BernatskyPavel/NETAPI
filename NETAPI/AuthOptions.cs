using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETAPI
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "http://localhost/"; // потребитель токена
        const string KEY = "}08$TCdA5@546tQp*Yp~HQVJCz1CuMScWznWPYDlqZb1EeKpAGz@Kn@9LBL*VLs398*SBrRd";   // ключ для шифрации
        public const int LIFETIME = 60*24*7; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
