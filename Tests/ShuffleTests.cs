using ConsoleApp12.Core;

namespace Tests
{
    public class ShuffleTests
    {
        private const float TEST_BPM = 128.0f;
        private const double TEST_TRACK_DURATION = 180_000;

        [Fact]
        public void GetPlaybackSegments_WithValidInput_ReturnsValidSegments()
        {
            // Act
            var segments = Shuffler.GetPlaybackSegments(TEST_BPM, TEST_TRACK_DURATION);

            // Assert
            Assert.NotNull(segments);
            Assert.NotEmpty(segments);
            Assert.All(segments, segment => Assert.True(segment.Length > 0));
        }

        [Fact]
        public void GetPlaybackSegments_StartsWithOriginalIntro()
        {
            // Act
            var segments = Shuffler.GetPlaybackSegments(TEST_BPM, TEST_TRACK_DURATION);

            // Assert
            Assert.Equal(0, segments[0].StartPosition);
        }

        [Fact]
        public void GetPlaybackSegments_EndsWithOriginalOutro()
        {
            // Arrange 
            var bpmDuration = 60 / TEST_BPM * 1000;

            // Act
            var segments = Shuffler.GetPlaybackSegments(TEST_BPM, TEST_TRACK_DURATION);

            // Assert
            Assert.True(segments[^1].StartPosition == segments.Max(x=>x.StartPosition));
        }

        [Theory]
        [InlineData(60)]
        [InlineData(128)]
        [InlineData(180)]
        public void GetPlaybackSegments_HandlesVariousBPMs(int bpm)
        {
            // Act
            var segments = Shuffler.GetPlaybackSegments(bpm, TEST_TRACK_DURATION);

            // Assert
            Assert.NotEmpty(segments);
            Assert.All(segments, segment => 
            {
                Assert.True(segment.StartPosition >= 0);
                Assert.True(segment.Length > 0);
            });
        }

        [Theory]
        [InlineData(30_000)]
        [InlineData(60_000)]
        [InlineData(180_000)]
        [InlineData(600_000)]
        public void GetPlaybackSegments_HandlesVariousDurations(int trackDurationMs)
        {
            // Act
            var segments = Shuffler.GetPlaybackSegments(TEST_BPM, trackDurationMs);

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
