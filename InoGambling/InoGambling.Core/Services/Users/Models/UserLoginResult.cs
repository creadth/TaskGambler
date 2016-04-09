using System;

namespace InoGambling.Core.Services.Users.Models
{
    public class UserLoginResult
    {
        public UserLoginState State { get; set; }
        public Int64 UserId { get; set; }
    }

    public enum UserLoginState : byte
    {
        Ok = 0,
        LoginNotExists = 1,
        IncorrectPassword = 2,

        Error = 100,
    }
}
