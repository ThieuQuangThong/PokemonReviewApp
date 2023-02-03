using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int ownerId);
        ICollection<Pokemon> GetPokemonsByOwner(int ownerId);
        bool OswnerExists(int ownerId);
        bool CreateOwner (Owner owner);
        bool Save();
    }
}
