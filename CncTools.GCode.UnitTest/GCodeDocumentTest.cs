//-----------------------------------------------------------------------
// <copyright file="GCodeDocumentTest.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CncTools.GCode.UnitTest;

using System.Text;

/// <summary>
/// Unit tests for the <see cref="GCodeDocument"/> class.
/// </summary>
[TestClass]
[DeploymentItem("hello_world.nc")]
public class GCodeDocumentTest
{
    /// <summary>
    /// Gets or sets the test context.
    /// </summary>
    public TestContext? TestContext { get; set; }

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

    /// <summary>
    /// Unit test for the <see cref="GCodeDocument.Load(Stream)"/> method.
    /// </summary>
    [TestMethod]
    public void LoadStreamTest()
    {
        using var ms = new MemoryStream();
        using var writer = new StreamWriter(ms);
        writer.WriteLine("%");
        writer.WriteLine("G00 X0 Y0 Z0");
        writer.WriteLine("%");
        writer.Flush();
        ms.Seek(0, SeekOrigin.Begin);

        // NOTE: No need to validate all aspects of the read, just that the whole doc is read.
        var target = GCodeDocument.Load(ms);
        Assert.IsNotNull(target);
        Assert.AreEqual(1, target.Lines.Count);
        var line = target.Lines[0];
        Assert.AreEqual(4, line.Segments.Count);
        ms.Close();
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeDocument.Load(TextReader)"/> method.
    /// </summary>
    [TestMethod]
    public void LoadTextReaderTest()
    {
        using var reader = new StringReader("%\nG00 X0 Y0 Z0\n%");

        // NOTE: No need to validate all aspects of the read, just that the whole doc is read.
        var target = GCodeDocument.Load(reader);
        Assert.IsNotNull(target);
        Assert.AreEqual(1, target.Lines.Count);
        var line = target.Lines[0];
        Assert.AreEqual(4, line.Segments.Count);
        reader.Close();
    }

    /// <summary>
    /// Tests loading the hello world sample from the spec.
    /// </summary>
    [TestMethod]
    public void LoadHelloWorldTest()
    {
        string path = Path.Combine(this.TestContext!.TestDeploymentDir!, "hello_world.nc");
        var target = GCodeDocument.Load(path);
        Assert.IsNotNull(target);
        Assert.AreEqual(62, target.Lines.Count);
        var line40 = target.Lines.FirstOrDefault(e => e.LineNumber == 40);
        Assert.IsNotNull(line40);
        Assert.AreEqual(3, line40.Segments.Count);
        var word1 = line40.Segments[0] as GCodeWordSegment;
        Assert.IsNotNull(word1);
        Assert.AreEqual(Code.G, word1.Code);
        var value1 = word1.Value as GCodeNumericValue;
        Assert.IsNotNull(value1);
        Assert.AreEqual(true, value1.IsInteger);
        Assert.AreEqual(1, (int)value1.Value);
        var word2 = line40.Segments[1] as GCodeWordSegment;
        Assert.IsNotNull(word2);
        Assert.AreEqual(Code.Z, word2.Code);
        var value2 = word2.Value as GCodeNumericValue;
        Assert.IsNotNull(value2);
        Assert.AreEqual(false, value2.IsInteger);
        Assert.AreEqual(-0.5M, value2.Value);
        var comment1 = line40.Segments[2] as GCodeCommentSegment;
        Assert.IsNotNull(comment1);
        Assert.AreEqual(false, comment1.IsMessage);
        Assert.AreEqual("start H", comment1.Text);
    }
}