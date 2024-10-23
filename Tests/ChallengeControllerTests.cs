using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Challenge.Tests
{
    public class ChallengeControllerTests
    {
        private readonly ChallengeController _controller;

        public ChallengeControllerTests()
        {
            _controller = new ChallengeController();
        }

        [Fact]
        public void Get_ValidRequest_ReturnsOkResult()
        {
            var request = new Request
            {
                x_capacity = 5,
                y_capacity = 3,
                z_amount_wanted = 4
            };
            var result = _controller.Get(request) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public void Get_NoSolution_ReturnsInternalServerError()
        {
            var request = new Request
            {
                x_capacity = 2,
                y_capacity = 3,
                z_amount_wanted = 5 // No es posible
            };
            var result = _controller.Get(request) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal("No solution", result.Value.GetType().GetProperty("solution").GetValue(result.Value));
        }
    }
}
