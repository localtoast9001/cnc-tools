//-----------------------------------------------------------------------
// <copyright file="Code.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CncTools.GCode;

/// <summary>
/// Code letter that starts a code word.
/// </summary>
/// <remarks>
/// E, O, U, V, and W are missing. These are also missing and not reserved in the official language grammar.
/// The enum values match the upper-case ASCII values for convenience.
/// </remarks>
/// <seealso href="https://www.nist.gov/publications/nist-rs274ngc-interpreter-version-3?pub_id=823374"/>
public enum Code : sbyte
{
    /// <summary>
    /// The default invalid code.
    /// </summary>
    Invalid = 0,

    /// <summary>
    /// A-axis of machine.
    /// </summary>
    A = (sbyte)'A',

    /// <summary>
    /// B-axis of machine.
    /// </summary>
    B = (sbyte)'B',

    /// <summary>
    /// C-axis of machine.
    /// </summary>
    C = (sbyte)'C',

    /// <summary>
    /// Tool radius compensation number.
    /// </summary>
    D = (sbyte)'D',

    /// <summary>
    /// Feedrate.
    /// </summary>
    F = (sbyte)'F',

    /// <summary>
    /// General function.
    /// </summary>
    G = (sbyte)'G',

    /// <summary>
    /// Tool length offset index.
    /// </summary>
    H = (sbyte)'H',

    /// <summary>
    /// X-axis offset for arcs.
    /// X offset in G87 canned cycle.
    /// </summary>
    I = (sbyte)'I',

    /// <summary>
    /// Y-axis offset for arcs.
    /// Y offset in G87 canned cycle.
    /// </summary>
    J = (sbyte)'J',

    /// <summary>
    /// Z-axis offset for arcs.
    /// Z offset in G87 canned cycle.
    /// </summary>
    K = (sbyte)'K',

    /// <summary>
    /// Number of repetitions in canned cycles.
    /// Key used with G10.
    /// </summary>
    L = (sbyte)'L',

    /// <summary>
    /// Miscellaneous function.
    /// </summary>
    M = (sbyte)'M',

    /// <summary>
    /// Line number.
    /// </summary>
    N = (sbyte)'N',

    /// <summary>
    /// Dwell time in canned cycles.
    /// Dwell time with G4.
    /// Key used with G10.
    /// </summary>
    P = (sbyte)'P',

    /// <summary>
    /// Feed increment in G83 canned cycle.
    /// </summary>
    Q = (sbyte)'Q',

    /// <summary>
    /// Arc radius.
    /// Canned cycle plane.
    /// </summary>
    R = (sbyte)'R',

    /// <summary>
    /// Spindle speed.
    /// </summary>
    S = (sbyte)'S',

    /// <summary>
    /// Tool selection.
    /// </summary>
    T = (sbyte)'T',

    /// <summary>
    /// X-axis of machine.
    /// </summary>
    X = (sbyte)'X',

    /// <summary>
    /// Y-axis of machine.
    /// </summary>
    Y = (sbyte)'Y',

    /// <summary>
    /// Z-axis of machine.
    /// </summary>
    Z = (sbyte)'Z',
}