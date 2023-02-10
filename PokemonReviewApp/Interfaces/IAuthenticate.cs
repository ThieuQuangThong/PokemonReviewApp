using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IAuthenticate
    {
        Task<User> Authenticate(UserLogin userLogin);
        Task<string> Generate(User user);
    }
}
