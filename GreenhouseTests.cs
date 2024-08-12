namespace APiTests;

using System.Threading.Tasks;
using Greenhouse;

[TestFixture]
public class GreenhouseTests
{
    static Greenhouse? greenhouse;

    [SetUp]
    public void Setup()
    {
        greenhouse = new();
    }

    [Test]
    public async Task Test1Async()
    {
        await greenhouse!.goAsync();

        Assert.Pass();
    }

    [Test]
    public async Task Integration_CanGetFirstSchedules()
    {
        var schedules = await greenhouse!.GetSchedulesAsync();
        Assert.That(schedules, Is.Not.Empty);
        Assert.That(schedules, Does.Not.Contain("Resource not found"));
        Console.WriteLine(schedules);
    }

    [Test]
    public async Task Integration_CanGetNextSchedules()
    {
        var schedules = await greenhouse!.GetNextSchedulesAsync();
        Assert.That(schedules, Is.Not.Empty);
        Assert.That(schedules, Does.Not.Contain("Resource not found"));
        Console.WriteLine(schedules);
    }

}
