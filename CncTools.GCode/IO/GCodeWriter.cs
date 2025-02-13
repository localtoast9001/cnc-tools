//-----------------------------------------------------------------------
// <copyright file="GCodeWriter.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CncTools.GCode.IO;

/// <summary>
/// Structured writer for GCode/RS274/NGC files.
/// </summary>
/// <seealso href="https://www.nist.gov/publications/nist-rs274ngc-interpreter-version-3?pub_id=823374"/>
public abstract class GCodeWriter : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeWriter"/> class.
    /// </summary>
    /// <param name="settings">The optional settings to use for the writer.</param>
    protected GCodeWriter(GCodeWriterSettings? settings = null)
    {
        this.Settings = settings?.AsReadOnly();
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="GCodeWriter"/> class.
    /// </summary>
    ~GCodeWriter()
    {
        this.Dispose(false);
    }

    /// <summary>
    /// Gets the settings used to create this instance.
    /// </summary>
    public virtual GCodeWriterSettings? Settings { get; }

    /// <summary>
    /// Gets a value indicating whether to close the underlying output on close.
    /// </summary>
    protected bool CloseOutput => this.Settings?.CloseOutput ?? false;

    /// <summary>
    /// Gets the maximum custom line number allowed to be output.
    /// </summary>
    protected int MaxLineNumber => this.Settings?.MaxLineNumber ?? GCodeWriterSettings.DefaultMaxLineNumber;

    /// <summary>
    /// Creates a writer to write to an output file.
    /// </summary>
    /// <param name="path">The path to the output file.</param>
    /// <param name="settings">Optional writer settings.</param>
    /// <returns>
    /// A new instance of the <see cref="GCodeWriter"/> class.
    /// </returns>
    public static GCodeWriter Create(string path, GCodeWriterSettings? settings = null)
    {
        GCodeWriterSettings newSettings = settings ?? new GCodeWriterSettings() { CloseOutput = true };
        return new GCodeTextWriter(new StreamWriter(path, false, System.Text.Encoding.ASCII, 512), newSettings);
    }

    /// <summary>
    /// Creates a writer to write to the given stream.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="settings">Optional writer settings.</param>
    /// <returns>
    /// A new instance of the <see cref="GCodeWriter"/> class.
    /// </returns>
    public static GCodeWriter Create(Stream stream, GCodeWriterSettings? settings = null)
    {
        GCodeWriterSettings newSettings = settings ?? new GCodeWriterSettings() { CloseOutput = true };
        return new GCodeTextWriter(new StreamWriter(stream, System.Text.Encoding.ASCII, 512, leaveOpen: !newSettings.CloseOutput), newSettings);
    }

    /// <summary>
    /// Creates a writer to write to the given text writer.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="settings">Optional writer settings.</param>
    /// <returns>
    /// A new instance of the <see cref="GCodeWriter"/> class.
    /// </returns>
    public static GCodeWriter Create(TextWriter writer, GCodeWriterSettings? settings = null)
    {
        return new GCodeTextWriter(writer, settings);
    }

    /// <summary>
    /// Releases all resources used by this instance.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Asynchronously releases all resources used by this instance.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> that represents the async dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        await this.DisposeAsyncCore();
        this.Dispose(false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Closes the current writer and releases any associated system resources.
    /// </summary>
    public abstract void Close();

    /// <summary>
    /// Closes the current writer and releases any associated system resources.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="ValueTask"/> for the async operation.</returns>
    public abstract ValueTask CloseAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts a file.
    /// </summary>
    public abstract void StartFile();

    /// <summary>
    /// Ends a file.
    /// </summary>
    public abstract void EndFile();

    /// <summary>
    /// Starts a line.
    /// </summary>
    /// <param name="lineNumber">Optional application defined line number.</param>
    /// <param name="blockDelete"><c>true</c> if this line is block deleted; otherwise <c>false</c>.</param>
    public abstract void StartLine(int lineNumber = -1, bool blockDelete = false);

    /// <summary>
    /// Ends the current line.
    /// </summary>
    /// <param name="endComment">Optional end of line comment.</param>
    public abstract void EndLine(string? endComment = null);

    /// <summary>
    /// Starts a new word on the current line.
    /// </summary>
    /// <param name="code">The code letter to start the word.</param>
    public abstract void StartWord(Code code);

    /// <summary>
    /// Ends the current word.
    /// </summary>
    public abstract void EndWord();

    /// <summary>
    /// Writes a comment.
    /// </summary>
    /// <param name="comment">The comment text.</param>
    public abstract void WriteComment(string comment);

    /// <summary>
    /// Writes a comment formatted as a message.
    /// </summary>
    /// <param name="message">The message text.</param>
    public abstract void WriteMessage(string message);

    /// <summary>
    /// Writes an integer value.
    /// </summary>
    /// <param name="value">The value.</param>
    public abstract void WriteValue(int value);

    /// <summary>
    /// Writes a single precision value.
    /// </summary>
    /// <param name="value">The value.</param>
    public abstract void WriteValue(float value);

    /// <summary>
    /// Writes a double precision value.
    /// </summary>
    /// <param name="value">The value.</param>
    public abstract void WriteValue(double value);

    /// <summary>
    /// Writes a decimal value.
    /// </summary>
    /// <param name="value">The value.</param>
    public abstract void WriteValue(decimal value);

    /// <summary>
    /// Starts setting a parameter.
    /// </summary>
    public abstract void StartParameterSetting();

    /// <summary>
    /// Ends setting a parameter.
    /// </summary>
    public abstract void EndParameterSetting();

    /// <summary>
    /// Starts an expression.
    /// </summary>
    public abstract void StartExpression();

    /// <summary>
    /// Ends an expression.
    /// </summary>
    public abstract void EndExpression();

    /// <summary>
    /// Writes a word with an integer value.
    /// </summary>
    /// <param name="code">The code letter.</param>
    /// <param name="value">The integer value.</param>
    public virtual void WriteWord(Code code, int value)
    {
        this.StartWord(code);
        this.WriteValue(value);
        this.EndWord();
    }

    /// <summary>
    /// Writes a word with a single precision value.
    /// </summary>
    /// <param name="code">The code letter.</param>
    /// <param name="value">The single precision value.</param>
    public virtual void WriteWord(Code code, float value)
    {
        this.StartWord(code);
        this.WriteValue(value);
        this.EndWord();
    }

    /// <summary>
    /// Writes a word with a double precision value.
    /// </summary>
    /// <param name="code">The code letter.</param>
    /// <param name="value">The double precision value.</param>
    public virtual void WriteWord(Code code, double value)
    {
        this.StartWord(code);
        this.WriteValue(value);
        this.EndWord();
    }

    /// <summary>
    /// Releases the unmanaged resources used by this instance and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing && this.CloseOutput)
        {
            this.Close();
        }
    }

    /// <summary>
    /// Asynchronously releases managed resources used by the TextWriter object.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> for the core async dispose operation.</returns>
    protected virtual ValueTask DisposeAsyncCore()
    {
        return this.CloseOutput ?
            this.CloseAsync() :
            default;
    }
}