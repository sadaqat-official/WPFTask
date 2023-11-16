using Common.Model;
using Common.Repositories;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;

namespace WPFTask.Tests
{
    public class EmployeeRepositoryTests
    {

        [Fact]
        public async Task AddEmployeeAsync_Success()
        {
            // Arrange
            var expectedEmployee = new Employee
            {
                Id = 1234567,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Gender = "male",
                Status = "active"
            };

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{'status':'success'}", Encoding.UTF8, "application/json"),
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {

            };

            var employeeRepository = new EmployeeRepository(httpClient);

            // Act
            var result = await employeeRepository.AddEmployeeAsync(expectedEmployee);

            // Assert
            Assert.True(result, "Expected the employee to be added successfully.");

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // Expected number of invocations
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
