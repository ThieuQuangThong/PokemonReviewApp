using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IPokemonRepository _pokemonRepository;
        public ReviewRepository(DataContext context, IMapper mapper, IReviewerRepository reviewerRepository, IPokemonRepository pokemonRepository)
        {
            _mapper = mapper;
            _context = context;
            _reviewerRepository = reviewerRepository;
            _pokemonRepository = pokemonRepository;
        }

        public async Task<bool> CreateReview(int reviewerId, int pokeId, ReviewDto review)
        {
            var reviewToCreate = _mapper.Map<Review>(review);
            reviewToCreate.Reviewer = await _context.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefaultAsync();
            reviewToCreate.Pokemon = await _context.Pokemon.Where(p => p.Id == pokeId).FirstOrDefaultAsync();
            await _context.Reviews.AddAsync(reviewToCreate);
            return await Save();
        }

        public async Task<bool> DeleteReview(ReviewDto review)
        {
            _context.Remove(_mapper.Map<Review>(review));
            return await Save();
        }

        public async Task<bool> DeleteReviews(List<Review> reviews)
        {
            _context.RemoveRange(reviews);
            return await Save();
        }

        public async Task<ReviewDto> GetReview(int id)
        {
            return await _context.Reviews.Where(r => r.Id == id).AsNoTracking().Select(r => new ReviewDto
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
            return await _context.Reviews.OrderBy(r => r.Id).AsNoTracking().Select(r => new ReviewDto
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

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateReview(ReviewDto review)
        {
            _context.Update(_mapper.Map<Review>(review));
            return await Save();
        }
    }
}
