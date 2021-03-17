using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL9035.Models
{
    public class CredentialAsset
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SecretKey { get; set; }
        public string Email { get; set; }
    }
}