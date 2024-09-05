using System.Text;
using AutomationExercise.API;
using AutomationExercise.TestData;
using Microsoft.Playwright;

namespace Automationexercise.Setup;

public class UiTestFixture
{
    public IPage Page { get; private set; }
    public IBrowserContext Context { get; set; }
    
    private IBrowser _browser;
    private Constants _constants;
    private UserApi _userApi; 
        
    [OneTimeSetUp]
    public async Task Setup()
    {
        _constants = new Constants();
        _userApi = new UserApi(Constants.BaseUrl);
                
        var userDetails = await _userApi.GetUserDetailByEmailAsync(Constants.Email);

        if (userDetails.Contains(Constants.Email) == false)
        {
            var isUserCreated = await _userApi.CreateUserAsync(_constants);
            if (isUserCreated == false) throw new Exception("User was not created");
        }
    }

    [SetUp]
    public async Task SetUp()
    {
        var playwrightDriver = await Playwright.CreateAsync();
        _browser = await playwrightDriver.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });

        var fileInfo = new FileInfo(Constants.AuthStorageFilePath);

        if (!fileInfo.Exists)
        {
                Directory.CreateDirectory(fileInfo.Directory!.FullName);
                using (FileStream fs = File.Create(Constants.AuthStorageFilePath))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("");
                    fs.Write(info, 0, info.Length);
                }
        }

        Context = await _browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize {
                    Width = 1920,
                    Height = 1080
            },
            StorageStatePath = Constants.AuthStorageFilePath
        });

        Page = await Context.NewPageAsync();
        await Page.GotoAsync(Constants.BaseUrl);
            
        var userLoggedIn = Page.Locator($"//li/*[contains(text(),'Logged in as')]/*[contains(text(),'{Constants.Name}')]");
        if (await userLoggedIn.IsHiddenAsync())
        {
            await Page.GotoAsync(Constants.BaseUrl + "/login");
            await Page.Locator("[data-qa='login-email']").FillAsync(Constants.Email);
            await Page.Locator("[data-qa='login-password']").FillAsync(Constants.Password);
            await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
                
            if (await userLoggedIn.IsVisibleAsync() == false) 
            {
                throw new Exception("User was not logged in");
            }
            await Context.StorageStateAsync(new() 
            {
                    Path = Constants.AuthStorageFilePath 
            });
        }
    }

    [TearDown]
    public async Task TearDown()
    {
        await Page.CloseAsync();
        await _browser.CloseAsync();
    }
    
    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        var userDetails = await _userApi.GetUserDetailByEmailAsync(Constants.Email);
        if (userDetails.Contains(Constants.Email))
        {
            var isUserDeleted = await _userApi.DeleteUserAsync(_constants);
            if (isUserDeleted == null) throw new Exception("User was not deleted");
            Assert.That(isUserDeleted, Is.EqualTo("{\"responseCode\": 200, \"message\": \"Account deleted!\"}"), "User was not deleted");
        }
        else
        {
            Console.WriteLine("User not found");
        }
    }   

}