using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        Task<List<OwnerDto>> GetOwners();
        Task<OwnerDto> GetOwner(int ownerId);
        Task<List<PokemonDto>> GetPokemonsByOwner(int ownerId);
        bool OwnerExists(int ownerId);
        Task<bool> OwnerExists(string ownerName);
        Task<bool> CreateOwner(int countryId, OwnerDto owner);
        bool UpdateOwner(Owner owner);
        bool DeleteOwner(Owner owner);
        bool Save();
    }
}
