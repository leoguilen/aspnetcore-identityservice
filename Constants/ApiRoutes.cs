namespace AuthenticationClientService.Constants
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
            public const string ConfirmEmail = Base + "/identity/confirmEmail";
        }
        public static class Profile
        {
            public const string Get = Base + "/profile/{id}";
            public const string Update = Base + "/profile/{id}";
        }
    }
}
