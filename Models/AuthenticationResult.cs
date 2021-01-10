namespace AuthenticationClientService.API.Models
{
    public class AuthenticationResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
        public string[] Errors { get; set; }
        public UserModel User { get; set; }
    }
}
