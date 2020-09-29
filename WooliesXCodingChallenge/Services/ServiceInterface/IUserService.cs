using System.Threading.Tasks;
using WooliesXCodingChallenge.Models;

namespace WooliesXCodingChallenge.Services
{
    public interface IUserService 
    {
        Task<User> GetUser();
    }
}
