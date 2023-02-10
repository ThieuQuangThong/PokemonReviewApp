using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }


        // GET Categories
        /// <summary>
        /// Get all categories.
        /// </summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public async Task<IActionResult> GetCategories()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categories = await _categoryRepository.GetCategories();
            return Ok(categories);
        }

        // GET Category/id
        /// <summary>
        /// Get category by Id.
        /// </summary>
        [HttpGet("{categoryId}")]
        [Authorize]

        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            var category = await _categoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(category);
        }
        /// <summary>
        /// Get Pokemon of Category.
        /// </summary>
        // GET Category/pokemon/categoryd
        [HttpGet("pokemon/{categoryId}")]
        [Authorize]

        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonByCategory(int categoryId)
        {
            var pokemons = await _categoryRepository.GetPokemonByCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        // POST Category
        /// <summary>
        /// Add category.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admins")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryCreate)
        {
            if (categoryCreate == null)
                return BadRequest(ModelState);

            if (_categoryRepository.CategoriesExists(categoryCreate.Name))
            {
                ModelState.AddModelError("", "Category already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _categoryRepository.CreateCategory(categoryCreate);
                return Ok(categoryCreate);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Something was wrong went saving !!!");
                return StatusCode(500, ModelState);
            }

        }

        // PUT category
        /// <summary>
        /// Update category information.
        /// </summary>
        [HttpPut("{categoryId}")]
        [Authorize(Roles = "Admins")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(200)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto categoryUpdate)
        {
            if (categoryUpdate == null)
                return BadRequest(ModelState);

            if (categoryId != categoryUpdate.Id)
                return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                _categoryRepository.UpdateCategory(categoryUpdate);
                return Ok(categoryUpdate);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Something was wrong went updating !!!");
                return StatusCode(500, ModelState);
            }

        }

        // DELETE categoryId
        /// <summary>
        /// Delete category by Id.
        /// </summary>
        [HttpDelete("{categoryId}")]
        [Authorize(Roles = "Admins")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var categoryToDelete = await _categoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _categoryRepository.DeleteCategory(categoryToDelete);
                return Ok(categoryToDelete);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Something went wrong deleting");
                return StatusCode(500, ModelState);
            }
        }

    }
}
