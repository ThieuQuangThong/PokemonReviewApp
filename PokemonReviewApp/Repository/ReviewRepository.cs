using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository

    {
        private readonly DataContext _context;
        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _context.Remove(review);
            return Save();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            _context.RemoveRange(reviews);
            return Save();
        }

        public async Task<ReviewDto> GetReview(int id)
        {
            return await _context.Reviews.Where(r => r.Id == id).Select(r => new ReviewDto
            {
                Id = r.Id,
                Title = r.Title,
                Rating = r.Rating,
                Text = r.Text,
            }).FirstOrDefaultAsync();
        }

        public async Task<List<ReviewDto>> GetReviewOfAPokemon(int pokeId)
        {
            return await _context.Reviews.Where(r => r.Pokemon.Id == pokeId).Select(p => new ReviewDto
            {
                Id = p.Id,
                Title = p.Title,
                Rating = p.Rating,
                Text = p.Text,
            }).ToListAsync();
        }

        public async Task<List<ReviewDto>> GetReviews()
        {
            return await _context.Reviews.OrderBy(r => r.Id).Select(r => new ReviewDto
            {
                Id = r.Id,
                Title = r.Title,
                Rating = r.Rating,
                Text = r.Text,
            }).ToListAsync();
        }

        public bool ReviewExist(int id)
        {
            return _context.Reviews.Any(r => r.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            _context.Update(review);
            return Save();
        }
    }
}
