using Microsoft.AspNetCore.Http;
using Cinema_ProjAss_Application.ReflectionDemo;
using Microsoft.AspNetCore.Mvc;

namespace Cinema_ProjAss_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReflectionController : ControllerBase
    {
        /// <summary>
        /// Demo endpoint:
        /// GET /api/reflection/stortbogstav?text=Hello
        /// Finder metoder med signaturen public bool StortBogstav(string) og kører dem.
        /// </summary>
        [HttpGet("stortbogstav")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<object> RunStortBogstav([FromQuery] string text)
        {
            var results = ReflectionMethodRunner.RunStortBogstavMethods(text);

            // “samlet” resultat: true hvis alle OK-resultater er true (og der var mindst én OK)
            var okResults = results.Where(r => r.Status == "OK" && r.Result.HasValue).ToList();
            var overall = okResults.Count > 0 && okResults.All(r => r.Result == true);

            return Ok(new
            {
                input = text,
                overall,
                results
            });
        }
    }
}

