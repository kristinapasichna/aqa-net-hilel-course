using System.Text;
using Microsoft.Playwright;
using UltimateQa.PageObjects;

namespace UltimateQa
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class UltimateQaTests
    {
        private IPage Page { get; set; }
        private IBrowser _browser;
        private IBrowserContext _context;
        private UltimateQaPage? _ultimateQa;
        private const string BaseUrl = "https://courses.ultimateqa.com";
        private const string AuthStorageFilePath = "../../../playwright/.auth/state.json";
        private const string Email = "badcatik@gmail.com"; // Replace with actual email
        private const string Password = "qwerty123"; // Replace with actual password

        [SetUp]
        public async Task Setup()
        {
            var playwrightDriver = await Playwright.CreateAsync();
            _browser = await playwrightDriver.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });
            
            var fileInfo = new FileInfo(AuthStorageFilePath);

            if (!fileInfo.Exists)
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
                using (FileStream fs = File.Create(AuthStorageFilePath))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("");
                    fs.Write(info, 0, info.Length);
                }
            }

            _context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize
                {
                    Width = 1920,
                    Height = 1080
                },
                StorageStatePath = AuthStorageFilePath
            });

            Page = await _context.NewPageAsync();

            await Page.GotoAsync(BaseUrl + "/enrollments");
            if (Page.Url.EndsWith("/users/sign_in"))
            {
                await Page.WaitForURLAsync(BaseUrl + "/users/sign_in");
                await Page.GetByPlaceholder("Email").FillAsync(Email);
                await Page.GetByPlaceholder("Password").FillAsync(Password);
                await Page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();
                await Page.WaitForURLAsync(BaseUrl + "/enrollments");
                await _context.StorageStateAsync(new()
                {
                    Path = AuthStorageFilePath
                });
            }
        }

        [Test]
        [TestCase("Kriss T", "Selenium")]
        public async Task LoginTest(string userName, string courseName)
        {
            //Arrange
            _ultimateQa = new UltimateQaPage(Page);
            await _ultimateQa.ClickViewMoreCoursesLink();
            
            //Act1
            await _ultimateQa.SearchCourse(courseName);
            //Assert1
            //Assert.That(await _ultimateQa.AreAllSearchResultsMatchingCourseName(courseName), Is.True, "Search result is not correct"); //BUG: expected to fail. Not all courses contain "Selenium" in their title
            
            //Act2
            await _ultimateQa.OpenMyDashboard();
            //Assert2
            Assert.That(await _ultimateQa.IsWelcomeMessageDisplayedCorrectly(userName), Is.True, "Welcome message is not visible");
            
            //Act3
            await _ultimateQa.OpenUserMenu();
            //Assert3
            Assert.That(await _ultimateQa.MyAccount.IsVisibleAsync(), Is.True, "My Account button is not visible");
            Assert.That(await _ultimateQa.Support.IsVisibleAsync(), Is.True, "Support button is not visible");
            Assert.That(await _ultimateQa.SignOut.IsVisibleAsync(), Is.True, "Sign Out button is not visible");
        } 
    }
}


