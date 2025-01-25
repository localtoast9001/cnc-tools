//-----------------------------------------------------------------------
// <copyright file="GCodeNumericValueTest.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.GCode.UnitTest;

using Common.GCode.IO;
using Moq;

/// <summary>
/// Unit tests for the <see cref="GCodeNumericValue"/> class.
/// </summary>
[TestClass]
public class GCodeNumericValueTest
{
    /// <summary>
    /// Unit test for the <see cref="GCodeNumericValue"/> constructor.
    /// </summary>
    [TestMethod]
    public void DefaultConstructorTest()
    {
        var target = new GCodeNumericValue();
        Assert.AreEqual(0M, target.Value);
        Assert.AreEqual(false, target.IsInteger);
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeNumericValue"/> constructor.
    /// </summary>
    [TestMethod]
    public void IntegerConstructorTest()
    {
        var target = new GCodeNumericValue(5);
        Assert.AreEqual(5M, target.Value);
        Assert.AreEqual(true, target.IsInteger);
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeNumericValue"/> constructor.
    /// </summary>
    [TestMethod]
    public void DecimalConstructorTest()
    {
        var target = new GCodeNumericValue(5.5M);
        Assert.AreEqual(5.5M, target.Value);
        Assert.AreEqual(false, target.IsInteger);
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeNumericValue.WriteTo(IO.GCodeWriter)"/> method.
    /// </summary>
    [TestMethod]
    public void WriteToDecimalTest()
    {
        var target = new GCodeNumericValue(5.5M);
        var writer = new Mock<IO.GCodeWriter>(new GCodeWriterSettings());
        writer
            .Setup(w => w.WriteValue(It.Is<decimal>(v => v == 5.5M)))
            .Verifiable(Times.Once);

        target.WriteTo(writer.Object);
        writer.VerifyAll();
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeNumericValue.WriteTo(IO.GCodeWriter)"/> method.
    /// </summary>
    [TestMethod]
    public void WriteToIntegerTest()
    {
        var target = new GCodeNumericValue(5);
        var writer = new Mock<IO.GCodeWriter>(new GCodeWriterSettings());
        writer
            .Setup(w => w.WriteValue(It.Is<int>(v => v == 5)))
            .Verifiable(Times.Once);

        target.WriteTo(writer.Object);
        writer.VerifyAll();
    }
}