using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        Task<List<CountryDto>> GetCountries();
        Task<CountryDto> GetCountry(int countryId);
        bool CountryExist(int countryId);
        bool CountryExists(string name);
        Task<bool> CreateCountry(CountryDto country);
        Task<bool> UpdateCountry(CountryDto country);
        Task<bool> DeleteCountry(CountryDto country);
        bool Save();
    }
}
