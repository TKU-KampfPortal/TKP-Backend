namespace TKP.Server.Infrastructure.ConfigSetting
{
    public class JwtConfigSetting
    {
        public string SecretKey { get; set; } = string.Empty;
        public int TokenValidityInHours { get; set; } = 0;
        public int RefreshTokenValidityInDays { get; set; } = 0;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;

        public static readonly string LoginHistoryIdClaimType = "LoginId";

        public static readonly string JwtTokenId = "JwtTokenId";

    }
}
