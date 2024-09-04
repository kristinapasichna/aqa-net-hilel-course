using AutomationExercise;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class Common (IPage page, string baseUrl) : BasePage(page: page, baseUrl)
{
    //Selectors & Locators
    /* Header menu */
    private ILocator ContactUsButton => Page.Locator("[href='/contact_us']");
    private ILocator ProductMenuButton => Page.Locator("[href='/products']");
    private ILocator CartMenuButton => Page.Locator(".nav [href='/view_cart']");
    private ILocator DeleteAccountButton => Page.Locator("[href='/delete_account']");
    public ILocator HomePageTitle => Page.Locator("//h2[contains(text(), 'Features Items')]");
    private ILocator HomeButton => Page.Locator(".btn-success");
    /* footer */
    private ILocator Footer => Page.Locator("#footer");
    public ILocator SubscriptionTitle => Page.Locator("//div/h2[text()= 'Subscription']");
    private ILocator SubscribeEmailInput => Page.Locator("#susbscribe_email");
    private ILocator SubscribeButton => Page.Locator("button#subscribe");
    public ILocator SubscribeSuccessMessage => Page.Locator("#success-subscribe");
    
    //Methods
    public async Task ClickContactUsButton()
    {
        await ContactUsButton.ClickAsync();
        await Page.WaitForURLAsync(BaseUrl +"/contact_us");
    }
    
    public async Task ClickHomeButton()
    {
        await HomeButton.ClickAsync();
        await Page.Locator(".features_items").ClickAsync();
    }
    
    public async Task ClickProductsButton()
    {
        await ProductMenuButton.ClickAsync();
        await Page.WaitForURLAsync(BaseUrl + "/products");
    }
    
    public async Task ClickCartButton()
    {
        await CartMenuButton.ClickAsync();
        await Page.WaitForURLAsync(BaseUrl + "/view_cart");
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
    
    public async Task ClickDeleteAccountButton()
    {
        await DeleteAccountButton.ClickAsync();
        await Page.WaitForURLAsync(BaseUrl + "/delete_account");
    }
}