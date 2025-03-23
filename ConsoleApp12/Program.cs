using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using ConsoleApp12.Benchmark;
using ConsoleApp12.Core;
using ConsoleApp12.HelpMe;
using ConsoleApp12.Player;
using System.Text;

namespace ConsoleApp12;

public class Program
{
    private static readonly CursedPlayer s_cursedPlayer = new();
    private static List<string>? s_songNames;

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        //BenchmarkRunner.Run<ShuffleCalypse>();

        ShowUI();
    }

    private static void ShowUI()
    {
        s_songNames = SongsData.GetFileNames().ToList();

        while (true)
        {
            ShowSongs();

            var chosenNumber = GetUserChoice(s_songNames.Count);

            if (chosenNumber == -1)
            {
                s_cursedPlayer.Stop();
                continue;
            }

            Task.Run(() => s_cursedPlayer.Play("/songs/" + s_songNames[chosenNumber - 1]));
        }
    }

    private static void ShowSongs()
    {
        Console.Clear();
        Console.WriteLine("Play: number; Stop: any 'normal' key");

        for (int i = 0; i < s_songNames!.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {s_songNames[i]}");
        }
    }

    private static int GetUserChoice(int songCount)
    {
        var input = Console.ReadLine();

        if (int.TryParse(input, out int choice))
        {
            if (choice >= 0 && choice <= songCount)
            {
                return choice;
            }
        }

        return -1;
    }
}
