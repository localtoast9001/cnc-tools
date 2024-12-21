//-----------------------------------------------------------------------
// <copyright file="GCodeDocumentTest.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.GCode.UnitTest;

using System.Text;

/// <summary>
/// Unit tests for the <see cref="GCodeDocument"/> class.
/// </summary>
[TestClass]
public class GCodeDocumentTest
{
    /// <summary>
    /// Unit test for the <see cref="GCodeDocument.WriteTo(IO.GCodeWriter)"/> method.
    /// </summary>
    [TestMethod]
    public void WriteToSingleLineTest()
    {
        var target = new GCodeDocument()
        {
            Lines =
            {
                new GCodeLine()
                {
                    Segments =
                    {
                        new GCodeWordSegment() { Code = Code.G, Value = new GCodeNumericValue() { IsInteger = true, Value = 1 } },
                        new GCodeWordSegment() { Code = Code.X, Value = new GCodeNumericValue() { IsInteger = false, Value = -4.5M } },
                        new GCodeWordSegment() { Code = Code.Y, Value = new GCodeNumericValue() { IsInteger = false, Value = 5.5M } },
                    },
                },
            },
        };

        StringBuilder sb = new();
        using IO.GCodeTextWriter writer = new(new StringWriter(sb));
        target.WriteTo(writer);
        string[] lines = sb.ToString().Split('\n').Select(e => e.Trim()).ToArray();
        Assert.AreEqual(4, lines.Length);
        Assert.AreEqual("%", lines[0]);
        Assert.AreEqual("G1 X-4.5 Y5.5", lines[1]);
        Assert.AreEqual("%", lines[2]);
        Assert.AreEqual(string.Empty, lines[3]);
    }
}