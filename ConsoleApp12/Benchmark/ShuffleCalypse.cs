using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ConsoleApp12.Core;

namespace ConsoleApp12.Benchmark
{
    [SimpleJob(RuntimeMoniker.Net90)]
    [MemoryDiagnoser]
    [HtmlExporter]
    public class ShuffleCalypse
    {
        private readonly string _testAudioFile;

        public ShuffleCalypse()
        {
            _testAudioFile = Path.Combine(Environment.CurrentDirectory, "songs", "kept.mp3");
        }

        [Benchmark(Description = "BPM Detection")]
        public BPMGroup[] BPMDetection()
        {
            var detector = new BPMDetector(_testAudioFile);
            return detector.Groups;
        }

        [Benchmark(Description = "Audio Shuffling")]
        public List<BeatPart> Shuffling()
        {
            return Shuffler.GetPlaybackSegments(128);
        }
    }
}
