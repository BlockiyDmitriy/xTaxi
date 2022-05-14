namespace xTaxi.Client.Models
{
    public class UserDataModel
    {
        public LoginDataModel LoginDataModel { get; set; } = new LoginDataModel();
        public CardDataModel CardDataModel { get; set; } = new CardDataModel();
    }

    public class LoginDataModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }

    public class CardDataModel
    {
        public string CardNumber { get; set; } = string.Empty;
        public string ExpireCardDate { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
    }
}
