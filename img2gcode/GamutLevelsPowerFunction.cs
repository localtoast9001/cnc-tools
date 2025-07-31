// <copyright file="GamutLevelsPowerFunction.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
namespace CncTools.Image2GCode;

using SkiaSharp;

/// <summary>
/// Represents a power function that maps colors to power levels based on an input gamut.
/// </summary>
public class GamutLevelsPowerFunction : PowerFunction
{
    private static readonly float[] DefaultGamutLevels = new[]
    {
        1.0f,
        1f,
        1f,
        0f,
        0f,
        0f,
        0f,
        0f,
    };

    private readonly float[] levels;

    /// <summary>
    /// Initializes a new instance of the <see cref="GamutLevelsPowerFunction"/> class.
    /// </summary>
    /// <param name="levels">The gamut levels.</param>
    public GamutLevelsPowerFunction(float[] levels)
    {
        if (levels == null)
        {
            throw new ArgumentNullException(nameof(levels));
        }

        this.levels = new float[levels.Length];
        Array.Copy(levels, this.levels, levels.Length);
    }

    /// <summary>
    /// Gets the default instance of the <see cref="GamutLevelsPowerFunction"/> class with predefined gamut levels.
    /// </summary>
    public static GamutLevelsPowerFunction Default { get; } = new(DefaultGamutLevels);

    /// <inheritdoc/>
    public override float GetPower(SKColor color, int x, int y)
    {
        // Convert the color to a grayscale value
        float gray = GetLuminance(color);

        // Map the grayscale value to the gamut levels
        int index = (int)(gray * (this.levels.Length - 1));
        return this.levels[index];
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Gamut: [{string.Join(", ", this.levels)}]";
    }
}