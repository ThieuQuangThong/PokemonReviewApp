using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokeRepository;
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewController(IReviewRepository reviewRepository, IPokemonRepository pokemonRepository, IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _pokeRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
        }

        // GET Reviews
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await _reviewRepository.GetReviews();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviews);
        }

        // GET Reviews/id
        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReview(int reviewId)
        {
            var review = await _reviewRepository.GetReview(reviewId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(review);
        }

        // GET Review/pokemon/pokeId
        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewsOfAPokemon(int pokeId)
        {
            var reviews = await _reviewRepository.GetReviewOfAPokemon(pokeId);
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            return Ok(reviews);
        }

        // POST Review
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateReview([FromQuery] int reviewerId, [FromQuery] int pokeId, [FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _reviewRepository.CreateReview(reviewerId, pokeId, reviewCreate);
                return Ok(reviewCreate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        // PUT review
        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] ReviewDto updateReview)
        {
            if (updateReview == null)
                return BadRequest(ModelState);

            if (reviewId != updateReview.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExist(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                await _reviewRepository.UpdateReview(updateReview);
                return Ok(updateReview);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE review
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExist(reviewId))
            {
                return NotFound();
            }

            var reviewToDelete = await _reviewRepository.GetReview(reviewId);
            try
            {
                await _reviewRepository.DeleteReview(reviewToDelete);
                return Ok(reviewToDelete);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
