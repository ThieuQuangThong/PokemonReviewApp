using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IReviewRepository reviewRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        // GET Pokemons
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDto>))]
        public async Task<IActionResult> GetPokemons()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pokemons = await _pokemonRepository.GetPokemons();
            return Ok(pokemons);

        }

        // GET Pokemons/pokeId
        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(PokemonDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExist(pokeId))
                return NotFound();

            var pokemon = await _pokemonRepository.GetPokemon(pokeId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemon);
        }

        // GET Poke
        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.PokemonExist(pokeId))
            {
                return NotFound();
            }

            var rating = await _pokemonRepository.GetPokemonRating(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(rating);
        }

        // POST poke
        //[HttpPost]
        //[ProducesResponseType(204)]
        //[ProducesResponseType(400)]
        //public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int catId, [FromBody] PokemonDto pokemonCreate)
        //{
        //    if (pokemonCreate == null)
        //        return BadRequest(ModelState);

        //    var pokemon = _pokemonRepository.GetPokemons().Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

        //    if (pokemon != null)
        //    {
        //        ModelState.AddModelError("", "Pokemon already exist");
        //        return StatusCode(422, ModelState);
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);
        //    if (!_pokemonRepository.CreatePokemon(ownerId, catId, pokemonMap))
        //    {
        //        ModelState.AddModelError("", "Something was wrong went saving !!!");
        //        return StatusCode(500, ModelState);
        //    }
        //    return Ok("Successfully Created!!!");
        //}

        // PUT poke
        [HttpPut("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdatePokemon(int pokeId, [FromQuery] int ownerId, [FromQuery] int catId, [FromBody] PokemonDto updatePokemon)
        {
            if (updatePokemon == null)
                return BadRequest(ModelState);

            if (pokeId != updatePokemon.Id)
                return BadRequest(ModelState);

            if (!_pokemonRepository.PokemonExist(pokeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                await _pokemonRepository.UpdatePokemon(ownerId, catId, updatePokemon);
                return Ok(updatePokemon);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        // DELETE Pokemon
        [HttpDelete("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExist(pokeId))
            {
                return NotFound();
            }

            //var reviewsToDelete = await _reviewRepository.GetReviewOfAPokemon(pokeId);
            var pokemonToDelete = await _pokemonRepository.GetPokemon(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            //{
            //    ModelState.AddModelError("", "Something went wrong when deleting reviews");
            //}

            try
            {
                await _pokemonRepository.DeletePokemon(pokemonToDelete);
                return Ok(pokemonToDelete);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

    }
}
