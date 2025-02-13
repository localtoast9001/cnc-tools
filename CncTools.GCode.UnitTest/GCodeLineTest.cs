//-----------------------------------------------------------------------
// <copyright file="GCodeLineTest.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CncTools.GCode.UnitTest;

using CncTools.GCode.IO;
using Moq;

/// <summary>
/// Unit tests for the <see cref="GCodeLine"/> class.
/// </summary>
[TestClass]
public class GCodeLineTest
{
    /// <summary>
    /// Unit test for the <see cref="GCodeLine.GCodeLine()"/> constructor.
    /// </summary>
    [TestMethod]
    public void DefaultConstructorTest()
    {
        var target = new GCodeLine();
        Assert.IsNotNull(target.Segments);
        Assert.AreEqual(0, target.Segments.Count);
        Assert.AreEqual(false, target.IsBlockDelete);
        Assert.IsTrue(target.LineNumber < 0);
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeLine.GCodeLine(GCodeSegment[])"/> constructor.
    /// </summary>
    [TestMethod]
    public void ConstructorWithParamsTest()
    {
        var target = new GCodeLine(
            new GCodeCommentSegment(),
            new GCodeWordSegment(),
            new GCodeWordSegment());
        Assert.IsNotNull(target.Segments);
        Assert.AreEqual(3, target.Segments.Count);
        Assert.AreEqual(false, target.IsBlockDelete);
        Assert.IsTrue(target.LineNumber < 0);
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeLine.GCodeLine(IEnumerable{GCodeSegment}, int, bool)"/> constructor.
    /// </summary>
    [TestMethod]
    public void FullConstructorTest()
    {
        var target = new GCodeLine(
            new GCodeSegment[]
            {
                new GCodeCommentSegment(),
                new GCodeWordSegment(),
                new GCodeWordSegment(),
            },
            5,
            true);
        Assert.IsNotNull(target.Segments);
        Assert.AreEqual(3, target.Segments.Count);
        Assert.AreEqual(true, target.IsBlockDelete);
        Assert.AreEqual(5, target.LineNumber);
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeLine.Comment(string, bool)"/> method.
    /// </summary>
    [TestMethod]
    public void CommentTest()
    {
        var actual = GCodeLine.Comment("test");
        Assert.IsNotNull(actual);
        Assert.AreEqual(1, actual.Segments.Count);
        var segment = actual.Segments.OfType<GCodeCommentSegment>().FirstOrDefault();
        Assert.IsNotNull(segment);
        Assert.AreEqual("test", segment.Text);
        Assert.AreEqual(false, segment.IsMessage);

        actual = GCodeLine.Comment("test message", true);
        Assert.IsNotNull(actual);
        Assert.AreEqual(1, actual.Segments.Count);
        segment = actual.Segments.OfType<GCodeCommentSegment>().FirstOrDefault();
        Assert.IsNotNull(segment);
        Assert.AreEqual("test message", segment.Text);
        Assert.AreEqual(true, segment.IsMessage);
    }
}