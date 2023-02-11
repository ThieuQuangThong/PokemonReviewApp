using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;

        public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        // GET Owners
        /// <summary>
        /// Get all owners.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public async Task<IActionResult> GetOwners()
        {
            try
            {
                var owners = await _ownerRepository.GetOwners();
                return Ok(owners);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET Owners/id
        /// <summary>
        /// Get owner by Id.
        /// </summary>
        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(OwnerDto))]
        public async Task<IActionResult> GetOwner(int ownerId)
        {
            try
            {
                var owner = await _ownerRepository.GetOwner(ownerId);
                return Ok(owner);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        // GET Owners/pokemon/ownerId
        /// <summary>
        /// Get pokemon of owner by ownerId.
        /// </summary>
        [HttpGet("pokemon/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public async Task<IActionResult> GetPokemonsByOwner(int ownerId)
        {
            try
            {
                var pokemonOfOwner = await _ownerRepository.GetPokemonsByOwner(ownerId);
                return Ok(pokemonOfOwner);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST Owner
        /// <summary>
        /// Add Owner.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admins")]

        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto ownerCreate)
        {
            if (ownerCreate == null)
                return BadRequest(ModelState);

            var owner = _ownerRepository.OwnerExists(ownerCreate.LastName);

            if (owner)
            {
                ModelState.AddModelError("", "Owner already Exist !!!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var ownerToCreate = await _ownerRepository.CreateOwner(countryId, ownerCreate);
                return Ok(ownerToCreate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT owner
        /// <summary>
        /// Update Owner by ownerId.
        /// </summary>
        [HttpPut("{ownerId}")]
        [Authorize(Roles = "Admins")]

        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdateOwner(int ownerId, [FromBody] OwnerDto ownerUpdate)
        {
            if (ownerUpdate == null)
                return BadRequest(ModelState);

            if (ownerId != ownerUpdate.Id)
                return BadRequest(ModelState);

            if (!_ownerRepository.OwnerExists(ownerId))
                return NotFound();

            try
            {
                await _ownerRepository.UpdateOwner(ownerUpdate);
                return Ok(ownerUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE owner
        /// <summary>
        /// Delete owner by ownerId.
        /// </summary>
        [HttpDelete("{ownerId}")]
        [Authorize(Roles = "Admins")]

        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteOwner(int ownerId)
        {
            if (!_countryRepository.CountryExist(ownerId))
                return BadRequest(ModelState);

            var ownerToDelete = await _ownerRepository.GetOwner(ownerId);

            try
            {
                await _ownerRepository.DeleteOwner(ownerToDelete);
                return Ok(ownerToDelete);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
