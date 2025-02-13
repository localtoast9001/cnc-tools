//-----------------------------------------------------------------------
// <copyright file="GCodeTextWriter.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CncTools.GCode.IO;

/// <summary>
/// Structured text writer for GCode/RS274/NGC files.
/// </summary>
public class GCodeTextWriter : GCodeWriter
{
    private readonly TextWriter inner;
    private readonly Stack<WriterState> stateStack = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeTextWriter"/> class.
    /// </summary>
    /// <param name="inner">The inner text writer.</param>
    /// <param name="settings">Writer settings.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="inner"/> is <see langword="null"/>.
    /// </exception>
    public GCodeTextWriter(TextWriter inner, GCodeWriterSettings? settings = null)
    : base(settings)
    {
        this.inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }

    private enum WriterState
    {
        None,
        File,
        Line,
        Word,
    }

    /// <summary>
    /// Gets the current state of the writer.
    /// </summary>
    private WriterState CurrentState => this.stateStack.Count > 0 ?
        this.stateStack.Peek() :
        WriterState.None;

    /// <inheritdoc/>
    public override void StartFile()
    {
        this.PushState(WriterState.File);
        this.inner.WriteLine("%");
    }

    /// <inheritdoc/>
    public override void EndFile()
    {
        if (this.CurrentState != WriterState.File)
        {
            throw new GCodeTextWriterException("End file not matched with start file.");
        }

        this.inner.WriteLine("%");
        this.stateStack.Pop();
    }

    /// <inheritdoc/>
    public override void StartLine(int lineNumber = -1, bool blockDelete = false)
    {
        if (lineNumber > this.MaxLineNumber)
        {
            throw new ArgumentOutOfRangeException(nameof(lineNumber), $"Valid line numbers are from 0 to {this.MaxLineNumber}.");
        }

        this.PushState(WriterState.Line);
        if (blockDelete)
        {
            this.inner.Write("/ ");
        }

        if (lineNumber > 0)
        {
            this.inner.Write("N");
            this.inner.Write(lineNumber);
            this.inner.Write(" ");
        }
    }

    /// <inheritdoc/>
    public override void EndLine(string? endComment = null)
    {
        if (this.CurrentState != WriterState.Line)
        {
            throw new GCodeTextWriterException("End line not matched with start line.");
        }

        if (endComment != null)
        {
            this.inner.Write("; ");
            this.inner.Write(endComment!);
        }

        this.inner.WriteLine();
        this.stateStack.Pop();
    }

    /// <inheritdoc/>
    public override void StartWord(Code code)
    {
        this.PushState(WriterState.Word);
        this.inner.Write((char)code);
    }

    /// <inheritdoc/>
    public override void EndWord()
    {
        if (this.CurrentState != WriterState.Word)
        {
            throw new GCodeTextWriterException("End of word not matched with start word.");
        }

        this.inner.Write(" ");
        this.stateStack.Pop();
    }

    /// <inheritdoc/>
    public override void WriteComment(string comment)
    {
        if (this.CurrentState != WriterState.Line)
        {
            throw new GCodeTextWriterException("Comments are only valid inside lines.");
        }

        this.inner.Write("( {0} ) ", EscapeComment(comment));
    }

    /// <inheritdoc/>
    public override void WriteMessage(string message)
    {
        this.WriteComment($"MSG, {message}");
    }

    /// <inheritdoc/>
    public override void WriteValue(int value)
    {
        if (this.CurrentState != WriterState.Word)
        {
            throw new GCodeTextWriterException("Values are currently only supported inside words.");
        }

        this.inner.Write("{0}", value);
    }

    /// <inheritdoc/>
    public override void WriteValue(float value)
    {
        if (this.CurrentState != WriterState.Word)
        {
            throw new GCodeTextWriterException("Values are currently only supported inside words.");
        }

        this.inner.Write("{0}", value);
    }

    /// <inheritdoc/>
    public override void WriteValue(double value)
    {
        if (this.CurrentState != WriterState.Word)
        {
            throw new GCodeTextWriterException("Values are currently only supported inside words.");
        }

        this.inner.Write("{0}", value);
    }

    /// <inheritdoc/>
    public override void WriteValue(decimal value)
    {
        if (this.CurrentState != WriterState.Word)
        {
            throw new GCodeTextWriterException("Values are currently only supported inside words.");
        }

        this.inner.Write("{0}", value);
    }

    /// <summary>
    /// Starts setting a parameter.
    /// </summary>
    public override void StartParameterSetting()
    {
    }

    /// <summary>
    /// Ends setting a parameter.
    /// </summary>
    public override void EndParameterSetting()
    {
    }

    /// <summary>
    /// Starts an expression.
    /// </summary>
    public override void StartExpression()
    {
    }

    /// <summary>
    /// Ends an expression.
    /// </summary>
    public override void EndExpression()
    {
    }

    /// <inheritdoc/>
    public override void Close()
    {
        this.inner.Close();
    }

    /// <inheritdoc/>
    public override ValueTask CloseAsync(CancellationToken cancellationToken = default)
    {
        this.Close();
        return default;
    }

    private static string EscapeComment(string source)
    {
        return source.Replace("(", "\\x28").Replace(")", "\\x29").Replace("\r", "\\x0d").Replace("\n", "\\x0a");
    }

    private void PushState(WriterState state)
    {
        if (!this.CanPushState(state))
        {
            throw new GCodeTextWriterException($"Unable to change writer state from {this.CurrentState} to {state}.");
        }

        this.stateStack.Push(state);
    }

    private bool CanPushState(WriterState state)
    {
        switch (this.CurrentState)
        {
            case WriterState.None:
                return state == WriterState.File;
            case WriterState.File:
                return state == WriterState.Line;
            case WriterState.Line:
                return state == WriterState.Word;
        }

        return false;
    }
}