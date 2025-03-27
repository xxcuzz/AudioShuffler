using ConsoleApp12.Core;
using ConsoleApp12.HelpMe;
using NAudio.Wave;
using System.Diagnostics;

namespace ConsoleApp12.Player;

internal static class CursedPlayer
{
    private static WaveOutEvent? _outputDevice;
    private static AudioFileReader? _reader;

    public static Task Play(string name)
    {
        // Get bpm
        var bpm = BpmData.GetBpm(name);
        var beatDurationMs = 60 / bpm * 1000;

        Debug.WriteLine(bpm);

        // Player initialization
        _outputDevice = new WaveOutEvent();
        _reader = new AudioFileReader(Environment.CurrentDirectory + name);
        _outputDevice.Init(_reader);

        var trackDurationMs = _reader.TotalTime.TotalMilliseconds;

        // Shuffle
        var segmentsToPlay = Shuffler.GetPlaybackSegments(bpm, trackDurationMs);
        _outputDevice.Play();

        foreach (var segment in segmentsToPlay)
        {
            if (!(_outputDevice.PlaybackState == PlaybackState.Playing)) break;
            _reader.CurrentTime = TimeSpan.FromMilliseconds(segment.StartPosition);
            Thread.Sleep(TimeSpan.FromMilliseconds(beatDurationMs * segment.Length));
        }


        Stop();
        return Task.CompletedTask;
    }

    public static void Stop()
    {
        _outputDevice?.Stop();
        _reader?.Dispose();
        _outputDevice?.Dispose();
    }
}
