namespace authenticationservice
{
    public class AppSettings
    {
        public static AppSettings Settings {get; set;}
        public string JwtSecret {get; set;}
        public string GoogleClientId  {get; set;}
        public string GoogleClientSecret  {get; set;}
        public string JwtEmailEncryption {get; set;}
    }
}