using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cinema_ProjAss_Application.DTOs.Shows;
using Cinema_ProjAss_Application.Exceptions;
using Cinema_ProjAss_Application.Services;

namespace Cinema_ProjAss_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;

        public ShowsController(IShowService showService)
        {
            _showService = showService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ShowDto>> GetById(int id)
        {
            try
            {
                var show = await _showService.GetByIdAsync(id);
                return Ok(show);
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
        }

        [HttpGet("movie/{movieId:int}")]
        public async Task<ActionResult<IEnumerable<ShowDto>>> GetByMovie(int movieId)
        {
            var shows = await _showService.GetShowsByMovieAsync(movieId);
            return Ok(shows);
        }

        // Ikke-basic CRUD (business query)
        [HttpGet("upcoming")]
        public async Task<ActionResult<IEnumerable<ShowDto>>> GetUpcoming([FromQuery] DateTime? fromDate)
        {
            var date = fromDate ?? DateTime.UtcNow;
            var shows = await _showService.GetUpcomingShowsAsync(date);
            return Ok(shows);
        }

        [HttpPost]
        public async Task<ActionResult<ShowDto>> Create([FromBody] CreateShowDto dto)
        {
            try
            {
                var created = await _showService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ValidationException ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateShowDto dto)
        {
            try
            {
                await _showService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (ValidationException ex) { return BadRequest(ex.Message); }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _showService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (ValidationException ex) { return BadRequest(ex.Message); }
        }
    }
}
