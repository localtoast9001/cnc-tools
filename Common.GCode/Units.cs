//-----------------------------------------------------------------------
// <copyright file="Units.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.GCode;

/// <summary>
/// Describes units to be used in value interpretation.
/// </summary>
public enum Units
{
    /// <summary>
    /// Values are expressed in millimeters.
    /// </summary>
    Millimeters = 0,

    /// <summary>
    /// Values are expressed in inches.
    /// </summary>
    Inches,
}
