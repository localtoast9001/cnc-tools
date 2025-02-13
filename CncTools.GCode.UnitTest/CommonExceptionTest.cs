//-----------------------------------------------------------------------
// <copyright file="CommonExceptionTest.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CncTools.GCode.UnitTest;

using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Base class for exception tests that include common semantics needed for all exceptions.
/// </summary>
public abstract class CommonExceptionTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommonExceptionTest"/> class.
    /// </summary>
    /// <param name="defaultConstructor">Reference to the exception's default constructor.</param>
    /// <param name="messageConstructor">Reference to the exception's constructor that takes a message argument.</param>
    /// <param name="messageAndInnerExceptionConstructor">Reference to the exception's constructor that takes a message and optional inner exception.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="defaultConstructor"/> is <see langword="null"/>, or
    /// <paramref name="messageConstructor"/> is <see langword="null"/>, or
    /// <paramref name="messageAndInnerExceptionConstructor"/> is <see langword="null"/>.
    /// </exception>
    protected CommonExceptionTest(
        Func<Exception> defaultConstructor,
        Func<string, Exception> messageConstructor,
        Func<string, Exception?, Exception> messageAndInnerExceptionConstructor)
    {
        this.DefaultConstructor = defaultConstructor ?? throw new ArgumentNullException(nameof(defaultConstructor));
        this.MessageConstructor = messageConstructor ?? throw new ArgumentNullException(nameof(messageConstructor));
        this.MessageAndInnerExceptionConstructor = messageAndInnerExceptionConstructor ?? throw new ArgumentNullException(nameof(messageAndInnerExceptionConstructor));
    }

    /// <summary>
    /// Gets the reference to the exception's default constructor.
    /// </summary>
    protected Func<Exception> DefaultConstructor { get; }

    /// <summary>
    /// Gets the reference to the exception's constructor that takes a string message.
    /// </summary>
    protected Func<string, Exception> MessageConstructor { get; }

    /// <summary>
    /// Gets the reference to the exception's constructor that takes a string message and optional inner exception.
    /// </summary>
    protected Func<string, Exception?, Exception> MessageAndInnerExceptionConstructor { get; }

    /// <summary>
    /// Unit test for the derived <see cref="Exception.Exception()"/> constructor.
    /// </summary>
    [TestMethod]
    public void DefaultConstructorTest()
    {
        var target = this.DefaultConstructor();
        Assert.AreEqual($"Exception of type '{target.GetType().FullName}' was thrown.", target.Message);
        Assert.IsNull(target.InnerException);
    }

    /// <summary>
    /// Unit test for the derived <see cref="Exception.Exception(string)"/> constructor.
    /// </summary>
    [TestMethod]
    public void MessageConstructorTest()
    {
        var target = this.MessageConstructor("test");
        Assert.AreEqual("test", target.Message);
        Assert.IsNull(target.InnerException);
    }

    /// <summary>
    /// Unit test for the derived <see cref="Exception.Exception(string, Exception?)"/> constructor.
    /// </summary>
    [TestMethod]
    public void MessageAndInnerExceptionConstructorTest()
    {
        var target = this.MessageAndInnerExceptionConstructor("test", new InvalidOperationException("inner"));
        Assert.AreEqual("test", target.Message);
        Assert.IsNotNull(target.InnerException);

        target = this.MessageAndInnerExceptionConstructor("test", null);
        Assert.AreEqual("test", target.Message);
        Assert.IsNull(target.InnerException);
    }

    /// <summary>
    /// Unit test for the derived exception serialization and deserialization.
    /// </summary>
    [TestMethod]
    public void SerializationTest()
    {
        // NOTE: Using unsafe serialization library for compatibility for exercising the required Exception serialization members used by the .Net Framework.
#pragma warning disable SYSLIB0011 // Type or member is obsolete
        BinaryFormatter formatter = new();
#pragma warning restore SYSLIB0011 // Type or member is obsolete

        using MemoryStream ms = new();
        var target = this.MessageAndInnerExceptionConstructor("test", new InvalidOperationException("inner"));
        formatter.Serialize(ms, target);
        Assert.IsTrue(ms.Length > 0);
        ms.Seek(0, SeekOrigin.Begin);

        var actual = formatter.Deserialize(ms);
        Assert.IsNotNull(actual);
        Assert.AreEqual(target.GetType(), actual.GetType());
        Exception actualEx = (Exception)actual;
        Assert.AreEqual(target.Message, actualEx.Message);
        Assert.IsNotNull(actualEx.InnerException);
        Assert.AreEqual(target.InnerException!.Message, actualEx.InnerException.Message);
    }
}