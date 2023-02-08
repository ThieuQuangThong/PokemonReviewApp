using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        // GET Reviewers
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        public async Task<IActionResult> GetReviewers()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var reviewers = await _reviewerRepository.GetReviewers();
                return Ok(reviewers);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }

        // GET Reviewers/id
        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        public async Task<IActionResult> GetReviewer(int reviewerId)
        {
            var reviewer = await _reviewerRepository.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewer);

        }

        // GET Reviewers/reviewerId/review
        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviewsByAReviewer(int reviewerId)
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewOfReviewer(reviewerId)).ToList();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviews);
        }

        // POST Reviewer
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateReviewer([FromBody] ReviewerDto reviewerCreate)
        {
            if (reviewerCreate == null)
                return BadRequest(ModelState);


            if (_reviewerRepository.ReviewerExists(reviewerCreate.LastName))
            {
                ModelState.AddModelError("", "Reviewer already exist !!!");
                return StatusCode(422, ModelState);
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _reviewerRepository.CreateReviewer(reviewerCreate);
                return Ok(reviewerCreate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT Reviewer
        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateReviewer(int reviewerId, [FromBody] ReviewerDto updateReviewer)
        {
            if (updateReviewer == null)
                return BadRequest(ModelState);

            if (reviewerId != updateReviewer.Id)
                return BadRequest(ModelState);

            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                await _reviewerRepository.UpdateReviewer(updateReviewer);
                return Ok(updateReviewer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();
            var reviewerToDelete = await _reviewerRepository.GetReviewer(reviewerId);

            try
            {
                _reviewerRepository.DeleteReviewer(reviewerToDelete);
                return Ok(reviewerToDelete);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }
    }
}
