using Xunit;

namespace Challenge.Tests
{
    public class WaterBucketChallengeTests
    {
        [Fact]
        public void IsPossible_ReturnsTrue_WhenSolutionExists()
        {
            var challenge = new WaterBucketChallenge(5, 3, 4);
            var result = challenge.IsPossible();
            Assert.True(result);
        }

        [Fact]
        public void IsPossible_ReturnsFalse_WhenSolutionDoesNotExist()
        {
            var challenge = new WaterBucketChallenge(2, 3, 5); // No es posible
            var result = challenge.IsPossible();
            Assert.False(result);
        }

        [Fact]
        public void Pour_ValidParameters_ReturnsSteps()
        {
            var challenge = new WaterBucketChallenge(5, 3, 4);
            var steps = challenge.Pour(5, 3, 4);
            Assert.NotEmpty(steps);
        }
    }
}
