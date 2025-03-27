namespace ConsoleApp12.Core;

// TODO
// sometimes deliberately messing up on some songs
public static class Shuffler
{
    // Play original intro and outro (in beats count)
    private const int ORIGINAL_SOUND_OFFSET = 4;

    public static List<BeatPart> GetPlaybackSegments(float bpm, double trackDurationMs, int beatsInStep = 4)
    {
        List<BeatPart> segments = [];

        var beatDurationMs = 60 / bpm * 1000;
        var beatsInTrack = trackDurationMs / beatDurationMs;
        var beatsInPart = beatsInStep * beatsInStep;

        for (int i = 0; i < beatsInTrack / beatsInPart; i++)
        {
            var part = GetPartFromAudio(bpm, beatsInPart, beatsInPart * i);
            var shuffledPart = Shuffle(part);
            segments.AddRange(shuffledPart);
        }

        PutOriginalIntroAndOutro(segments);
        
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

    internal static List<BeatPart> Shuffle(List<double> parts)
    {
        // Divide parts into even and odd groups
        if (parts == null || parts.Count == 0)
            throw new ArgumentException("Parts list is empty");

        List<BeatPart> evenParts = [];
        List<BeatPart> oddParts = [];

        bool isEvenPart = true;
        for (int i = 0; i < parts.Count;)
        {
            int groupSize = Random.Shared.Next(1, Math.Min(4, parts.Count - i));
            if (isEvenPart)
            {
                evenParts.Add(new BeatPart(parts[i], groupSize));
            }
            else
            {
                oddParts.Add(new BeatPart(parts[i], groupSize));
            }

            i += groupSize;
            if (groupSize % 2 == 1)
                isEvenPart = !isEvenPart;
        }

        // Shuffle each group
        evenParts = evenParts.OrderBy(_ => Random.Shared.Next()).ToList();
        oddParts = oddParts.OrderBy(_ => Random.Shared.Next()).ToList();

        // Merge groups
        List<BeatPart> shuffledParts = [];
        int evenIndex = 0;
        int oddIndex = 0;
        isEvenPart = true;

        while (evenIndex < evenParts.Count || oddIndex < oddParts.Count)
        {
            BeatPart currentPart;
            if (isEvenPart && evenIndex < evenParts.Count)
            {
                currentPart = evenParts[evenIndex++];
            }
            else if(!isEvenPart && oddIndex < oddParts.Count)
            {
                currentPart = oddParts[oddIndex++];
            }
            else
            {
                while (evenIndex < evenParts.Count)
                {
                    shuffledParts.Add(evenParts[evenIndex++]);
                }
                while (oddIndex < oddParts.Count)
                {
                    shuffledParts.Add(oddParts[oddIndex++]);
                }
                break;
            }
           
            shuffledParts.Add(currentPart);
            if (currentPart.Length % 2 == 1)
                isEvenPart = !isEvenPart;
        }

        return shuffledParts;
    }

    private static void PutOriginalIntroAndOutro(List<BeatPart> segments)
    {
        // Need to keep lengths to not mess up rhythm
        int first = segments.FindIndex(s => s.StartPosition == 0);
        segments[first] = new BeatPart(segments[0].StartPosition, segments[first].Length);
        segments[0] = new BeatPart(0, segments[0].Length);

        int last = segments.FindIndex(s => s.StartPosition == segments.Max(seg => seg.StartPosition));
        double lastPosition = segments[^1].StartPosition;
        segments[^1] = new BeatPart(segments[last].StartPosition, segments[^1].Length);
        segments[last] = new BeatPart(lastPosition, segments[last].Length);
    }
}