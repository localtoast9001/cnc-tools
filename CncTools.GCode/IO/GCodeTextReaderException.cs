//-----------------------------------------------------------------------
// <copyright file="GCodeTextReaderException.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CncTools.GCode.IO;

using System.Runtime.Serialization;

/// <summary>
/// Exception thrown on input errors inside <see cref="GCodeTextReader"/>.
/// </summary>
[Serializable]
public class GCodeTextReaderException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeTextReaderException"/> class.
    /// </summary>
    public GCodeTextReaderException()
    : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeTextReaderException"/> class.
    /// </summary>
    /// <param name="message">The text message.</param>
    public GCodeTextReaderException(string message)
    : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeTextReaderException"/> class.
    /// </summary>
    /// <param name="message">The text message.</param>
    /// <param name="innerException">Optional inner exception.</param>
    public GCodeTextReaderException(string message, Exception? innerException)
    : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeTextReaderException"/> class.
    /// </summary>
    /// <param name="info">Serialization info.</param>
    /// <param name="context">Streaming context.</param>
    protected GCodeTextReaderException(SerializationInfo info, StreamingContext context)
    : base(info, context)
    {
    }
}