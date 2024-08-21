using System.Text.Json.Nodes;
using Microsoft.Playwright;

namespace Fruits
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class FruitsTest : UITestFixture
    {
        [Test]
        public async Task ReplaceResponse()
        {
            await Page.RouteAsync("*/**/api/v1/fruits", async route =>
            {
                var response = await route.FetchAsync();
                var body = await response.BodyAsync();
                var ja = JsonNode.Parse(body)!.AsArray();
                // find item index where name is "Orange" and replace it with "LAST FRUIT"
                int index = -1;
                for (int i = 0; i < ja.Count; i++)
                {
                    if (ja[i]!["name"]?.ToString() == "Orange")
                    {
                        index = i;
                        ja[index]!["name"] = "LAST FRUIT";
                        break;
                    }
                }
                // create new array with elements up to the found index
                var newArray = index != -1 ? new JsonArray(ja.Take(index + 1).Select(item => JsonNode.Parse(item!.ToString())).ToArray()) : ja;
                await route.FulfillAsync(new() { Response = response, Json = newArray });
            });
            await Page.GotoAsync("https://demo.playwright.dev/api-mocking");
            await Assertions.Expect(Page.GetByText("Raspberry")).ToBeHiddenAsync();
            await Assertions.Expect(Page.GetByText("LAST FRUIT")).ToBeVisibleAsync();
        }
    }
}