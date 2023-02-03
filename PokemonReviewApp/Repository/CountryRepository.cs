using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {

        private readonly DataContext _context;
        public CountryRepository(DataContext context)
        {
            _context = context;
        }

        public bool CountryExist(int countryId)
        {
            return _context.Countries.Any(c => c.Id == countryId);
        }

        public bool CreateCountry(Country country)
        {
            _context.Add(country);
            return Save();
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.OrderBy(c=>c.Id ).ToList();
        }

        public Country GetCountry(int countryId)
        {
            return _context.Countries.Where(c => c.Id == countryId).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
