using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public OwnerRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateOwner(int CountryId, OwnerDto owner)
        {
            var ownerToCreate = _mapper.Map<Owner>(owner);
            ownerToCreate.Country = await _context.Countries.Where(c => c.Id == CountryId).FirstOrDefaultAsync();
            await _context.Owners.AddAsync(ownerToCreate);
            return await Save();
        }

        public async Task<bool> DeleteOwner(OwnerDto owner)
        {
            _context.Remove(_mapper.Map<Owner>(owner));
            return await Save();
        }

        public async Task<OwnerDto> GetOwner(int ownerId)
        {
            return await _context.Owners.Where(c => c.Id == ownerId).Select(o => new OwnerDto
            {
                Id = o.Id,
                FirstName = o.FirstName,
                LastName = o.LastName,
                Gym = o.Gym
            }).FirstOrDefaultAsync();

        }

        public async Task<List<OwnerDto>> GetOwners()
        {
            return await _context.Owners.Select(o => new OwnerDto
            {
                Id = o.Id,
                FirstName = o.FirstName,
                LastName = o.LastName,
                Gym = o.Gym
            }).ToListAsync();

        }

        public async Task<List<PokemonDto>> GetPokemonsByOwner(int ownerId)
        {
            return await _context.PokemonOwners.Where(c => c.OwnerId == ownerId).Select(c => new PokemonDto
            {
                Id = c.Pokemon.Id,
                Name = c.Pokemon.Name,
                BirthDate = c.Pokemon.BirthDate,
            }).ToListAsync();
        }

        public bool OwnerExists(int ownerId)
        {
            return _context.Owners.Any(c => c.Id == ownerId);
        }

        public bool OwnerExists(string ownerName)
        {
            return _context.Owners.Any(c => c.LastName == ownerName);
        }

        public async Task<bool> Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateOwner(OwnerDto owner)
        {
            _context.Update(_mapper.Map<Owner>(owner));
            return await Save();
        }
    }
}
