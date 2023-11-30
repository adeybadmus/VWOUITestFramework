using System.Diagnostics;
using global::NUnit.Framework;
using global::TechTalk.SpecFlow;

[SetUpFixture]
public class VWOMUIAutomation_NUnitAssemblyHooks
{
    [OneTimeSetUp]
    public void AssemblyInitialize()
    {
        var currentAssembly = typeof(VWOMUIAutomation_NUnitAssemblyHooks).Assembly;

        TestRunnerManager.OnTestRunStart(currentAssembly);
    }

    [OneTimeTearDown]
    public void AssemblyCleanup()
    {
        var currentAssembly = typeof(VWOMUIAutomation_NUnitAssemblyHooks).Assembly;

        TestRunnerManager.OnTestRunEnd(currentAssembly);
    }
}
