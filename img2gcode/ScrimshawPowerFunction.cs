//-----------------------------------------------------------------------
// <copyright file="ScrimshawPowerFunction.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CncTools.Image2GCode;

using SkiaSharp;

/// <summary>
/// Represents a power function for scrimshaw engraving.
/// </summary>
public class ScrimshawPowerFunction : PowerFunction
{
    /// <summary>
    /// Calculates the power for scrimshaw engraving based on the color and position.
    /// </summary>
    /// <param name="color">The color of the pixel.</param>
    /// <param name="x">The x-coordinate of the pixel.</param>
    /// <param name="y">The y-coordinate of the pixel.</param>
    /// <returns>The calculated power value.</returns>
    public override float GetPower(SKColor color, int x, int y)
    {
        float gray = GetLuminance(color);
        switch (y % 2)
        {
            case 0:
                return gray < 0.25f ? 1.0f : 0.0f;
            default:
                return gray < 0.75f ? 1.0f : 0.0f;
        }
    }
}