//-----------------------------------------------------------------------
// <copyright file="GCodeReaderSettings.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CncTools.GCode.IO;

/// <summary>
/// Settings used to control behavior of the <see cref="GCodeReader"/>.
/// </summary>
public class GCodeReaderSettings : ICloneable
{
    private readonly bool isReadOnly;
    private bool @async;
    private bool closeInput;
    private bool ignoreComments;
    private bool ignoreBlockDelete;

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeReaderSettings"/> class.
    /// </summary>
    public GCodeReaderSettings()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeReaderSettings"/> class.
    /// </summary>
    /// <param name="source">The source settings to copy.</param>
    /// <param name="isReadOnly"><c>true</c> to create a read-only copy; otherwise, <c>false</c>.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    public GCodeReaderSettings(GCodeReaderSettings source, bool isReadOnly = false)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        this.@async = source.Async;
        this.closeInput = source.CloseInput;
        this.ignoreComments = source.IgnoreComments;
        this.ignoreBlockDelete = source.IgnoreBlockDelete;
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
    /// Gets or sets a value indicating whether the underlying stream or <see cref="TextReader"/> should be closed when the reader is closed.
    /// </summary>
    public bool CloseInput
    {
        get
        {
            return this.closeInput;
        }

        set
        {
            this.ThrowOnReadOnly();
            this.closeInput = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether comments should be ignored and skipped when reading tokens.
    /// </summary>
    public bool IgnoreComments
    {
        get
        {
            return this.ignoreComments;
        }

        set
        {
            this.ThrowOnReadOnly();
            this.ignoreComments = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether lines marked with block delete should be ignored when reading.
    /// </summary>
    public bool IgnoreBlockDelete
    {
        get
        {
            return this.ignoreBlockDelete;
        }

        set
        {
            this.ThrowOnReadOnly();
            this.ignoreBlockDelete = value;
        }
    }

    /// <summary>
    /// Gets a value indicating whether this instance is read-only.
    /// </summary>
    public bool IsReadOnly => this.isReadOnly;

    /// <inheritdoc/>
    public object Clone()
    {
        return new GCodeReaderSettings(this);
    }

    /// <summary>
    /// Gets the current instance as a read-only instance.
    /// </summary>
    /// <returns>The current instance if it is read-only or a read-only copy.</returns>
    public GCodeReaderSettings AsReadOnly() =>
        this.IsReadOnly ?
            this :
            new GCodeReaderSettings(this, true);

    private void ThrowOnReadOnly()
    {
        if (this.IsReadOnly)
        {
            throw new InvalidOperationException("This instance is read-only.");
        }
    }
}