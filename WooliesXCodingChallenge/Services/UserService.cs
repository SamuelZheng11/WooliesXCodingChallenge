using System.Threading.Tasks;
using WooliesXCodingChallenge.Models;

namespace WooliesXCodingChallenge.Services
{
    public class UserService : IUserService
    {
        public Task<User> GetUser() {
            return Task.FromResult<User>(new User()
            {
                Name = "Samuel Zheng",
                Token = "1234-455662-22233333-3333"
            });
        }
    }
}
