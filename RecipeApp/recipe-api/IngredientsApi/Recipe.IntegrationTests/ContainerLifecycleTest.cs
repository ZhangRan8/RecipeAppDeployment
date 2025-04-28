using IngredientsApi.Data;
using IntegrationTests.CommonFiles;
using Microsoft.Extensions.DependencyInjection;

namespace Recipe.IntegrationTests
{
    public class ContainerLifecycleTest : BaseIntegrationTest
    {
        public ContainerLifecycleTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task SqlContainer_ShouldStart_AndAcceptConnections()
        {
            // Arrange
            var dbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Act
            var canConnect = await dbContext.Database.CanConnectAsync();

            // Assert
            Assert.True(canConnect, "The test SQL Server container should be running and accepting connections.");
        }
    }
}
