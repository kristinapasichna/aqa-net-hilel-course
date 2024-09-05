using AutomationExercise;
using AutomationExercise.TestData;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class Common(IPage page) : BasePage(page)
{
    private readonly IPage _page = page;

    //Selectors & Locators
    /* Header menu */
    private ILocator ContactUsButton => _page.Locator("[href='/contact_us']");
    private ILocator ProductMenuButton => _page.Locator("[href='/products']");
    private ILocator CartMenuButton => _page.Locator(".nav [href='/view_cart']");
    public ILocator HomePageTitle => _page.Locator("//h2[contains(text(), 'Features Items')]");
    private ILocator HomeButton => _page.Locator(".btn-success");
    /* footer */
    private ILocator Footer => _page.Locator("#footer");
    public ILocator SubscriptionTitle => _page.Locator("//div/h2[text()= 'Subscription']");
    private ILocator SubscribeEmailInput => _page.Locator("#susbscribe_email");
    private ILocator SubscribeButton => _page.Locator("button#subscribe");
    public ILocator SubscribeSuccessMessage => _page.Locator("#success-subscribe");
    
    //Methods
    public async Task OpenContactUsPage()
    {
        await ContactUsButton.ClickAsync();
        await _page.WaitForURLAsync(Constants.BaseUrl +"/contact_us");
        await _page.GetByText("Get In Touch").WaitForAsync();
    }
    
    public async Task OpenHomePage()
    {
        await HomeButton.ClickAsync();
        await _page.Locator(".features_items").ClickAsync();
    }
    
    public async Task OpenProductsPage()
    {
        await ProductMenuButton.ClickAsync();
        await _page.WaitForURLAsync(Constants.BaseUrl + "/products");
    }
    
    public async Task OpenCartPage()
    {
        await CartMenuButton.ClickAsync();
        await _page.WaitForURLAsync(Constants.BaseUrl + "/view_cart");
    }
    
    public async Task ScrollToFooter()
    {
        await Footer.ScrollIntoViewIfNeededAsync();
    }
    
    public async Task EnterEmailInSubscriptionInput(string email)
    {
        ArgumentNullException.ThrowIfNull(email);
        await SubscribeEmailInput.FillAsync(email);
        await Assertions.Expect(SubscribeEmailInput).ToHaveValueAsync(email);
    }

    public async Task ClickSubscriptionArrowButton()
    {
        await SubscribeButton.ClickAsync();
    }
}