using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using AutoMapper;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public CategoryRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public bool CategoryExists(int categoryId)
        {
            return _context.Categories.Any(c => c.Id == categoryId);
        }

        public async Task<bool> CreateCategory(CategoryDto category)
        {
            await _context.AddAsync(_mapper.Map<Category>(category));
            await _context.SaveChangesAsync();
            return await Save();
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


        public async Task<List<PokemonDto>> GetPokemonByCategory(int categoryId)
        {
            return await _context.PokemonCategories.Where(c => c.CategoryId == categoryId).Select(p => new PokemonDto
            {
                Name = p.Pokemon.Name,
                BirthDate = p.Pokemon.BirthDate
            }).ToListAsync();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();

            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateCategory(CategoryDto category)
        {
            _context.Update(_mapper.Map<Category>(category));
            await _context.SaveChangesAsync();
            return await Save();
        }

        public async Task<bool> DeleteCategory(CategoryDto category)
        {
            _context.Remove(_mapper.Map<Category>(category));
            await _context.SaveChangesAsync();
            return await Save();
        }

        public bool CategoriesExists(string name)
        {
            return _context.Categories.Any(c => c.Name == name);
        }
    }
}
