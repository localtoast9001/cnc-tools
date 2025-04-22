// <copyright file="GCodeWriterTest.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace CncTools.GCode.UnitTest;

using System.Text;
using CncTools.GCode;
using CncTools.GCode.IO;
using Moq;

/// <summary>
/// Unit tests for the <see cref="GCodeWriter"/> class.
/// </summary>
[TestClass]
public class GCodeWriterTest
{
    /// <summary>
    /// Gets or sets the test context.
    /// </summary>
    public TestContext? TestContext { get; set; }

    /// <summary>
    /// Unit test for the <see cref="GCodeWriter.Create(string, GCodeWriterSettings?)"/> method.
    /// </summary>
    [TestMethod]
    public void CreateWithPathTest()
    {
        string path = Path.Combine(this.TestContext!.TestDir!, $"{Guid.NewGuid()}.ngc");
        using var target = GCodeWriter.Create(path);
        Assert.IsNotNull(target);
        target.StartFile();
        target.EndFile();
        target.Close();
        Assert.IsTrue(File.Exists(path));
        string[] lines = File.ReadAllLines(path);
        Assert.IsNotNull(lines);
        Assert.IsTrue(lines.Length > 0);
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeWriter.Create(Stream, GCodeWriterSettings?)"/> method.
    /// </summary>
    [TestMethod]
    public void CreateWithStreamTest()
    {
        using var ms = new MemoryStream();
        using var target = GCodeWriter.Create(ms, new GCodeWriterSettings());
        Assert.IsNotNull(target);
        target.StartFile();
        target.EndFile();
        target.Close();
        Assert.IsTrue(ms.Length > 0);
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeWriter.Create(TextWriter, GCodeWriterSettings?)"/> method.
    /// </summary>
    [TestMethod]
    public void CreateWithTextWriterTest()
    {
        var sb = new StringBuilder();
        var writer = new StringWriter(sb);
        var target = GCodeWriter.Create(writer);
        Assert.IsNotNull(target);
        target.StartFile();
        target.EndFile();
        target.Close();
        Assert.IsTrue(sb.Length > 0);
    }

    /// <summary>
    /// Unit test for the <see cref="GCodeWriter.DisposeAsync()"/> method.
    /// </summary>
    /// <returns>A <see cref="Task"/> for the async operation.</returns>
    [TestMethod]
    public async Task DisposeAsyncTest()
    {
        var settings = new GCodeWriterSettings()
        {
            CloseOutput = true,
            Async = true,
        };

        // NOTE: Using Moq will override all virtual methods.
        TestGCodeWriter target = new TestGCodeWriter(settings);

        bool closeAsyncCalled = false;
        target.OnCloseAsyncCallback = (ct) =>
        {
            closeAsyncCalled = true;
        };

        Assert.IsNotNull(target.Settings);
        Assert.AreEqual(true, target.Settings.CloseOutput);
        await using (target.ConfigureAwait(false))
        {
        }

        Assert.IsTrue(closeAsyncCalled);
    }

    private class TestGCodeWriter : GCodeWriter
    {
        public TestGCodeWriter(GCodeWriterSettings? settings = null)
        : base(settings)
        {
        }

        public Action? OnCloseCallback { get; set; }

        public Action<CancellationToken>? OnCloseAsyncCallback { get; set; }

        public override void Close()
        {
            if (this.OnCloseCallback != null)
            {
                this.OnCloseCallback();
            }
        }

        public override ValueTask CloseAsync(CancellationToken cancellationToken = default)
        {
            if (this.OnCloseAsyncCallback != null)
            {
                this.OnCloseAsyncCallback(cancellationToken);
            }

            return ValueTask.CompletedTask;
        }

        public override void EndExpression()
        {
            throw new NotImplementedException();
        }

        public override void EndFile()
        {
            throw new NotImplementedException();
        }

        public override void EndLine(string? endComment = null)
        {
            throw new NotImplementedException();
        }

        public override void EndParameterSetting()
        {
            throw new NotImplementedException();
        }

        public override void EndWord()
        {
            throw new NotImplementedException();
        }

        public override void StartExpression()
        {
            throw new NotImplementedException();
        }

        public override void StartFile()
        {
            throw new NotImplementedException();
        }

        public override void StartLine(int lineNumber = -1, bool blockDelete = false)
        {
            throw new NotImplementedException();
        }

        public override void StartParameterSetting()
        {
            throw new NotImplementedException();
        }

        public override void StartWord(Code code)
        {
            throw new NotImplementedException();
        }

        public override void WriteComment(string comment)
        {
            throw new NotImplementedException();
        }

        public override void WriteMessage(string message)
        {
            throw new NotImplementedException();
        }

        public override void WriteValue(int value)
        {
            throw new NotImplementedException();
        }

        public override void WriteValue(float value)
        {
            throw new NotImplementedException();
        }

        public override void WriteValue(double value)
        {
            throw new NotImplementedException();
        }

        public override void WriteValue(decimal value)
        {
            throw new NotImplementedException();
        }
    }
}