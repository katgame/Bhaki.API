using Bhaki.API.Data.ViewModels.Authentication;

namespace Bhaki.API.Data.Dto
{
    public class LoginRespose
    {
        public AuthResultVM token { get; set; }
        public UserInfo userDetails { get; set; }
    }
}
