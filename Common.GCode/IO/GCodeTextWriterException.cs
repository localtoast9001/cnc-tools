//-----------------------------------------------------------------------
// <copyright file="GCodeTextWriterException.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.GCode.IO;

using System.Runtime.Serialization;

/// <summary>
/// Exception thrown on usage errors from <see cref="GCodeTextWriter"/>.
/// </summary>
[Serializable]
public class GCodeTextWriterException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeTextWriterException"/> class.
    /// </summary>
    public GCodeTextWriterException()
    : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeTextWriterException"/> class.
    /// </summary>
    /// <param name="message">The text message.</param>
    public GCodeTextWriterException(string message)
    : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeTextWriterException"/> class.
    /// </summary>
    /// <param name="message">The text message.</param>
    /// <param name="innerException">Optional inner exception.</param>
    public GCodeTextWriterException(string message, Exception? innerException)
    : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeTextWriterException"/> class.
    /// </summary>
    /// <param name="info">Serialization info.</param>
    /// <param name="context">Streaming context.</param>
    protected GCodeTextWriterException(SerializationInfo info, StreamingContext context)
    : base(info, context)
    {
    }
}