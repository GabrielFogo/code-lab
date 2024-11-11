namespace CodeLab.Services
{
    public class AdminService : IAdminService
    {
        private readonly IConfiguration _configuration;

        public AdminService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetAdminEmail()
        {
            var email = _configuration.GetValue<string>("Admin:Email");
            return !string.IsNullOrEmpty(email) ? email : throw new InvalidOperationException("Admin email is required in configuration.");
        }

        public string GetAdminPassword()
        {
            var password = _configuration.GetValue<string>("Admin:Password");
            return !string.IsNullOrEmpty(password) ? password : throw new InvalidOperationException("Admin password is required in configuration.");
        }
    }
}
