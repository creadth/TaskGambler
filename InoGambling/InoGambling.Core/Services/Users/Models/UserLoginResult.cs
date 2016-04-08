using InoGambling.Data.Models;

namespace InoGambling.Core.Services.Users.Models
{
    public class UserLoginResult
    {
        public UserLoginState State { get; set; }
        public User User { get; set; }
    }

    public enum UserLoginState : byte
    {
        Ok = 0,
        LoginNotExists = 1,
        IncorrectPassword = 2,
    }
}
