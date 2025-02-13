//-----------------------------------------------------------------------
// <copyright file="ScanConverter.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CncTools.Image2GCode;

using System.Reflection;
using CncTools.GCode;
using SkiaSharp;

/// <summary>
/// Processes an image and converts it to GCode output.
/// </summary>
internal class ScanConverter
{
    /*
    private static readonly float[] GamutLevels = new[]
    {
        1.0f,
        0.875f,
        0.75f,
        0.625f,
        0.5f,
        0.375f,
        0.25f,
        0f,
    };
    */

    private static readonly float[] GamutLevels = new[]
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

    /// <summary>
    /// Initializes a new instance of the <see cref="ScanConverter"/> class.
    /// </summary>
    /// <param name="input">The input image.</param>
    /// <param name="output">The output document.</param>
    /// <param name="scale">The image scale for output.</param>
    public ScanConverter(
        SKImage input,
        GCodeDocument output,
        float scale = 50.0f)
    {
        this.Input = input;
        this.Output = output;
        this.Width = scale;
        this.Height = input.Height * scale / input.Width;
    }

    /// <summary>
    /// Gets the input image.
    /// </summary>
    public SKImage Input { get; }

    /// <summary>
    /// Gets the output document.
    /// </summary>
    public GCodeDocument Output { get; }

    /// <summary>
    /// Gets the width of the image output in mm.
    /// </summary>
    public float Width { get; }

    /// <summary>
    /// Gets the height of the image output in mm.
    /// </summary>
    public float Height { get; }

    /// <summary>
    /// Gets or sets the resolution of the output in mm/scan line.
    /// </summary>
    public float Resolution { get; set; } = 0.1f;

    /// <summary>
    /// Gets or sets the feed rate in mm/min.
    /// </summary>
    public float FeedRate { get; set; } = 1000f;

    /// <summary>
    /// Gets or sets the max laser power.
    /// </summary>
    public int MaxPower { get; set; } = 1000;

    /// <summary>
    /// Gets the normalized input converted to scan rows.
    /// </summary>
    public SKBitmap? NormalizedInput { get; private set; }

    /// <summary>
    /// Converts the image to GCode.
    /// </summary>
    public void Convert()
    {
        this.InitNormalizedImage();
        this.Output.Lines.Add(GCodeLine.Comment($"Image dimensions: w={this.Input.Width}px, h={this.Input.Height}px."));
        this.Output.Lines.Add(GCodeLine.Comment($"Output dimenions: x={this.Width}mm, y={this.Height}mm, res={this.Resolution}mm/pt."));
        this.Output.Lines.Add(GCodeLine.Comment($"Options: feedrate={this.FeedRate}mm/min, maxpower={this.MaxPower}."));
        this.Output.Lines.Add(new GCodeLine(GCodeWordSegment.FromCommand(CommandCode.AbsoluteMode)));
        this.Output.Lines.Add(new GCodeLine(GCodeWordSegment.FromCommand(CommandCode.MillimetersSelection)));

        // Stop the laser.
        this.Output.Lines.Add(new GCodeLine(GCodeWordSegment.FromCommand(CommandCode.SpindleStop)));

        for (int y = 0; y < this.NormalizedInput!.Height; y++)
        {
            ScanSegment? prevSegment = null;
            foreach (var segment in this.ScanLine(y).Where(e => e.Power > 0))
            {
                bool continuation = prevSegment != null && prevSegment.End == segment.Start;
                if (continuation)
                {
                    this.Output.Lines.Add(new GCodeLine(new GCodeWordSegment(Code.S, segment.Power)));
                }
                else
                {
                    this.Output.Lines.Add(new GCodeLine(GCodeWordSegment.FromCommand(CommandCode.SpindleStop)));
                    this.Output.Lines.Add(new GCodeLine(
                        GCodeWordSegment.FromCommand(CommandCode.RapidPosition),
                        new GCodeWordSegment(Code.X, (decimal)(segment.Start * this.Resolution)),
                        new GCodeWordSegment(Code.Y, (decimal)((this.NormalizedInput.Height - y) * this.Resolution))));

                    this.Output.Lines.Add(new GCodeLine(
                        GCodeWordSegment.FromCommand(CommandCode.SpindleOnClockwise),
                        new GCodeWordSegment(Code.S, segment.Power)));
                }

                this.Output.Lines.Add(new GCodeLine(
                    GCodeWordSegment.FromCommand(CommandCode.LinearInterpretation),
                    new GCodeWordSegment(Code.X, (decimal)(segment.End * this.Resolution)),
                    new GCodeWordSegment(Code.F, (decimal)this.FeedRate)));

                prevSegment = segment;
            }
        }

        // Stop the laser.
        this.Output.Lines.Add(new GCodeLine(GCodeWordSegment.FromCommand(CommandCode.SpindleStop)));

        this.Output.Lines.Add(new GCodeLine(GCodeWordSegment.FromCommand(CommandCode.EndOfProgramAndReset)));
    }

    private IEnumerable<ScanSegment> ScanLine(int y)
    {
        int power = 0;
        int startX = 0;
        for (int x = 0; x < this.NormalizedInput!.Width; x++)
        {
            int p = this.GetPower(x, y);

            if (power != p)
            {
                yield return new ScanSegment
                {
                    Start = startX,
                    End = x,
                    Power = power,
                };

                power = p;
                startX = x;
            }
        }

        yield return new ScanSegment
        {
            Start = startX,
            End = this.NormalizedInput!.Width,
            Power = power,
        };
    }

    private void InitNormalizedImage()
    {
        int normWidth = (int)(this.Width / this.Resolution);
        int normHeight = (int)(this.Height / this.Resolution);
        this.NormalizedInput = new SKBitmap(
            normWidth,
            normHeight,
            SKColorType.Gray8,
            SKAlphaType.Opaque);
        using SKCanvas canvas = new SKCanvas(this.NormalizedInput);
        canvas.Clear(new SKColor(0xff, 0xff, 0xff));
        canvas.DrawImage(
            this.Input,
            new SKRect(0, 0, normWidth, normHeight));
    }

    private int GetPower(int x, int y)
    {
        SKColor color = this.NormalizedInput!.GetPixel(
            x,
            y);
        return this.Gamut(color);
    }

    private int Gamut(SKColor color)
    {
        int index = color.Red >> 5; // 3-bit 0-7
        return (int)(GamutLevels[index] * this.MaxPower);
    }

    private class ScanSegment
    {
        public int Start { get; set; }

        public int End { get; set; }

        public int Power { get; set; }
    }
}