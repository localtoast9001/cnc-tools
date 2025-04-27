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
        Project result = JsonSerializer.Deserialize<Project>(jsonContent) ??
            throw new InvalidOperationException($"Failed to deserialize project from {path}.");
        result.Path = path;
        return result;
    }
}