using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cinema_ProjAss_Application.DTOs.Movies;
using Cinema_ProjAss_Application.Services;

namespace Cinema_ProjAss_Api.Controllers
{
    /// <summary>
    /// REST API endpoints for film (Movies).
    /// Fejl håndteres via global ExceptionMiddleware.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        /// <summary>
        /// Henter alle film.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetAll()
        {
            var movies = await _movieService.GetAllAsync();
            return Ok(movies);
        }

        /// <summary>
        /// Henter én film ud fra id.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieDto>> GetById(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            return Ok(movie);
        }

        /// <summary>
        /// Søger film via titel (query string).
        /// Eksempel: GET /api/movies/search?title=Avatar
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> Search([FromQuery] string title)
        {
            var movies = await _movieService.SearchByTitleAsync(title);
            return Ok(movies);
        }

        /// <summary>
        /// Opretter en ny film.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<MovieDto>> Create([FromBody] CreateMovieDto dto)
        {
            var created = await _movieService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Opdaterer en eksisterende film.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateMovieDto dto)
        {
            await _movieService.UpdateAsync(id, dto);
            return NoContent();
        }

        /// <summary>
        /// Sletter en film.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _movieService.DeleteAsync(id);
            return NoContent();
        }
    }
}
