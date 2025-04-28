using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests.CommonFiles
{
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
    {
        protected readonly IServiceScope _scope;

        protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            _scope = factory.Services.CreateScope();

        }
    }
}
