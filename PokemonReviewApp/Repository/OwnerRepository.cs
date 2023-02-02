using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;
        public OwnerRepository(DataContext context) 
        {
            _context= context;
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners.Where(c => c.Id == ownerId).FirstOrDefault();

        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();

        }

        public ICollection<Pokemon> GetPokemonsByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(c =>c.OwnerId == ownerId).Select(c => c.Pokemon).ToList();
        }

        public bool OswnerExists(int ownerId)
        {
            return _context.Owners.Any(c => c.Id == ownerId);
        }
    }
}
