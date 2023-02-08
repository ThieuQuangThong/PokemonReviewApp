using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository
    {
        Task<List<PokemonDto>> GetPokemons();
        Task<PokemonDto> GetPokemon(int id);
        Pokemon GetPokemon(string name);
        Task<decimal> GetPokemonRating(int pokeId);
        bool PokemonExist(int pokeId);
        bool PokemonExist(string pokeName);
        Task<bool> CreatePokemon(int ownerId, int categoryId, PokemonDto pokemon);
        Task<bool> UpdatePokemon(int ownerId, int categoryId, PokemonDto pokemon);
        Task<bool> DeletePokemon(PokemonDto pokemon);
        bool Save();
    }
}
