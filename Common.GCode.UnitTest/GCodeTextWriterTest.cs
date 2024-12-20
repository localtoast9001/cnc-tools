//-----------------------------------------------------------------------
// <copyright file="GCodeTextWriterTest.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.GCode.UnitTest;

using System.IO;
using System.Text;
using Common.GCode.IO;

/// <summary>
/// Unit tests for the <see cref="GCodeTextWriter"/> class.
/// </summary>
[TestClass]
public class GCodeTextWriterTest
{
    /// <summary>
    /// Tests the underlying text writer is left open when the writer is disposed.
    /// </summary>
    [TestMethod]
    public void LeaveOpenTest()
    {
        StringBuilder sb = new();
        var writer = new StringWriter(sb);
        using (var target = new GCodeTextWriter(writer))
        {
        }

        writer.WriteLine("test");
        writer.Close();
        Assert.AreEqual("test", sb.ToString().Trim());
    }

    /// <summary>
    /// Tests the underlying text writer is closed when the writer is disposed.
    /// </summary>
    [TestMethod]
    public void DoNotLeaveOpenTest()
    {
        StringBuilder sb = new();
        var writer = new StringWriter(sb);
        using (var target = new GCodeTextWriter(writer, new GCodeWriterSettings() { CloseInput = true }))
        {
        }

        try
        {
            writer.WriteLine("test");
            Assert.Fail("Expected exception.");
        }
        catch (ObjectDisposedException)
        {
        }
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeTextWriter"/> constructor.
    /// </summary>
    [TestMethod]
    public void ConstructorNegativeTest()
    {
        try
        {
            _ = new GCodeTextWriter(null!);
            Assert.Fail("Expected exception.");
        }
        catch (ArgumentNullException ex)
        {
            Assert.AreEqual("inner", ex.ParamName);
        }
    }

    /// <summary>
    /// Tests using the writer for complete sample output.
    /// </summary>
    [TestMethod]
    public void SampleSingleLineOutputTest()
    {
        StringBuilder sb = new();
        using var target = new GCodeTextWriter(new StringWriter(sb));
        target.StartFile();
        target.StartLine();
        target.WriteWord('G', 1);
        target.WriteWord('X', 5.5);
        target.WriteWord('Y', -4.75);
        target.EndLine();
        target.EndFile();
        string[] lines = sb.ToString().Split('\n').Select(e => e.Trim()).ToArray();
        Assert.AreEqual(4, lines.Length);
        Assert.AreEqual("%", lines[0]);
        Assert.AreEqual("G1 X5.5 Y-4.75", lines[1]);
        Assert.AreEqual("%", lines[2]);
        Assert.AreEqual(string.Empty, lines[3]);
    }
}