//-----------------------------------------------------------------------
// <copyright file="GCodeLine.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.GCode;

using System.Collections.ObjectModel;

/// <summary>
/// Represents a line in a <see cref="GCodeDocument"/>.
/// </summary>
public class GCodeLine
{
    /// <summary>
    /// Gets or sets a value indicating whether this line is block deleted.
    /// </summary>
    public bool IsBlockDelete { get; set; }

    /// <summary>
    /// Gets or sets the optional line number.
    /// </summary>
    /// <remarks>
    /// If the value is less than 0, the line number is not specified.
    /// </remarks>
    public int LineNumber { get; set; } = -1;

    /// <summary>
    /// Gets the ordered list of segments that compose the line.
    /// </summary>
    public Collection<GCodeSegment> Segments { get; } = new Collection<GCodeSegment>();

    /// <summary>
    /// Writes the contents of the line to an open writer.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public void WriteTo(IO.GCodeWriter writer)
    {
        writer.StartLine(this.LineNumber, this.IsBlockDelete);
        foreach (GCodeSegment segment in this.Segments)
        {
            segment.WriteTo(writer);
        }

        writer.EndLine();
    }
}