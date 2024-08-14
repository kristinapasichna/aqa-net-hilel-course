using System.Text.RegularExpressions;
using Microsoft.Playwright;

namespace DemoEvershop
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class ExampleTest : UITestFixture
    {
        [Test]
        public async Task HasTitle()
        {
            await Page.GotoAsync("https://playwright.dev");

            // Expect a title "to contain" a substring.
            await Assertions.Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));
        }

        [Test]
        public async Task GetStartedLink()
        {
            await Page.GotoAsync("https://playwright.dev");

            // Click the get started link.
            await Page.GetByRole(AriaRole.Link, new() { Name = "Get started" }).ClickAsync();

            // Expects page to have a heading with the name of Installation.
            await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Installation" })).ToBeVisibleAsync();
        } 
    }
}
