using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutProj.Auth
{
    public static class AuthOptions
    {
        public const string ISSUER = "HelpingHandAPI";
        public const string AUDIENCE = "Client";
        const string KEY = "InitalSecurityKey";
        public const int LIFETIME = 3000;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
