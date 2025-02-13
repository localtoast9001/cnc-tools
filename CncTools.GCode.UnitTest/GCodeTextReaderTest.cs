//-----------------------------------------------------------------------
// <copyright file="GCodeTextReaderTest.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.GCode.UnitTest;

using Common.GCode.IO;

/// <summary>
/// Unit tests for the <see cref="GCodeTextReader"/> class.
/// </summary>
[TestClass]
public class GCodeTextReaderTest
{
    /// <summary>
    /// Test reading an empty file.
    /// </summary>
    [TestMethod]
    public void ReadEmptyFileTest()
    {
        using var target = new GCodeTextReader(new StringReader(string.Empty));
        Assert.IsFalse(target.Read());
    }

    /// <summary>
    /// Test reading a valid file with no lines.
    /// </summary>
    [TestMethod]
    public void ReadFileBlockWithNoLinesTest()
    {
        using var target = new GCodeTextReader(new StringReader("%\n%"));
        Assert.IsTrue(target.Read());
        Assert.AreEqual(GCodeTokenType.FileStart, target.TokenType);
        Assert.IsTrue(target.Read());
        Assert.AreEqual(GCodeTokenType.FileEnd, target.TokenType);
        Assert.IsFalse(target.Read());
    }
}