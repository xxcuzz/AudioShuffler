using System.Reflection;

namespace ConsoleApp12.HelpMe;

public static class SongsData
{
    private static readonly List<string> SupportedSongExt = [
        ".mp3"
        ];


    public static IEnumerable<string> GetFilePaths()
    {
        var asm = Assembly.GetExecutingAssembly();

        var names = asm.GetManifestResourceNames()
             .Where(res => SupportedSongExt.Any(ext => res.EndsWith(ext)));

        return names;
    }

    public static IEnumerable<string> GetFileNames()
    {
        var paths = GetFilePaths();

        // TODO get rid of hardcode, or not
        return paths.Select(res => Path.GetFileName(res).Replace("ConsoleApp12.songs.", ""));
    }
}
