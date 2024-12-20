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
    /// Letter prefix for general purpose or geometry command codes.
    /// </summary>
    public const char GeneralPrefix = 'G';

    /// <summary>
    /// Letter prefix for miscellaneous or machine command codes.
    /// </summary>
    public const char MiscPrefix = 'M';

    /// <summary>
    /// The rapid position command code.
    /// </summary>
    public static readonly CommandCode RapidPosition = new(GeneralPrefix, 0);

    /// <summary>
    /// The linear interpolation command code.
    /// </summary>
    public static readonly CommandCode LinearInterpretation = new(GeneralPrefix, 1);

    /// <summary>
    /// The circular or helical clockwise interpolation command code.
    /// </summary>
    public static readonly CommandCode CircularInterpolationClockwise = new(GeneralPrefix, 2);

    /// <summary>
    /// The circular or helical counter-clockwise interpolation command code.
    /// </summary>
    public static readonly CommandCode CircularInterpolationCounterClockwise = new(GeneralPrefix, 3);

    /// <summary>
    /// The XY plane selection command code.
    /// </summary>
    public static readonly CommandCode XYPlaneSelection = new CommandCode(GeneralPrefix, 17);

    /// <summary>
    /// The XZ plane selection command code.
    /// </summary>
    public static readonly CommandCode XZPlaneSelection = new CommandCode(GeneralPrefix, 18);

    /// <summary>
    /// The YZ plane selection command code.
    /// </summary>
    public static readonly CommandCode YZPlaneSelection = new CommandCode(GeneralPrefix, 19);

    /// <summary>
    /// The inches unit selection command code.
    /// </summary>
    public static readonly CommandCode InchesSelection = new CommandCode(GeneralPrefix, 20);

    /// <summary>
    /// The millimeter unit selection command code.
    /// </summary>
    public static readonly CommandCode MillimetersSelection = new CommandCode(GeneralPrefix, 21);

    /// <summary>
    /// The return home command code.
    /// </summary>
    public static readonly CommandCode ReturnHome = new CommandCode(GeneralPrefix, 28);

    /// <summary>
    /// The absolute coordinate interpretation mode selection command code.
    /// </summary>
    public static readonly CommandCode AbsoluteMode = new CommandCode(GeneralPrefix, 90);

    /// <summary>
    /// The relative coordinate interpretation mode selection command code.
    /// </summary>
    public static readonly CommandCode RelativeMode = new CommandCode(GeneralPrefix, 91);

    /// <summary>
    /// The program stop command code.
    /// </summary>
    public static readonly CommandCode ProgramStop = new CommandCode(MiscPrefix, 0);

    /// <summary>
    /// The end of program command code.
    /// </summary>
    public static readonly CommandCode EndOfProgram = new CommandCode(MiscPrefix, 2);

    /// <summary>
    /// The spindle ON clockwise command code.
    /// </summary>
    public static readonly CommandCode SpindleOnClockwise = new CommandCode(MiscPrefix, 3);

    /// <summary>
    /// The spindle ON counter-clockwise command code.
    /// </summary>
    public static readonly CommandCode SpindleOnCounterClockwise = new CommandCode(MiscPrefix, 4);

    /// <summary>
    /// The spindle stop command code.
    /// </summary>
    public static readonly CommandCode SpindleStop = new CommandCode(MiscPrefix, 5);

    /// <summary>
    /// The tool change command code.
    /// </summary>
    public static readonly CommandCode ToolChange = new CommandCode(MiscPrefix, 6);

    /// <summary>
    /// The flood coolant ON command code.
    /// </summary>
    public static readonly CommandCode FloodCoolantOn = new CommandCode(MiscPrefix, 8);

    /// <summary>
    /// The flood coolant OFF command code.
    /// </summary>
    public static readonly CommandCode FloodCoolantOff = new CommandCode(MiscPrefix, 9);

    /// <summary>
    /// The end of program and reset command code.
    /// </summary>
    public static readonly CommandCode EndOfProgramAndReset = new CommandCode(MiscPrefix, 30);

    /// <summary>
    /// The start extruder heating command code.
    /// </summary>
    public static readonly CommandCode StartExtruderHeating = new CommandCode(MiscPrefix, 104);

    /// <summary>
    /// The set fan speed command code.
    /// </summary>
    public static readonly CommandCode SetFanSpeed = new CommandCode(MiscPrefix, 106);

    /// <summary>
    /// The wait for extruder to reach temperature command code.
    /// </summary>
    public static readonly CommandCode WaitExtruderTemperature = new CommandCode(MiscPrefix, 109);

    /// <summary>
    /// The start bed heating command code.
    /// </summary>
    public static readonly CommandCode StartBedHeating = new CommandCode(MiscPrefix, 140);

    /// <summary>
    /// The wait for bed to reach temperature command code.
    /// </summary>
    public static readonly CommandCode WaitBedTemperature = new CommandCode(MiscPrefix, 190);

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandCode"/> struct.
    /// </summary>
    /// <param name="prefix">The prefix letter.</param>
    /// <param name="code">The code value.</param>
    public CommandCode(char prefix, int code)
    {
        this.Prefix = prefix;
        this.Code = code;
    }

    /// <summary>
    /// Gets the letter prefix for the command.
    /// </summary>
    public char Prefix { get; }

    /// <summary>
    /// Gets the code value.
    /// </summary>
    public int Code { get; }
}