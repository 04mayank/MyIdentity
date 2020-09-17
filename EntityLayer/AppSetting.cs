using System;
namespace MyIdentity.EntityLayer
{
    public class AppSetting
    {
        // Properties for Jwt Signature
        public string Site { get; set; }
        public string Audience { get; set; }
        public string ExpireTime { get; set; }
        public string Secret { get; set; }

        // Token Refresh Properties added 
        public string RefreshToken { get; set; }
        public string GrantType { get; set; }
        public string ClientId { get; set; }

        // Sendgrind
        public string SendGridKey { get; set; }
        public string SendGridUser { get; set; }
    }
}
