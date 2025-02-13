//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CncTools.Image2GCode;

using System.CommandLine;
using CncTools.GCode;
using SkiaSharp;

/// <summary>
/// Main program.
/// </summary>
internal class Program
{
    /// <summary>
    /// Command line handler to convert the image.
    /// </summary>
    /// <param name="outputFile">Output command line.</param>
    /// <param name="inputFile">Input command line.</param>
    /// <param name="scanOutputFile">Optional file to write with the intermediate scan output.</param>
    /// <param name="width">Optional width in mm.</param>
    /// <param name="height">Optional height in mm.</param>
    /// <param name="feed">Optional feed rate.</param>
    /// <param name="power">Optional max laser power.</param>
    /// <param name="resolution">Optional resolution in scan lines/mm.</param>
    /// <returns>A <see cref="Task"/> for the async operation.</returns>
    internal static async Task ConvertImageAsync(
        FileInfo? outputFile,
        FileInfo? inputFile,
        FileInfo? scanOutputFile,
        float? width,
        float? height,
        float? feed,
        int? power,
        float? resolution)
    {
        SKImage inputImage = SKImage.FromEncodedData(inputFile!.FullName);
        Console.WriteLine($"Loaded image w={inputImage.Width}, h={inputImage.Height}.");
        float aspectRatio = inputImage.Height / inputImage.Width;
        float scale = 50f;
        if (width != null)
        {
            scale = width.Value;
        }

        if (height != null)
        {
            scale = height.Value / aspectRatio;
        }

        GCodeDocument outputDoc = new();
        ScanConverter converter = new(inputImage, outputDoc, scale);
        if (feed.HasValue)
        {
            converter.FeedRate = feed.Value;
        }

        if (power.HasValue)
        {
            converter.MaxPower = power.Value;
        }

        if (resolution.HasValue)
        {
            converter.Resolution = resolution.Value;
        }

        await Task.Run(() => converter.Convert());
        using FileStream fs = outputFile!.Create();
        using GCode.IO.GCodeWriter writer = GCode.IO.GCodeWriter.Create(fs);
        outputDoc.WriteTo(writer);
        await fs.FlushAsync();

        if (scanOutputFile != null)
        {
            SKImage scanImage = SKImage.FromBitmap(converter.NormalizedInput);
            SKData data = scanImage.Encode();
            using var scanOut = scanOutputFile.Create();
            data.SaveTo(scanOut);
            await scanOut.FlushAsync();
        }
    }

    private static async Task<int> Main(string[] args)
    {
        var inputFileArgument = new Argument<FileInfo?>(
            name: "input",
            description: "The input image file.");
        var outputFileOption = new Option<FileInfo?>(
            name: "--output",
            description: "The output gcode file.");
        outputFileOption.AddAlias("-o");
        outputFileOption.IsRequired = true;
        var scanOutputFileOption = new Option<FileInfo?>(
            name: "--scan-output",
            description: "Scan converted output image.");
        var widthOption = new Option<float?>(
            name: "--width",
            description: "Width of the output in mm.");
        widthOption.AddAlias("-w");
        var heightOption = new Option<float?>(
            name: "--height",
            description: "Height of the output in mm.");
        heightOption.AddAlias("-h");
        var feedOption = new Option<float?>(
            name: "--feed",
            description: "Set the feed rate in mm/min.");
        feedOption.AddAlias("-f");
        var powerOption = new Option<int?>(
            name: "--power",
            description: "Set the max power for the laser from 0-1000");
        powerOption.AddAlias("-p");
        var resOption = new Option<float?>(
            name: "--resolution",
            description: "Set the resolution in mm/scan line.");
        resOption.AddAlias("-r");
        var levelsOption = new Option<int?>(
            name: "--levels",
            description: "Number of color levels to use.");
        levelsOption.AddAlias("-l");
        var rootCommand = new RootCommand("Converts an image file to GCode for laser engraving.");
        rootCommand.AddOption(outputFileOption);
        rootCommand.AddOption(scanOutputFileOption);
        rootCommand.AddOption(widthOption);
        rootCommand.AddOption(heightOption);
        rootCommand.AddOption(feedOption);
        rootCommand.AddOption(powerOption);
        rootCommand.AddOption(resOption);
        rootCommand.AddOption(levelsOption);
        rootCommand.AddArgument(inputFileArgument);

        rootCommand.SetHandler(
            (output, input, scanOutput, width, height, feed, power, res) => ConvertImageAsync(output, input, scanOutput, width, height, feed, power, res),
            outputFileOption,
            inputFileArgument,
            scanOutputFileOption,
            widthOption,
            heightOption,
            feedOption,
            powerOption,
            resOption);

        return await rootCommand.InvokeAsync(args);
    }
}