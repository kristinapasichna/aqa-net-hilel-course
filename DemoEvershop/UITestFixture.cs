using Microsoft.Playwright;

namespace DemoEverShop
{
    public class UiTestFixture
    {
        public const string BaseUrl = "https://demo.evershop.io";
        public IPage Page { get; private set; }
        private IBrowser _browser;
        public IAPIRequestContext ApiContext;

        [SetUp]
        public async Task Setup()
        {
            var playwrightDriver = await Playwright.CreateAsync();
            _browser = await playwrightDriver.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });

            var context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize
                {
                    Width = 1920,
                    Height = 1080
                }
            });

            Page = await context.NewPageAsync();
            Page.SetDefaultTimeout(10000);
            ApiContext = await playwrightDriver.APIRequest.NewContextAsync();
            var headers = new Dictionary<string, string> { { "Accept", "application/json" } };
            ApiContext = await playwrightDriver.APIRequest.NewContextAsync(new APIRequestNewContextOptions
            {
                BaseURL = BaseUrl,
                ExtraHTTPHeaders = headers,
            });
        }
        
        [TearDown]
        public async Task Teardown()
        {
            await Page.CloseAsync();
            await _browser.CloseAsync();
        }
    }
}
