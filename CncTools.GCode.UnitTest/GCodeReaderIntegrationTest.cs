//-----------------------------------------------------------------------
// <copyright file="GCodeReaderIntegrationTest.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CncTools.GCode.UnitTest;

using CncTools.GCode.IO;

/// <summary>
/// Integration tests for the reader.
/// </summary>
[TestClass]
[DeploymentItem("hello_world.nc")]
public class GCodeReaderIntegrationTest
{
    /// <summary>
    /// Gets or sets the test context.
    /// </summary>
    public TestContext? TestContext { get; set; }

    /// <summary>
    /// Tests loading the hello world GCode file from the NIST spec.
    /// </summary>
    [TestMethod]
    public void HelloWorldTest()
    {
        string path = Path.Combine(this.TestContext!.TestDeploymentDir!, "hello_world.nc");
        GCodeDocument target = GCodeDocument.Load(path);
    }
}