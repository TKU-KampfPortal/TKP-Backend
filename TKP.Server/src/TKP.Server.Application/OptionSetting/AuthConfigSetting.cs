namespace TKP.Server.Application.OptionSetting
{
    public sealed class AuthConfigSetting
    {
        public readonly int MaxFailedLoginAttempts;
        public AuthConfigSetting(int maxFailedLoginAttempts)
        {
            MaxFailedLoginAttempts = maxFailedLoginAttempts;
        }
    }
}
