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
        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        Task<bool> DeletePokemon(PokemonDto pokemon);
        bool Save();
    }
}
