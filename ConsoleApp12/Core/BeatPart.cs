namespace ConsoleApp12.Core;

/// <summary>
/// Represents a segment of a track with a defined start position and length.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BeatPart"/> struct.
/// </remarks>
/// <param name="startPosition">The start position in milliseconds.</param>
/// <param name="length">Length in BPM count.</param>
public readonly struct BeatPart(double startPosition, int length)
{
    /// <summary>
    /// Milliseconds from the beginning of the track.
    /// </summary>
    public readonly double StartPosition = startPosition;

    /// <summary>
    /// Length duration in BPM count.
    /// </summary>
    public readonly int Length = length;
}
