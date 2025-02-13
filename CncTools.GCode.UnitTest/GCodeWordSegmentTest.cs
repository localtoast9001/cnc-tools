//-----------------------------------------------------------------------
// <copyright file="GCodeWordSegmentTest.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CncTools.GCode.UnitTest;

using CncTools.GCode.IO;
using Moq;

/// <summary>
/// Unit tests for the <see cref="GCodeWordSegment"/> class.
/// </summary>
[TestClass]
public class GCodeWordSegmentTest
{
    /// <summary>
    /// Unit test for the <see cref="GCodeWordSegment.GCodeWordSegment()"/> constructor.
    /// </summary>
    [TestMethod]
    public void DefaultConstructorTest()
    {
        var target = new GCodeWordSegment();
        Assert.AreEqual(Code.Invalid, target.Code);
        Assert.IsNull(target.Value);
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeWordSegment.GCodeWordSegment(Code, int)"/> constructor.
    /// </summary>
    [TestMethod]
    public void IntegerConstructorTest()
    {
        var target = new GCodeWordSegment(Code.A, 5);
        Assert.AreEqual(Code.A, target.Code);
        var actual = target.Value as GCodeNumericValue;
        Assert.IsNotNull(actual);
        Assert.AreEqual(5M, actual.Value);
        Assert.AreEqual(true, actual.IsInteger);
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeWordSegment.GCodeWordSegment(Code, decimal)"/> constructor.
    /// </summary>
    [TestMethod]
    public void DecimalConstructorTest()
    {
        var target = new GCodeWordSegment(Code.A, 5.5M);
        Assert.AreEqual(Code.A, target.Code);
        var actual = target.Value as GCodeNumericValue;
        Assert.IsNotNull(actual);
        Assert.AreEqual(5.5M, actual.Value);
        Assert.AreEqual(false, actual.IsInteger);
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeWordSegment.FromCommand(CommandCode)"/> constructor.
    /// </summary>
    [TestMethod]
    public void FromCommandTest()
    {
        var actual = GCodeWordSegment.FromCommand(CommandCode.AbsoluteMode);
        Assert.IsNotNull(actual);
        Assert.AreEqual(Code.G, actual.Code);
        var actualValue = actual.Value as GCodeNumericValue;
        Assert.IsNotNull(actualValue);
        Assert.AreEqual(90M, actualValue.Value);
        Assert.AreEqual(true, actualValue.IsInteger);
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeWordSegment.WriteTo(IO.GCodeWriter)"/> method.
    /// </summary>
    [TestMethod]
    public void WriteToIntegerTest()
    {
        var target = GCodeWordSegment.FromCommand(CommandCode.AbsoluteMode);
        var writer = new Mock<GCodeWriter>(new GCodeWriterSettings());
        writer.Setup(x => x.StartWord(It.Is(Code.G, EqualityComparer<Code>.Default)))
            .Verifiable(Times.Once);
        writer.Setup(x => x.WriteValue(It.Is(90, EqualityComparer<int>.Default)))
            .Verifiable(Times.Once);
        writer.Setup(x => x.EndWord())
            .Verifiable(Times.Once);

        target.WriteTo(writer.Object);

        writer.VerifyAll();
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeWordSegment.WriteTo(IO.GCodeWriter)"/> method.
    /// </summary>
    [TestMethod]
    public void WriteToDecimalTest()
    {
        var target = new GCodeWordSegment(Code.X, 5.5M);
        var writer = new Mock<GCodeWriter>(new GCodeWriterSettings());
        writer.Setup(x => x.StartWord(It.Is(Code.X, EqualityComparer<Code>.Default)))
            .Verifiable(Times.Once);
        writer.Setup(x => x.WriteValue(It.Is(5.5M, EqualityComparer<decimal>.Default)))
            .Verifiable(Times.Once);
        writer.Setup(x => x.EndWord())
            .Verifiable(Times.Once);

        target.WriteTo(writer.Object);

        writer.VerifyAll();
    }
}