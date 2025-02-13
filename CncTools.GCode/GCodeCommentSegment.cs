//-----------------------------------------------------------------------
// <copyright file="GCodeCommentSegment.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CncTools.GCode;

/// <summary>
/// A comment or message segment.
/// </summary>
public class GCodeCommentSegment : GCodeSegment
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeCommentSegment"/> class.
    /// </summary>
    public GCodeCommentSegment()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeCommentSegment"/> class.
    /// </summary>
    /// <param name="text">The comment text.</param>
    /// <param name="isMessage"><c>True</c> if the comment is a message; otherwise, <c>false</c>.</param>
    public GCodeCommentSegment(string text, bool isMessage = false)
    {
        this.Text = text;
        this.IsMessage = isMessage;
    }

    /// <summary>
    /// Gets or sets the text of the comment.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this comment is a message.
    /// </summary>
    public bool IsMessage { get; set; }

    /// <inheritdoc/>
    public override void WriteTo(IO.GCodeWriter writer)
    {
        if (this.IsMessage)
        {
            writer.WriteMessage(this.Text ?? string.Empty);
        }
        else
        {
            writer.WriteComment(this.Text ?? string.Empty);
        }
    }
}