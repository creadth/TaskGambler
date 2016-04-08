using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.Data.Models;

namespace InoGambling.Core.Services.Users.Models
{
    public class UserCreateResult
    {
        public UserCreateState State { get; set; }
        public User User { get; set; }
    }

    public enum UserCreateState : byte
    {
        Ok = 0,
        LoginExists = 1,
    }
}
