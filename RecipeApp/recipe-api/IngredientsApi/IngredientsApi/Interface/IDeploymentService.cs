using Microsoft.AspNetCore.Mvc;

namespace IngredientsApi.Interface
{
    public interface IDeploymentService
    {
        Task<bool> TestDbConnectionAsync();
    }
}
