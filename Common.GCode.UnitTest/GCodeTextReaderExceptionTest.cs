//-----------------------------------------------------------------------
// <copyright file="GCodeTextReaderExceptionTest.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.GCode.UnitTest;

using Common.GCode.IO;

/// <summary>
/// Unit test for the <see cref="GCodeTextReaderException"/> class.
/// </summary>
[TestClass]
public class GCodeTextReaderExceptionTest : CommonExceptionTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeTextReaderExceptionTest"/> class.
    /// </summary>
    public GCodeTextReaderExceptionTest()
    : base(() => new GCodeTextReaderException(), (m) => new GCodeTextReaderException(m), (m, i) => new GCodeTextReaderException(m, i))
    {
    }
}