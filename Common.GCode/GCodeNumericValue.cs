//-----------------------------------------------------------------------
// <copyright file="GCodeNumericValue.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.GCode;

/// <summary>
/// Word numeric value.
/// </summary>
public class GCodeNumericValue : GCodeValue
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeNumericValue"/> class.
    /// </summary>
    public GCodeNumericValue()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeNumericValue"/> class.
    /// </summary>
    /// <param name="value">Integer value.</param>
    public GCodeNumericValue(int value)
    {
        this.Value = value;
        this.IsInteger = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeNumericValue"/> class.
    /// </summary>
    /// <param name="value">Decimal value.</param>
    public GCodeNumericValue(decimal value)
    {
        this.Value = value;
        this.IsInteger = false;
    }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not the value should be represented as an integer.
    /// </summary>
    public bool IsInteger { get; set; }

    /// <inheritdoc/>
    public override void WriteTo(IO.GCodeWriter writer)
    {
        if (!this.IsInteger)
        {
            writer.WriteValue(this.Value);
        }
        else
        {
            writer.WriteValue((int)this.Value);
        }
    }
}