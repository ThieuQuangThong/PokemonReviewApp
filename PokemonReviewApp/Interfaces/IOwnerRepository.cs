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
        bool OwnerExists(string ownerName);
        Task<bool> CreateOwner(int countryId, OwnerDto owner);
        Task<bool> UpdateOwner(OwnerDto owner);
        Task<bool> DeleteOwner(OwnerDto owner);
        Task<bool> Save();
    }
}
