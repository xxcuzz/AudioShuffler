using System.Text.Json;
using ConsoleApp12.Core;

namespace ConsoleApp12.HelpMe;

public static class BpmData
{
    // BPM detector is eating ton of ram, so im storing known bpm
    // Key: song name, Value: BPM
    private static Dictionary<string, float>? s_bpmData;
    private const string BpmDataFileName = "bpmdata.json";

    public static float GetBpm(string name)
    {
        LoadBpmData();

        if (s_bpmData!.TryGetValue(name, out var bpm))
        {
            return bpm;
        }

        var detector = new BPMDetector(Environment.CurrentDirectory + name);
        var detectedBpm = detector.Groups[0].Tempo;

        s_bpmData[name] = detectedBpm;
        SaveBpm();

        return detectedBpm;
    }

    private static void LoadBpmData()
    {
        if (s_bpmData != null) return; // Данные уже загружены

        if (File.Exists(BpmDataFileName))
        {
            using var stream = File.OpenRead(BpmDataFileName);
            s_bpmData = JsonSerializer.Deserialize<Dictionary<string, float>>(stream);
            return;
        }

        s_bpmData = new Dictionary<string, float>();
    }

    private static void SaveBpm()
    {
        if (s_bpmData == null) return;

        using var stream = File.Create(BpmDataFileName);
        JsonSerializer.Serialize(stream, s_bpmData);
    }
}
