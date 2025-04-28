using IngredientsApi.Interface;
using IngredientsApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IngredientsApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class DeploymentController : ControllerBase
    {
        private readonly IDeploymentService _deploymentService;

        public DeploymentController(IDeploymentService deployment)
        {
            _deploymentService = deployment;
        }

        [HttpGet("test-db-connection")]
        public async Task<IActionResult> TestDbConnection()
        {
            var isConnected = await _deploymentService.TestDbConnectionAsync();
            if (isConnected)
            {
                return Ok("Database connection successful!");
            }
            else
            {
                return StatusCode(500, "Database connection failed!");
            }
        }
    }

}
