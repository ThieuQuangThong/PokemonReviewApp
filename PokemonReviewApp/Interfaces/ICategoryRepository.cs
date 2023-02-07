using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<CategoryDto>> GetCategories();
        Task<CategoryDto> GetCategory(int id);
        Task<List<PokemonDto>> GetPokemonByCategory(int categoryId);
        bool CategoryExists(int id);
        bool CategoriesExists(string name);
        Task<bool> CreateCategory(CategoryDto category);
        Task<bool> UpdateCategory(CategoryDto category);
        Task<bool> DeleteCategory(CategoryDto category);
        Task<bool> Save();
    }
}
