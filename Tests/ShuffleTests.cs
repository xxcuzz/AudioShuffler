using ConsoleApp12.Core;
using Xunit;

namespace Tests
{
    public class ShuffleTests
    {
        private const float TEST_BPM = 128.0f;

        [Fact]
        public void GetPlaybackSegments_WithValidInput_ReturnsValidSegments()
        {
            // Act
            var segments = Shuffler.GetPlaybackSegments(TEST_BPM);

            // Assert
            Assert.NotNull(segments);
            Assert.NotEmpty(segments);
            Assert.All(segments, segment => Assert.True(segment.Length > 0));
        }

        [Fact]
        public void GetPlaybackSegments_StartsWithOriginalIntro()
        {
            // Act
            var segments = Shuffler.GetPlaybackSegments(TEST_BPM);

            // Assert
            Assert.Equal(0, segments[0].StartPosition);
            Assert.Equal(4, segments[0].Length);
        }

        [Theory]
        [InlineData(60)]
        [InlineData(128)]
        [InlineData(180)]
        public void GetPlaybackSegments_HandlesVariousBPMs(int bpm)
        {
            // Act
            var segments = Shuffler.GetPlaybackSegments(bpm);

            // Assert
            Assert.NotEmpty(segments);
            Assert.All(segments, segment => 
            {
                Assert.True(segment.StartPosition >= 0);
                Assert.True(segment.Length > 0);
            });
        }
    }
}
