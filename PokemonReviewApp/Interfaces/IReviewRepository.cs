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
        Task<bool> CreateReview(int reviewerId, int pokeId, ReviewDto review);
        Task<bool> UpdateReview(ReviewDto review);
        Task<bool> DeleteReview(ReviewDto review);
        Task<bool> DeleteReviews(List<Review> reviews);

        Task<bool> Save();
    }
}
