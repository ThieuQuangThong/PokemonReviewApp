using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public PokemonRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreatePokemon(int ownerId, int categoryId, PokemonDto pokemon)
        {
            var pokemonOwnerEntity = await _context.Owners.Where(o => o.Id == ownerId).FirstOrDefaultAsync();
            var pokemonCategoryEntity = await _context.Categories.Where(c => c.Id == categoryId).FirstOrDefaultAsync();
            var pokemonMap = _mapper.Map<Pokemon>(pokemon);

            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemonMap
            };

            await _context.AddAsync(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = pokemonCategoryEntity,
                Pokemon = pokemonMap
            };

            await _context.AddAsync(pokemonCategory);

            await _context.AddAsync(pokemonMap);
            return Save();
        }

        public async Task<bool> DeletePokemon(PokemonDto pokemon)
        {
            _context.Remove(_mapper.Map<Pokemon>(pokemon));
            return Save();
        }

        public async Task<PokemonDto> GetPokemon(int id)
        {
            return await _context.Pokemon.Where(p => p.Id == id).Select(p => new PokemonDto
            {
                Id = p.Id,
                Name = p.Name,
                BirthDate = p.BirthDate
            }).FirstOrDefaultAsync();
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemon.Where(p => p.Name == name).FirstOrDefault();
        }

        public async Task<decimal> GetPokemonRating(int pokeId)
        {
            var review = await _context.Reviews.Where(p => p.Pokemon.Id == pokeId).Select(r => new Review
            {
                Id = r.Id,
                Rating = r.Rating,
            }).ToListAsync();

            if (review.Count() <= 0)
            {
                return 0;
            }
            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        public async Task<List<PokemonDto>> GetPokemons()
        {
            return await _context.Pokemon.OrderBy(p => p.Id).Select(p => new PokemonDto
            {
                Id = p.Id,
                Name = p.Name,
                BirthDate = p.BirthDate,
            }).ToListAsync();
        }

        public bool PokemonExist(int pokeId)
        {
            return _context.Pokemon.Any(p => p.Id == pokeId);
        }

        public bool PokemonExist(string pokeName)
        {
            return _context.Pokemon.Any(p => p.Name ==pokeName);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdatePokemon(int ownerId, int categoryId, PokemonDto pokemonUpdate)
        {
            _context.Update(_mapper.Map<Pokemon>(pokemonUpdate));
            return Save();

        }
    }
}
