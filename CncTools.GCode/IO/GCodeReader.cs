//-----------------------------------------------------------------------
// <copyright file="GCodeReader.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CncTools.GCode.IO;

/// <summary>
/// The type of token read by the <see cref="GCodeReader"/>.
/// </summary>
public enum GCodeTokenType
{
    /// <summary>
    /// No value or uninitialized.
    /// </summary>
    None = 0,

    /// <summary>
    /// Start of file delimiter.
    /// </summary>
    FileStart,

    /// <summary>
    /// End of file delimiter.
    /// </summary>
    FileEnd,

    /// <summary>
    /// Start of a line.
    /// </summary>
    LineStart,

    /// <summary>
    /// End of a line.
    /// </summary>
    LineEnd,

    /// <summary>
    /// A comment or message.
    /// </summary>
    CommentOrMessage,

    /// <summary>
    /// The start of a word.
    /// </summary>
    WordStart,

    /// <summary>
    /// The end of a word.
    /// </summary>
    WordEnd,

    /// <summary>
    /// A value.
    /// </summary>
    Value,
}

/// <summary>
/// Describes the type of value read.
/// </summary>
public enum GCodeValueType
{
    /// <summary>
    /// Not a value type.
    /// </summary>
    None = 0,

    /// <summary>
    /// The value is an integer.
    /// </summary>
    Integer = 1,

    /// <summary>
    /// The value is a decimal.
    /// </summary>
    Decimal = 2,
}

/// <summary>
/// Structured reader for GCode/RS274/NGC files.
/// </summary>
/// <seealso href="https://www.nist.gov/publications/nist-rs274ngc-interpreter-version-3?pub_id=823374"/>
public abstract class GCodeReader : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeReader"/> class.
    /// </summary>
    /// <param name="settings">Optional settings used for the reader.</param>
    protected GCodeReader(GCodeReaderSettings? settings = null)
    {
        this.Settings = settings?.AsReadOnly();
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="GCodeReader"/> class.
    /// </summary>
    ~GCodeReader()
    {
        this.Dispose(false);
    }

    /// <summary>
    /// Gets the settings used to create this instance.
    /// </summary>
    public virtual GCodeReaderSettings? Settings { get; }

    /// <summary>
    /// Gets the type of the current token in the reader.
    /// </summary>
    public abstract GCodeTokenType TokenType { get; }

    /// <summary>
    /// Gets the decimal value of the current value token.
    /// </summary>
    public abstract decimal Value { get; }

    /// <summary>
    /// Gets the type of value of the current value token.
    /// </summary>
    public abstract GCodeValueType ValueType { get; }

    /// <summary>
    /// Gets a value indicating whether or not the current line has been marked for block delete.
    /// </summary>
    public abstract bool IsBlockDeleteLine { get; }

    /// <summary>
    /// Gets the current line number if specified.
    /// </summary>
    /// <remarks>
    /// This value is less than zero if no line number is specified.
    /// </remarks>
    public abstract int LineNumber { get; }

    /// <summary>
    /// Gets the letter at the start of the current word.
    /// </summary>
    public abstract Code Code { get; }

    /// <summary>
    /// Gets the text of the current comment.
    /// </summary>
    public abstract string? Comment { get; }

    /// <summary>
    /// Gets a value indicating whether to close the input.
    /// </summary>
    protected bool CloseInput => this.Settings?.CloseInput ?? true;

    /// <summary>
    /// Creates a reader using the given file as input.
    /// </summary>
    /// <param name="path">The path to the input file.</param>
    /// <param name="settings">Optional reader settings.</param>
    /// <returns>
    /// A new instance of the <see cref="GCodeReader"/> class.
    /// </returns>
    public static GCodeReader Create(string path, GCodeReaderSettings? settings = null)
    {
        GCodeReaderSettings newSettings = settings ?? new GCodeReaderSettings() { CloseInput = true };
        return new GCodeTextReader(new StreamReader(path, System.Text.Encoding.ASCII, false, 512), newSettings);
    }

    /// <summary>
    /// Creates a reader using the given stream as input.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    /// <param name="settings">Optional reader settings.</param>
    /// <returns>
    /// A new instance of the <see cref="GCodeReader"/> class.
    /// </returns>
    public static GCodeReader Create(Stream stream, GCodeReaderSettings? settings = null)
    {
        GCodeReaderSettings newSettings = settings ?? new GCodeReaderSettings() { CloseInput = true };
        return new GCodeTextReader(new StreamReader(stream, System.Text.Encoding.ASCII, false, 512, leaveOpen: !newSettings.CloseInput), newSettings);
    }

    /// <summary>
    /// Creates a reader using the given text reader as input.
    /// </summary>
    /// <param name="reader">The input text reader.</param>
    /// <param name="settings">Optional reader settings.</param>
    /// <returns>
    /// A new instance of the <see cref="GCodeReader"/> class.
    /// </returns>
    public static GCodeReader Create(TextReader reader, GCodeReaderSettings? settings = null)
    {
        return new GCodeTextReader(reader, settings);
    }

    /// <summary>
    /// Closes the current reader and releases any associated system resources.
    /// </summary>
    public abstract void Close();

    /// <summary>
    /// Closes the current reader and releases any associated system resources.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="ValueTask"/> for the async operation.</returns>
    public abstract ValueTask CloseAsync(CancellationToken cancellationToken = default);

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
    /// Advances the reader to the next token in the input stream.
    /// </summary>
    /// <returns>
    /// <c>True</c> if a token was available to read; otherwise, <c>false</c> of end of stream.
    /// </returns>
    public abstract bool Read();

    /// <summary>
    /// Releases the unmanaged resources used by this instance and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing && this.CloseInput)
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
        return this.CloseInput ?
            this.CloseAsync() :
            default;
    }
}