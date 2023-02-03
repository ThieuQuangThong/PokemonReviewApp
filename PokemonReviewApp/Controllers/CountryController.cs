using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CountryController :Controller
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries() 
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(countries);   
        }

        // GET Country/id
        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        public IActionResult GetCountry(int countryId)
        {
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(country);
        }

        // POST Country
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult PostCountry([FromBody]CountryDto countryCreate)
        {
            if(countryCreate == null)
                return BadRequest(ModelState);

            var country = _countryRepository.GetCountries().Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper());

            if(country != null)
            {
                ModelState.AddModelError("", "Country already exist");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
            { return BadRequest(ModelState); }

            var countryMap = _mapper.Map<Country>(countryCreate);

            if (!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong went saving !!!");
                return StatusCode(500, ModelState);
            }
            return Ok("Succesfully Created!!!");
        }


    }
}
