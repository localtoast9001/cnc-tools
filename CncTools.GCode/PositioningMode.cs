//-----------------------------------------------------------------------
// <copyright file="PositioningMode.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CncTools.GCode;

/// <summary>
/// Describes how the machine should interpret positioning in the commands.
/// </summary>
public enum PositioningMode
{
    /// <summary>
    /// Coordinates are relative to the current position.
    /// </summary>
    Relative = 0,

    /// <summary>
    /// Coordinates are absolute positions.
    /// </summary>
    Absolute,
}
