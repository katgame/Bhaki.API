using Dice.API.Data.Models;
using Dice.API.Data.ViewModels.Authentication;

namespace Dice.API.Data.Dto
{
    public class LoginRespose
    {
        public AuthResultVM token { get; set; }
        public UserInfo userDetails { get; set; }

        public AccountInfo userAccount { get; set; }
    }
}
