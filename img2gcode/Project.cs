// <copyright file="Project.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace CncTools.Image2GCode;

using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Represents a project to keep settings in a saved project file.
/// </summary>
public class Project
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        AllowTrailingCommas = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
    };

    /// <summary>
    /// Gets or sets the input image file.
    /// </summary>
    public string InputFile { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the output GCode file.
    /// </summary>
    public string OutputFile { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional intermediate scan output file.
    /// </summary>
    public string? ScanOutputFile { get; set; }

    /// <summary>
    /// Gets or sets the width of the output in mm.
    /// </summary>
    public float? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the output in mm.
    /// </summary>
    public float? Height { get; set; }

    /// <summary>
    /// Gets or sets the feed rate in mm/min.
    /// </summary>
    public float? Feed { get; set; }

    /// <summary>
    /// Gets or sets the maximum laser power between 0-1000.
    /// </summary>
    public int? Power { get; set; }

    /// <summary>
    /// Gets or sets the resolution of the output in mm/scan line.
    /// </summary>
    public float? Resolution { get; set; }

    /// <summary>
    /// Gets or sets the gamut levels for the image.
    /// </summary>
    public float[]? Levels { get; set; }

    /// <summary>
    /// Gets or sets the power function to determine the power levels for the image.
    /// </summary>
    public string? PowerFunction { get; set; }

    /// <summary>
    /// Gets the path to the project file.
    /// </summary>
    [JsonIgnore]
    public string Path { get; private set; } = string.Empty;

    /// <summary>
    /// Loads a <see cref="Project"/> instance from a JSON file.
    /// </summary>
    /// <param name="path">The path to the JSON file containing the project data.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Project"/> instance populated with data from the JSON file.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the specified file does not exist.</exception>
    public static async Task<Project> LoadAsync(string path, CancellationToken cancellationToken = default)
    {
        string jsonContent = await File.ReadAllTextAsync(path, cancellationToken);

        Project result = JsonSerializer.Deserialize<Project>(jsonContent, JsonOptions) ??
            throw new InvalidOperationException($"Failed to deserialize project from {path}.");
        result.Path = path;
        return result;
    }

    /// <summary>
    /// Gets a <see cref="FileInfo"/> object for the output file by combining it with the project path.
    /// </summary>
    /// <returns>A <see cref="FileInfo"/> object representing the output file.</returns>
    public FileInfo GetOutputFileInfo()
    {
        if (string.IsNullOrEmpty(this.Path) || string.IsNullOrEmpty(this.OutputFile))
        {
            throw new InvalidOperationException("Path or OutputFile is not set.");
        }

        return this.ResolveProjectRelativePath(this.OutputFile);
    }

    /// <summary>
    /// Gets a <see cref="FileInfo"/> object for the input file by combining it with the project path.
    /// </summary>
    /// <returns>A <see cref="FileInfo"/> object representing the input file.</returns>
    public FileInfo GetInputFileInfo()
    {
        if (string.IsNullOrEmpty(this.Path) || string.IsNullOrEmpty(this.InputFile))
        {
            throw new InvalidOperationException("Path or InputFile is not set.");
        }

        return this.ResolveProjectRelativePath(this.InputFile);
    }

    /// <summary>
    /// Gets a <see cref="FileInfo"/> object for the scan output file by combining it with the project path.
    /// </summary>
    /// <returns>A <see cref="FileInfo"/> object representing the scan output file, or null if not set.</returns>
    public FileInfo? GetScanOutputFileInfo()
    {
        if (string.IsNullOrEmpty(this.Path) || string.IsNullOrEmpty(this.ScanOutputFile))
        {
            return null;
        }

        return this.ResolveProjectRelativePath(this.ScanOutputFile);
    }

    /// <summary>
    /// Gets the power function to use for the project based on the PowerFunction property and Levels.
    /// </summary>
    /// <returns>
    /// A new instance of the <see cref="PowerFunction"/> class based on the project's PowerFunction property and Levels.
    /// </returns>
    public PowerFunction GetPowerFunction()
    {
        if (string.Equals(this.PowerFunction, "scrimshaw", StringComparison.Ordinal))
        {
            return new ScrimshawPowerFunction();
        }

        if (this.Levels != null)
        {
            return new GamutLevelsPowerFunction(this.Levels);
        }

        return GamutLevelsPowerFunction.Default;
    }

    /// <summary>
    /// Resolves a project-relative path by combining it with the project path.
    /// </summary>
    /// <param name="relativePath">The relative path to resolve.</param>
    /// <returns>The resolved absolute path.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the project path is not set.</exception>
    private FileInfo ResolveProjectRelativePath(string relativePath)
    {
        if (string.IsNullOrEmpty(this.Path))
        {
            throw new InvalidOperationException("Project path is not set.");
        }

        string absolutePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.Path)!, relativePath);
        return new FileInfo(absolutePath);
    }
}