//-----------------------------------------------------------------------
// <copyright file="PowerFunction.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CncTools.Image2GCode;

using SkiaSharp;

/// <summary>
/// Abstract base class for power functions.
/// </summary>
public abstract class PowerFunction
{
    /// <summary>
    /// Gets the power output between 0 and 1 for the given color and coordinates.
    /// </summary>
    /// <param name="color">The color of the image at the point.</param>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <returns>The power output between 0 and 1.</returns>
    public abstract float GetPower(SKColor color, int x, int y);

    /// <summary>
    /// Gets the grayscale intensity of the given color.
    /// </summary>
    /// <param name="color">The color.</param>
    /// <returns>The grayscale intensity between 0 and 1.</returns>
    protected static float GetLuminance(SKColor color)
    {
        // Use the standard formula for luminance: 0.299 * R + 0.587 * G + 0.114 * B
        return ((0.299f * color.Red) + (0.587f * color.Green) + (0.114f * color.Blue)) / 255f;
    }
}