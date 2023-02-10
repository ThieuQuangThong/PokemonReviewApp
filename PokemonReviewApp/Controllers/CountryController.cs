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

    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        // GET Countries
        [HttpGet]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _countryRepository.GetCountries();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something went wrong getting data");
                return StatusCode(500, ex.Message);
            }

        }

        // GET Country/id
        [HttpGet("{countryId}")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(Country))]
        public async Task<IActionResult> GetCountry(int countryId)
        {
            try
            {
                var countryFound = await _countryRepository.GetCountry(countryId);
                return Ok(countryFound);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something wrong when getting data");
                return StatusCode(500, ex.Message);
            }
        }

        // POST Country
        [HttpPost]
        [Authorize(Roles = "Admins")]

        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostCountry([FromBody] CountryDto countryCreate)
        {
            if (countryCreate == null)
                return BadRequest(ModelState);


            if (_countryRepository.CountryExists(countryCreate.Name))
            {
                ModelState.AddModelError("", "Country already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }

            try
            {
                await _countryRepository.CreateCountry(countryCreate);
                return Ok(countryCreate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ModelState);
            }

        }

        // PUT Country
        [HttpPut("{countryId}")]
        [Authorize(Roles = "Admins")]

        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdateCountry(int countryId, [FromBody] CountryDto countryUpdate)
        {
            if (countryUpdate == null)
                return BadRequest(ModelState);

            if (countryId != countryUpdate.Id)
                return BadRequest(ModelState);

            if (!_countryRepository.CountryExist(countryId))
                return BadRequest(ModelState);

            try
            {
                await _countryRepository.UpdateCountry(countryUpdate);
                return Ok(countryUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ModelState);
            }

        }
        // DELETE countryId
        [HttpDelete("{countryId}")]
        [Authorize(Roles = "Admins")]

        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteCountry(int countryId)
        {
            if (!_countryRepository.CountryExist(countryId))
                return NotFound();

            var countryToDelete = await _countryRepository.GetCountry(countryId);

            try
            {
                _countryRepository.DeleteCountry(countryToDelete);
                return Ok(countryToDelete);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ModelState);
            }

        }
    }
}
