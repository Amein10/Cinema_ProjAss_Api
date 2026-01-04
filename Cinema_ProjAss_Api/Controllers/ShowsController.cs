using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cinema_ProjAss_Application.DTOs.Shows;
using Cinema_ProjAss_Application.Services;

namespace Cinema_ProjAss_Api.Controllers
{
    /// <summary>
    /// REST API endpoints for visninger (Shows).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;

        public ShowsController(IShowService showService)
        {
            _showService = showService;
        }

        /// <summary>
        /// Henter en visning ud fra id.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ShowDto>> GetById(int id)
        {
            var show = await _showService.GetByIdAsync(id);
            return Ok(show);
        }

        /// <summary>
        /// Henter alle shows for en given film.
        /// Eksempel: GET /api/shows/movie/5
        /// </summary>
        [HttpGet("movie/{movieId:int}")]
        public async Task<ActionResult<IEnumerable<ShowDto>>> GetByMovie(int movieId)
        {
            var shows = await _showService.GetShowsByMovieAsync(movieId);
            return Ok(shows);
        }

        /// <summary>
        /// Henter alle kommende shows fra en given dato.
        /// Hvis 'from' ikke angives, bruges DateTime.UtcNow.
        /// Eksempel: GET /api/shows/upcoming?from=2026-01-04T00:00:00Z
        /// </summary>
        [HttpGet("upcoming")]
        public async Task<ActionResult<IEnumerable<ShowDto>>> GetUpcoming([FromQuery] DateTime? from)
        {
            var shows = await _showService.GetUpcomingShowsAsync(from ?? DateTime.UtcNow);
            return Ok(shows);
        }

        /// <summary>
        /// Opretter en ny visning.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ShowDto>> Create([FromBody] CreateShowDto dto)
        {
            var created = await _showService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}

