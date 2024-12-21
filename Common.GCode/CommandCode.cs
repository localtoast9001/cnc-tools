//-----------------------------------------------------------------------
// <copyright file="CommandCode.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.GCode;

/// <summary>
/// The code for a command.
/// </summary>
public struct CommandCode
{
    /// <summary>
    /// The rapid position command code.
    /// </summary>
    public static readonly CommandCode RapidPosition = new(Code.G, 0);

    /// <summary>
    /// The linear interpolation command code.
    /// </summary>
    public static readonly CommandCode LinearInterpretation = new(Code.G, 1);

    /// <summary>
    /// The circular or helical clockwise interpolation command code.
    /// </summary>
    public static readonly CommandCode CircularInterpolationClockwise = new(Code.G, 2);

    /// <summary>
    /// The circular or helical counter-clockwise interpolation command code.
    /// </summary>
    public static readonly CommandCode CircularInterpolationCounterClockwise = new(Code.G, 3);

    /// <summary>
    /// The XY plane selection command code.
    /// </summary>
    public static readonly CommandCode XYPlaneSelection = new(Code.G, 17);

    /// <summary>
    /// The XZ plane selection command code.
    /// </summary>
    public static readonly CommandCode XZPlaneSelection = new(Code.G, 18);

    /// <summary>
    /// The YZ plane selection command code.
    /// </summary>
    public static readonly CommandCode YZPlaneSelection = new(Code.G, 19);

    /// <summary>
    /// The inches unit selection command code.
    /// </summary>
    public static readonly CommandCode InchesSelection = new(Code.G, 20);

    /// <summary>
    /// The millimeter unit selection command code.
    /// </summary>
    public static readonly CommandCode MillimetersSelection = new(Code.G, 21);

    /// <summary>
    /// The return home command code.
    /// </summary>
    public static readonly CommandCode ReturnHome = new(Code.G, 28);

    /// <summary>
    /// The absolute coordinate interpretation mode selection command code.
    /// </summary>
    public static readonly CommandCode AbsoluteMode = new(Code.G, 90);

    /// <summary>
    /// The relative coordinate interpretation mode selection command code.
    /// </summary>
    public static readonly CommandCode RelativeMode = new(Code.G, 91);

    /// <summary>
    /// The program stop command code.
    /// </summary>
    public static readonly CommandCode ProgramStop = new(Code.M, 0);

    /// <summary>
    /// The end of program command code.
    /// </summary>
    public static readonly CommandCode EndOfProgram = new(Code.M, 2);

    /// <summary>
    /// The spindle ON clockwise command code.
    /// </summary>
    public static readonly CommandCode SpindleOnClockwise = new(Code.M, 3);

    /// <summary>
    /// The spindle ON counter-clockwise command code.
    /// </summary>
    public static readonly CommandCode SpindleOnCounterClockwise = new(Code.M, 4);

    /// <summary>
    /// The spindle stop command code.
    /// </summary>
    public static readonly CommandCode SpindleStop = new(Code.M, 5);

    /// <summary>
    /// The tool change command code.
    /// </summary>
    public static readonly CommandCode ToolChange = new(Code.M, 6);

    /// <summary>
    /// The flood coolant ON command code.
    /// </summary>
    public static readonly CommandCode FloodCoolantOn = new(Code.M, 8);

    /// <summary>
    /// The flood coolant OFF command code.
    /// </summary>
    public static readonly CommandCode FloodCoolantOff = new(Code.M, 9);

    /// <summary>
    /// The end of program and reset command code.
    /// </summary>
    public static readonly CommandCode EndOfProgramAndReset = new(Code.M, 30);

    /// <summary>
    /// The start extruder heating command code.
    /// </summary>
    public static readonly CommandCode StartExtruderHeating = new(Code.M, 104);

    /// <summary>
    /// The set fan speed command code.
    /// </summary>
    public static readonly CommandCode SetFanSpeed = new(Code.M, 106);

    /// <summary>
    /// The wait for extruder to reach temperature command code.
    /// </summary>
    public static readonly CommandCode WaitExtruderTemperature = new(Code.M, 109);

    /// <summary>
    /// The start bed heating command code.
    /// </summary>
    public static readonly CommandCode StartBedHeating = new(Code.M, 140);

    /// <summary>
    /// The wait for bed to reach temperature command code.
    /// </summary>
    public static readonly CommandCode WaitBedTemperature = new(Code.M, 190);

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandCode"/> struct.
    /// </summary>
    /// <param name="code">The code letter.</param>
    /// <param name="value">The code value.</param>
    public CommandCode(Code code, int value)
    {
        this.Code = code;
        this.Value = value;
    }

    /// <summary>
    /// Gets the letter prefix for the command.
    /// </summary>
    public Code Code { get; }

    /// <summary>
    /// Gets the code value.
    /// </summary>
    public int Value { get; }
}