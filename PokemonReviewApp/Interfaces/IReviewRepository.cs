using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<ReviewDto>> GetReviews();
        Task<ReviewDto> GetReview(int id);
        Task<List<ReviewDto>> GetReviewOfAPokemon(int pokeId);
        bool ReviewExist(int id);
        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool DeleteReviews(List<Review> reviews);

        bool Save();
    }
}
