//-----------------------------------------------------------------------
// <copyright file="GCodeTextWriterExceptionTest.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.GCode.UnitTest;

using Common.GCode.IO;

/// <summary>
/// Unit test for the <see cref="GCodeTextWriterException"/> class.
/// </summary>
[TestClass]
public class GCodeTextWriterExceptionTest : CommonExceptionTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeTextWriterExceptionTest"/> class.
    /// </summary>
    public GCodeTextWriterExceptionTest()
    : base(() => new GCodeTextWriterException(), (m) => new GCodeTextWriterException(m), (m, i) => new GCodeTextWriterException(m, i))
    {
    }
}