using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.GetTask3.Sdk
{
    public class TokenResponse
    {
        public string kind { get; set; }
        public string idToken { get; set; }
        public string refreshToken { get; set; }
        public string exiresIn { get; set; }

        public bool isNewUser { get; set; }
    }
}
