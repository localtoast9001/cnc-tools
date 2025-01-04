//-----------------------------------------------------------------------
// <copyright file="GCodeWriterSettings.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.GCode.IO;

/// <summary>
/// Settings used to control behavior of the <see cref="GCodeWriter"/>.
/// </summary>
public class GCodeWriterSettings : ICloneable
{
    /// <summary>
    /// The default max line number supported by the standard.
    /// </summary>
    /// <seealso href="https://www.nist.gov/publications/nist-rs274ngc-interpreter-version-3?pub_id=823374"/>
    public const int DefaultMaxLineNumber = 99999;

    private readonly bool isReadOnly;
    private bool @async;
    private bool closeOutput;
    private int maxLineNumber = DefaultMaxLineNumber;

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeWriterSettings"/> class.
    /// </summary>
    public GCodeWriterSettings()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeWriterSettings"/> class.
    /// </summary>
    /// <param name="source">The source settings to copy.</param>
    /// <param name="isReadOnly"><c>True</c> to create a read-only copy; otherwise, <c>false</c>.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    public GCodeWriterSettings(
        GCodeWriterSettings source,
        bool isReadOnly = false)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        this.@async = source.Async;
        this.closeOutput = source.CloseOutput;
        this.maxLineNumber = source.MaxLineNumber;
        this.isReadOnly = isReadOnly;
    }

    /// <summary>
    /// Gets or sets a value indicating whether asynchronous methods can be used on a particular <see cref="GCodeReader"/> instance.
    /// </summary>
    public bool Async
    {
        get
        {
            return this.@async;
        }

        set
        {
            this.ThrowOnReadOnly();
            this.@async = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the underlying stream or <see cref="TextWriter"/> should be closed when the writer is closed.
    /// </summary>
    public bool CloseOutput
    {
        get
        {
            return this.closeOutput;
        }

        set
        {
            this.ThrowOnReadOnly();
            this.closeOutput = value;
        }
    }

    /// <summary>
    /// Gets or sets the maximum line number that could be specified for a line.
    /// </summary>
    public int MaxLineNumber
    {
        get
        {
            return this.maxLineNumber;
        }

        set
        {
            this.ThrowOnReadOnly();
            this.maxLineNumber = value;
        }
    }

    /// <summary>
    /// Gets a value indicating whether this instance is read only.
    /// </summary>
    public bool IsReadOnly => this.isReadOnly;

    /// <inheritdoc/>
    public object Clone()
    {
        return new GCodeWriterSettings(this);
    }

    /// <summary>
    /// Gets the current instance as a read-only instance.
    /// </summary>
    /// <returns>The current instance if it is read-only or a read-only copy.</returns>
    public GCodeWriterSettings AsReadOnly() =>
        this.IsReadOnly ?
            this :
            new GCodeWriterSettings(this, true);

    private void ThrowOnReadOnly()
    {
        if (this.IsReadOnly)
        {
            throw new InvalidOperationException("This instance is read-only.");
        }
    }
}