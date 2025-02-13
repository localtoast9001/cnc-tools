//-----------------------------------------------------------------------
// <copyright file="GCodeLine.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CncTools.GCode;

using System.Collections.ObjectModel;

/// <summary>
/// Represents a line in a <see cref="GCodeDocument"/>.
/// </summary>
public class GCodeLine
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeLine"/> class.
    /// </summary>
    public GCodeLine()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeLine"/> class.
    /// </summary>
    /// <param name="segments">The segments in the line.</param>
    public GCodeLine(params GCodeSegment[] segments)
    : this(segments, -1, false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeLine"/> class.
    /// </summary>
    /// <param name="segments">The segments in the line.</param>
    /// <param name="lineNumber">Optional line number.</param>
    /// <param name="blockDelete"><c>True</c> if this line is block deleted; otherwise, <c>false</c>.</param>
    public GCodeLine(
        IEnumerable<GCodeSegment> segments,
        int lineNumber = -1,
        bool blockDelete = false)
    {
        this.Segments.AddRange(segments);
        this.LineNumber = lineNumber;
        this.IsBlockDelete = blockDelete;
    }

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
    /// Creates a new line with a single comment.
    /// </summary>
    /// <param name="text">The comment text.</param>
    /// <param name="isMessage"><c>True</c> to create a message; otherwise, <c>false</c>.</param>
    /// <returns>
    /// A new instance of the <see cref="GCodeLine"/> class.
    /// </returns>
    public static GCodeLine Comment(string text, bool isMessage = false) =>
        new(new GCodeCommentSegment(text, isMessage));

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