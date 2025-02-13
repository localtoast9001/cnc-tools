//-----------------------------------------------------------------------
// <copyright file="GCodeSegment.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CncTools.GCode;

/// <summary>
/// Abstract segment inside a <see cref="GCodeLine"/>.
/// </summary>
/// <remarks>
/// The design is left open to include parameter setting and other dialects in the future.
/// </remarks>
public abstract class GCodeSegment
{
    /// <summary>
    /// Writes the segment content to the given writer.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public abstract void WriteTo(IO.GCodeWriter writer);
}