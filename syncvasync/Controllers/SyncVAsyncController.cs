using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Threading;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace syncvasync.Controllers
{
    [Route("api/[controller]")]
    public class SyncVAsyncController : Controller
    {
        private readonly string _connectionString;
        public SyncVAsyncController(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection");

        [HttpGet("sync")]
        public IActionResult SyncGet()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                connection.Execute("WAITFOR DELAY '00:00:02';");
            }
            return Ok(GetThreadInfo());
        }

        [HttpGet("async")]
        public async Task<IActionResult> AsyncGet()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                await connection.ExecuteAsync("WAITFOR DELAY '00:00:02';");
            }
            return Ok(GetThreadInfo());
        }

        private dynamic GetThreadInfo()
        {
            int availableWorkerThreads;
            int availableAsyncIOThreads;
            ThreadPool.GetAvailableThreads(out availableWorkerThreads, out availableAsyncIOThreads);

            return new { AvailableAsyncIOThreads = availableAsyncIOThreads, AvailableWorkerThreads = availableWorkerThreads };
        }
    }
}