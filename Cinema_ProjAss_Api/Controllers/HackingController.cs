using Microsoft.AspNetCore.Mvc;
using System.Data;

// Tving SqlClient-typerne til Microsoft.Data.SqlClient
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;

namespace Cinema_ProjAss_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HackingController : ControllerBase
    {
        private readonly string _connStr;

        public HackingController(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("Missing connection string: DefaultConnection");
        }

        // GET api/hacking/ping
        [HttpGet("ping")]
        public IActionResult Ping()
            => Ok("Hacking demo endpoints are alive.");

        // GET api/hacking/dbinfo
        // Debug: viser hvilken DB + server du reelt rammer (LocalDB kan snyde)
        [HttpGet("dbinfo")]
        public async Task<IActionResult> DbInfo()
        {
            await using var conn = new SqlConnection(_connStr);
            await conn.OpenAsync();

            return Ok(new
            {
                DataSource = conn.DataSource,
                Database = conn.Database
            });
        }

        // POST api/hacking/insert?text=hello
        // Sikker INSERT (parameteriseret)
        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromQuery] string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return BadRequest("text is required.");

            const string sql = @"
INSERT INTO dbo.HackDemoLog (TextValue, CreatedUtc)
VALUES (@text, SYSUTCDATETIME());";

            await using var conn = new SqlConnection(_connStr);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@text", SqlDbType.NVarChar, 4000).Value = text;

            var rows = await cmd.ExecuteNonQueryAsync();
            return Ok(new { inserted = rows, savedText = text });
        }

        // DELETE api/hacking/delete?id=123
        // Sikker DELETE (parameteriseret)
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            const string sql = @"DELETE FROM dbo.HackDemoLog WHERE Id = @id;";

            await using var conn = new SqlConnection(_connStr);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            var rows = await cmd.ExecuteNonQueryAsync();
            return Ok(new { deleted = rows, id });
        }

        // GET api/hacking/latest?take=10
        // Hjælper dig med at “vise” at insert virkede, uden at åbne DB manuelt
        [HttpGet("latest")]
        public async Task<IActionResult> Latest([FromQuery] int take = 10)
        {
            if (take < 1 || take > 100) take = 10;

            const string sql = @"
SELECT TOP (@take) Id, TextValue, CreatedUtc
FROM dbo.HackDemoLog
ORDER BY Id DESC;";

            await using var conn = new SqlConnection(_connStr);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@take", SqlDbType.Int).Value = take;

            var result = new List<object>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new
                {
                    Id = reader.GetInt32(0),
                    TextValue = reader.GetString(1),
                    CreatedUtc = reader.GetDateTime(2)
                });
            }

            return Ok(result);
        }
        [HttpPost("createtable")]
        public async Task<IActionResult> CreateTable()
        {
            const string sql = @"
IF NOT EXISTS (
    SELECT 1
    FROM sys.tables t
    JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE s.name = 'dbo' AND t.name = 'HackDemoLog'
)
BEGIN
    CREATE TABLE dbo.HackDemoLog (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        TextValue NVARCHAR(4000) NOT NULL,
        CreatedUtc DATETIME2 NOT NULL
    );
END
";

            await using var conn = new SqlConnection(_connStr);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();

            return Ok("dbo.HackDemoLog created (if it did not exist).");
        }


    }
}
