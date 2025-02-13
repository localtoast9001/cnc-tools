//-----------------------------------------------------------------------
// <copyright file="GCodeCommentSegmentTest.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CncTools.GCode.UnitTest;

using CncTools.GCode.IO;
using Moq;

/// <summary>
/// Unit tests for the <see cref="GCodeCommentSegment"/> class.
/// </summary>
[TestClass]
public class GCodeCommentSegmentTest
{
    /// <summary>
    /// Unit test for the <see cref="GCodeCommentSegment.GCodeCommentSegment()"/> constructor.
    /// </summary>
    [TestMethod]
    public void DefaultConstructorTest()
    {
        var target = new GCodeCommentSegment();
        Assert.IsNull(target.Text);
        Assert.AreEqual(false, target.IsMessage);
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeCommentSegment.GCodeCommentSegment(string, bool)"/> constructor.
    /// </summary>
    [TestMethod]
    public void MemberConstructorTest()
    {
        var target = new GCodeCommentSegment("test", true);
        Assert.AreEqual("test", target.Text);
        Assert.AreEqual(true, target.IsMessage);
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeCommentSegment.WriteTo(GCodeWriter)"/> method.
    /// </summary>
    [TestMethod]
    public void WriteToCommentTest()
    {
        var target = new GCodeCommentSegment("test");
        var writer = new Mock<GCodeWriter>(new GCodeWriterSettings());
        writer.Setup(x => x.WriteComment(It.Is<string>(e => "test".Equals(e, StringComparison.Ordinal))))
            .Verifiable(Times.Once);

        target.WriteTo(writer.Object);
        writer.VerifyAll();
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeCommentSegment.WriteTo(GCodeWriter)"/> method.
    /// </summary>
    [TestMethod]
    public void WriteToNullCommentTest()
    {
        var target = new GCodeCommentSegment();
        var writer = new Mock<GCodeWriter>(new GCodeWriterSettings());
        writer.Setup(x => x.WriteComment(It.Is<string>(e => string.Empty.Equals(e, StringComparison.Ordinal))))
            .Verifiable(Times.Once);

        target.WriteTo(writer.Object);
        writer.VerifyAll();
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeCommentSegment.WriteTo(GCodeWriter)"/> method.
    /// </summary>
    [TestMethod]
    public void WriteToMessageTest()
    {
        var target = new GCodeCommentSegment("test", true);
        var writer = new Mock<GCodeWriter>(new GCodeWriterSettings());
        writer.Setup(x => x.WriteMessage(It.Is<string>(e => "test".Equals(e, StringComparison.Ordinal))))
            .Verifiable(Times.Once);

        target.WriteTo(writer.Object);
        writer.VerifyAll();
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeCommentSegment.WriteTo(GCodeWriter)"/> method.
    /// </summary>
    [TestMethod]
    public void WriteToNullMessageTest()
    {
        var target = new GCodeCommentSegment() { IsMessage = true };
        var writer = new Mock<GCodeWriter>(new GCodeWriterSettings());
        writer.Setup(x => x.WriteMessage(It.Is<string>(e => string.Empty.Equals(e, StringComparison.Ordinal))))
            .Verifiable(Times.Once);

        target.WriteTo(writer.Object);
        writer.VerifyAll();
    }
}