using ConsoleApp12.Core;
using NAudio.Wave;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace ConsoleApp12.Player;

internal sealed class CursedPlayer
{
    private WaveOutEvent _outputDevice;
    private AudioFileReader _reader;

    public Task Play(string name)
    {

        if (_outputDevice != null)
        {
            _outputDevice?.Stop();
        }

        // Get bpm
        var detector = new BPMDetector(Environment.CurrentDirectory + name);
        var bpm = detector.Groups[0].Tempo;
        var beatDurationMs = 60 / bpm * 1000;

        Debug.WriteLine(bpm);

        // Shuffle
        var segmentsToPlay = Shuffler.GetPlaybackSegments(bpm);

        // Player init
        _outputDevice = new WaveOutEvent();
        _reader = new AudioFileReader(Environment.CurrentDirectory + name);
        _outputDevice.Init(_reader);

        // TODO
        // Create actual .waw or something instead of setting player position every time ?
        foreach (var segment in segmentsToPlay)
        {
            // null ref maybe?
            _reader.CurrentTime = TimeSpan.FromMilliseconds(segment.StartPosition);
            _outputDevice.Play();
            Thread.Sleep(TimeSpan.FromMilliseconds(beatDurationMs * segment.Length));
            if (_outputDevice.PlaybackState == PlaybackState.Stopped)
            {
                break;
            }
        }

        return Task.CompletedTask;
    }

    public void Stop()
    {
        _outputDevice?.Stop();
    }
}
