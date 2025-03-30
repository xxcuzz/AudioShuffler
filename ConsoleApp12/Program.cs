using BenchmarkDotNet.Running;
using ConsoleApp12.Benchmark;
using ConsoleApp12.HelpMe;
using ConsoleApp12.Player;
using System.Text;

namespace ConsoleApp12;

public class Program
{
    private static List<string>? s_songNames;
    private static CancellationTokenSource _cts = new();
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        //BenchmarkRunner.Run<ShuffleCalypse>();

        ShowUI();
    }

    private static void ShowUI()
    {
        s_songNames = SongsData.GetFileNames().ToList();
        ShowSongs();

        while (true)
        {
            var chosenNumber = GetUserChoice(s_songNames.Count);

            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();

            if (chosenNumber == -1)
            {
                continue;
            }

            DeleteLastLine();
            ShowSongs();
            Console.WriteLine($"Playing: {s_songNames[chosenNumber - 1]}");
            Task.Run(() => CursedPlayer.Play("/songs/" + s_songNames[chosenNumber - 1], _cts.Token))
                .ContinueWith(t => ShowSongs());
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

        DeleteLastLine();
        if (int.TryParse(input, out int choice))
        {
            if (choice >= 0 && choice <= songCount)
            {
                return choice;
            }
        }

        return -1;
    }

    private static void DeleteLastLine()
    {
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop);
    }
}
