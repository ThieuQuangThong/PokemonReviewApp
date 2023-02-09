using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public CountryRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool CountryExist(int countryId)
        {
            return _context.Countries.Any(c => c.Id == countryId);
        }

        public bool CountryExists(string name)
        {
            return _context.Countries.Any(c => c.Name == name);
        }

        public async Task<bool> CreateCountry(CountryDto country)
        {
            await _context.AddAsync(_mapper.Map<Country>(country));
            await _context.SaveChangesAsync();
            return await Save();
        }

        public async Task<bool> DeleteCountry(CountryDto country)
        {
            _context.Remove(_mapper.Map<Country>(country));
            return await Save();
        }

        public async Task<List<CountryDto>> GetCountries()
        {
            return await _context.Countries.OrderBy(c => c.Id).Select(c => new CountryDto
            {
                Id = c.Id,
                Name = c.Name,
            }).ToListAsync();
        }

        public async Task<CountryDto> GetCountry(int countryId)
        {
            return await _context.Countries.Where(c => c.Id == countryId).Select(c => new CountryDto
            {
                Id = c.Id,
                Name = c.Name,
            }).FirstOrDefaultAsync();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateCountry(CountryDto country)
        {
            _context.Update(_mapper.Map<Country>(country));
            return await Save();
        }
    }
}
