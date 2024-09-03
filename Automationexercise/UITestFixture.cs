using System.Text;
using AutomationExercise.API;
using Microsoft.Playwright;

namespace AutomationExercise
{
    public class UiTestFixture
    {
        protected IPage Page { get; private set; }
        private IBrowser _browser;
        private UserApi UserApi { get; set; }

        protected const string BaseUrl = "https://automationexercise.com";
        private const string AuthStorageFilePath = "../../../playwright/.auth/state.json";
        public const string UploadFilePath = "../../../Uploads/contact_us.png";
        public class User
        {
            public string Name = "MartaDoe";
            public string Email = "kristest@mailinator.com";
            public string Password = "qwerty123";
            public string Title = "Mrs";
            public string BirthDate = "01";
            public string BirthMonth = "10";
            public string BirthYear = "2000";
            public string FirstName = "Marta";
            public string LastName = "Doe";
            public string Company = "Bingo";
            public string Address1 = "My Address 1";
            public string Address2 = "My Address 2";
            public string Country = "United States";
            public string Zipcode = "12345";
            public string State = "Alabama";
            public string City = "Birmingham";
            public string MobileNumber = "1234567890";
        }
        
        [SetUp]
        public async Task Setup()
        {
            var playwrightDriver = await Playwright.CreateAsync();
            _browser = await playwrightDriver.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });
            
            var fileInfo = new FileInfo(AuthStorageFilePath);

            if (!fileInfo.Exists)
            {
                Directory.CreateDirectory(fileInfo.Directory!.FullName);
                using (FileStream fs = File.Create(AuthStorageFilePath))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("");
                    fs.Write(info, 0, info.Length);
                }
            }
            
            var context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize
                {
                    Width = 1920,
                    Height = 1080
                },
                StorageStatePath = AuthStorageFilePath
            });
            
            Page = await context.NewPageAsync();
            
            UserApi = new UserApi(Page, BaseUrl);
            var user = new User();
            var userDetails = await UserApi.GetUserDetailByEmailAsync(user);
            
            if (userDetails.Contains(user.Email) == false)
            {
                var isUserCreated = await UserApi.CreateUserAsync(user);
                if (isUserCreated == false) throw new Exception("User was not created");
            }
            
            await Page.GotoAsync(BaseUrl);
            var userLoggedIn = Page.Locator($"//li/*[contains(text(),'Logged in as')]/*[contains(text(),'{user.Name}')]");
            if (await userLoggedIn.IsHiddenAsync())
            {
                await Page.GotoAsync(BaseUrl + "/login");
                await Page.Locator("[data-qa='login-email']").FillAsync(user.Email);
                await Page.Locator("[data-qa='login-password']").FillAsync(user.Password);
                await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
                await userLoggedIn.IsVisibleAsync();
                await context.StorageStateAsync(new()
                {
                    Path = AuthStorageFilePath
                });
            }
        }
        
        [TearDown]
        public async Task Teardown()
        {
            await Page.CloseAsync();
            await _browser.CloseAsync();
            var isUserDeleted = await UserApi.DeleteUserAsync(new User()); //TODO verify if user is deleted
            if (isUserDeleted == null) throw new Exception("User was not deleted");
        }
    }
}
