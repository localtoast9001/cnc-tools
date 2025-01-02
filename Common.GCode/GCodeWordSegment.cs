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
    /// Initializes a new instance of the <see cref="GCodeWordSegment"/> class.
    /// </summary>
    public GCodeWordSegment()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeWordSegment"/> class.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <param name="value">The value.</param>
    public GCodeWordSegment(Code code, GCodeValue value)
    {
        this.Code = code;
        this.Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeWordSegment"/> class.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <param name="value">The value.</param>
    public GCodeWordSegment(Code code, int value)
    : this(code, new GCodeNumericValue(value))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeWordSegment"/> class.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <param name="value">The value.</param>
    public GCodeWordSegment(Code code, decimal value)
    : this(code, new GCodeNumericValue(value))
    {
    }

    /// <summary>
    /// Gets or sets the code letter.
    /// </summary>
    public Code Code { get; set; }

    /// <summary>
    /// Gets or sets the value for the word.
    /// </summary>
    public GCodeValue? Value { get; set; }

    /// <summary>
    /// Creates a command code word.
    /// </summary>
    /// <param name="command">The command code.</param>
    /// <returns>A new instance of the <see cref="GCodeWordSegment"/> class.</returns>
    public static GCodeWordSegment FromCommand(CommandCode command) =>
        new(command.Code, command.Value);

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