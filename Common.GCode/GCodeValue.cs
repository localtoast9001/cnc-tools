//-----------------------------------------------------------------------
// <copyright file="GCodeValue.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.GCode;

/// <summary>
/// Abstract value associated with a <see cref="GCodeWordSegment"/>.
/// </summary>
/// <remarks>
/// The design is left open to add expressions and parameter references in the future.
/// </remarks>
public abstract class GCodeValue
{
    /// <summary>
    /// Writes the value to the given writer.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public abstract void WriteTo(IO.GCodeWriter writer);
}