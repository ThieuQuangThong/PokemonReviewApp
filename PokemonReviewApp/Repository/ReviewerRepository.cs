using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ReviewerRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<bool> CreateReviewer(ReviewerDto reviewer)
        {
            await _context.AddAsync(_mapper.Map<Reviewer>(reviewer));
            return await Save();
        }

        public async Task<bool> DeleteReviewer(ReviewerDto reviewer)
        {
            _context.Remove(_mapper.Map<Reviewer>(reviewer));
            return await Save();
        }

        public async Task<ReviewerDto> GetReviewer(int reviewerId)
        {
            return await _context.Reviewers.Where(c => c.Id == reviewerId).AsNoTracking().Select(r => new ReviewerDto
            {
                Id = r.Id,
                FirstName = r.FirstName,
                LastName = r.LastName,
            }).FirstOrDefaultAsync();
        }

        public async Task<List<ReviewerDto>> GetReviewers()
        {
            return await _context.Reviewers.AsNoTracking().Select(r => new ReviewerDto
            {
                Id = r.Id,
                FirstName = r.FirstName,
                LastName = r.LastName,
            }).ToListAsync();
        }

        public ICollection<Review> GetReviewOfReviewer(int reviewerId)
        {
            return _context.Reviews.Where(c => c.Reviewer.Id == reviewerId).ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _context.Reviewers.Any(c => c.Id == reviewerId);
        }

        public bool ReviewerExists(string name)
        {
            return _context.Reviewers.Any(c => c.LastName == name);

        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateReviewer(ReviewerDto reviewer)
        {
            _context.Update(_mapper.Map<Reviewer>(reviewer));
            return await Save();
        }
    }
}
