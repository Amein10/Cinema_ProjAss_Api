using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cinema_ProjAss_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HackingController : ControllerBase
    {
        // GET: api/<HackingController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<HackingController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            /*
             * 0 - SqlInjection
             *     Det skal være muligt at udføre SQL Injection angreb via denne metode.
             * 1 - SQLConnection
             * 2 - SQLCommand
             * 3 - Open
             * 4 - ExecuteNonQuery
             */

            return "value";
        }

        // POST api/<HackingController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<HackingController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<HackingController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
