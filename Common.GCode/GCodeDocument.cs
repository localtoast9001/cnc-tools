//-----------------------------------------------------------------------
// <copyright file="GCodeDocument.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.GCode;

using System.Collections.ObjectModel;

/// <summary>
/// Low-level document object model of a GCODE/NGC/RS274 document.
/// </summary>
/// <seealso href="https://www.nist.gov/publications/nist-rs274ngc-interpreter-version-3?pub_id=823374"/>
public class GCodeDocument
{
    /// <summary>
    /// Gets the lines in the document.
    /// </summary>
    public Collection<GCodeLine> Lines { get; } = new Collection<GCodeLine>();

    /// <summary>
    /// Creates a new <see cref="GCodeDocument"/> from a string.
    /// </summary>
    /// <param name="text">A string that contains GCode.</param>
    /// <returns>
    /// A new instance of the <see cref="GCodeDocument"/> class.
    /// </returns>
    public static GCodeDocument Parse(string text)
    {
        return Load(new IO.GCodeTextReader(new StringReader(text)));
    }

    /// <summary>
    /// Creates a new <see cref="GCodeDocument"/> from a <see cref="IO.GCodeReader"/>.
    /// </summary>
    /// <param name="reader">The reader that has the content for the document.</param>
    /// <returns>
    /// A new instance of the <see cref="GCodeDocument"/> class.
    /// </returns>
    public static GCodeDocument Load(IO.GCodeReader reader)
    {
        GCodeDocument result = new();
        ExpectStartFile(reader);
        while (reader.Read())
        {
            if (reader.TokenType == IO.GCodeTokenType.FileEnd)
            {
                break;
            }

            if (reader.TokenType != IO.GCodeTokenType.LineStart)
            {
                throw new InvalidOperationException($"Unexpected input {reader.TokenType}. Expected line start.");
            }

            result.Lines.Add(LoadLine(reader));
        }

        return result;
    }

    /// <summary>
    /// Writes the document to a <see cref="IO.GCodeWriter"/>.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public void WriteTo(IO.GCodeWriter writer)
    {
        writer.StartFile();
        foreach (GCodeLine line in this.Lines)
        {
            line.WriteTo(writer);
        }

        writer.EndFile();
    }

    private static GCodeLine LoadLine(IO.GCodeReader reader)
    {
        GCodeLine result = new();
        result.IsBlockDelete = reader.IsBlockDeleteLine;
        result.LineNumber = reader.LineNumber;

        while (reader.Read() && reader.TokenType != IO.GCodeTokenType.LineEnd)
        {
            result.Segments.Add(LoadSegment(reader));
        }

        return result;
    }

    private static GCodeSegment LoadSegment(IO.GCodeReader reader)
    {
        switch (reader.TokenType)
        {
            case IO.GCodeTokenType.CommentOrMessage:
                return LoadComment(reader);
            case IO.GCodeTokenType.WordStart:
                return LoadWord(reader);
            default:
                throw new InvalidOperationException($"Unexpected token {reader.TokenType}.");
        }
    }

    private static GCodeCommentSegment LoadComment(IO.GCodeReader reader)
    {
        return new GCodeCommentSegment() { Text = reader.Comment };
    }

    private static GCodeWordSegment LoadWord(IO.GCodeReader reader)
    {
        Code code = (Code)reader.WordLetter;
        if (!reader.Read())
        {
            throw new InvalidOperationException("Expected value for word, not end of file.");
        }

        if (reader.TokenType != IO.GCodeTokenType.Value)
        {
            throw new InvalidOperationException($"Unexpected or unsupported token {reader.ValueType} in input.");
        }

        GCodeNumericValue value = new()
        {
            IsInteger = reader.ValueType == IO.GCodeValueType.Integer,
            Value = reader.Value,
        };

        if (!reader.Read() || reader.TokenType != IO.GCodeTokenType.WordEnd)
        {
            throw new InvalidOperationException("Expected end of word in input.");
        }

        return new GCodeWordSegment() { Code = code, Value = value };
    }

    private static void ExpectStartFile(IO.GCodeReader reader)
    {
        if (!reader.Read() || reader.TokenType != IO.GCodeTokenType.FileStart)
        {
            throw new InvalidOperationException("Expected file start.");
        }
    }
}