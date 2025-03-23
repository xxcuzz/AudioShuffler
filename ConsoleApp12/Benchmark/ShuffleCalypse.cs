using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ConsoleApp12.Core;
using ConsoleApp12.HelpMe;

namespace ConsoleApp12.Benchmark
{
    [SimpleJob(RuntimeMoniker.Net90)]
    [MemoryDiagnoser]
    [MinColumn, MaxColumn]
    [HtmlExporter]
    public class ShuffleCalypse
    {
        //public string kept = Environment.CurrentDirectory + 
        //        SongsData.GetFileNames()
        //        .First(x=>x.Contains("kept.mp3"));

        [Benchmark]
        public void Shuffling() => Shuffler.GetPlaybackSegments(128);

        // TODO

        //[Benchmark]
        //public BPMDetector BPMDetection()
        //{
        //    return new BPMDetector(kept);
        //}
    }
}
