using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        Task<List<ReviewerDto>> GetReviewers();
        Task<ReviewerDto> GetReviewer(int reviewerId);
        ICollection<Review> GetReviewOfReviewer(int reviewerId);
        bool ReviewerExists(int reviewerId);
        bool ReviewerExists(string name);
        Task<bool> CreateReviewer(ReviewerDto reviewer);
        Task<bool> UpdateReviewer(ReviewerDto reviewer);
        Task<bool> DeleteReviewer(ReviewerDto reviewer);
        bool Save();
    }
}
