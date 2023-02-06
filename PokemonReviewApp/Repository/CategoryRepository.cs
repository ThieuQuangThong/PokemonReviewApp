using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CategoryExists(int categoryId)
        {
            return _context.Categories.Any(c => c.Id == categoryId);
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }


        public async Task<List<CategoryDto>> GetCategories()
        {
            return await _context.Categories.OrderBy(c => c.Id).Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync();
        }

        public async Task<CategoryDto> GetCategory(int id)
        {
            return await _context.Categories.Where(c => c.Id == id).Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).FirstOrDefaultAsync();
        }

        //public Category GetCategory(string name)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<List<PokemonDto>> GetPokemonByCategory(int categoryId)
        {
            return await _context.PokemonCategories.Where(c => c.CategoryId == categoryId).Select(c => c.Pokemon).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }
    }
}
