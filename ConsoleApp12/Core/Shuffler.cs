using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;
using NAudio.Wave;
using System.Diagnostics;
using System.Text;

namespace ConsoleApp12.Core;

// TODO
// sometimes deliberately messing up on some songs
public static class Shuffler
{
    public static List<BeatPart> GetPlaybackSegments(float bpm, int beatsInStep = 4) {
        List<BeatPart> segments = [];

        // 4 * 8 should be replaced with some representation of track duration
        for (int i = 0; i < 4 * 8; i++)
        {
            var beatsInPart = beatsInStep * beatsInStep;
            var part = GetPartFromAudio(bpm, beatsInPart, beatsInPart * i);
            var shuffledPart = ShufflePart(part);
            segments.AddRange(shuffledPart);
        }

        return segments;
    }

    private static List<double> GetPartFromAudio(double bpm, int beatsInPart, int offset = 0)
    {
        var beatDuration = 60 / bpm * 1000;
        List<double> startingPositions = [];

        for (int partIndex = offset; partIndex < offset + beatsInPart; partIndex++)
        {
            var startPosition = beatDuration * partIndex;

            startingPositions.Add(startPosition);
        }

        return startingPositions;
    }

    // TODO Implement non-repeatable shuffle
    internal static List<BeatPart> ShufflePart(List<double> parts)
    {
        List<BeatPart> outPositions = [];
        int[] lastR = [-1, -1];

        if (parts[0] == 0)
        {
            outPositions.Add(new BeatPart(0, 4));
        }

        for (int i = 0; i < parts.Count;)
        {
            int randomLength = Math.Min(Random.Shared.Next(1, 4), parts.Count - i);
            var oddOrEven = i % 2;

            var selectables = parts.Select((value, index) => (value, index))
                .Where(p =>
                p.index != lastR[0] &&
                p.index != lastR[1] &&
                p.index % 2 == oddOrEven)
                .ToList();


            if (selectables.Count == 0)
                return outPositions;
            var randIndex = Random.Shared.Next(selectables.Count);
            var selectedPart = selectables[randIndex].value;

            var partElement = parts.Skip(parts.IndexOf(selectedPart)).First();

            int index = parts.IndexOf(selectedPart);

            lastR[1] = lastR[0];
            lastR[0] = index;

            outPositions.Add(new BeatPart(partElement, randomLength));
            i += randomLength;
        }

        return outPositions;
    }
}