//-----------------------------------------------------------------------
// <copyright file="GCodeWordSegment.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.GCode;

/// <summary>
/// Word segment inside a <see cref="GCodeLine"/>.
/// </summary>
public class GCodeWordSegment : GCodeSegment
{
    /// <summary>
    /// Gets or sets the code letter.
    /// </summary>
    public Code Code { get; set; }

    /// <summary>
    /// Gets or sets the value for the word.
    /// </summary>
    public GCodeValue? Value { get; set; }

    /// <inheritdoc/>
    public override void WriteTo(IO.GCodeWriter writer)
    {
        if (this.Value == null)
        {
            throw new InvalidOperationException("The Value property must not be null.");
        }

        writer.StartWord(this.Code);
        this.Value.WriteTo(writer);
        writer.EndWord();
    }
}