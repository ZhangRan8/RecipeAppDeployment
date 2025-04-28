using IngredientsApi.Data;
using IngredientsApi.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace IngredientsApi.Service
{
    public class DeploymentService : IDeploymentService
    {
        private readonly AppDbContext _context;

        public DeploymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> TestDbConnectionAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {
                    await connection.OpenAsync();
                    return connection.State == System.Data.ConnectionState.Open;
                }
            }
            catch
            {
                // You can log the exception here if you want
                return false;
            }
        }
    }
}
