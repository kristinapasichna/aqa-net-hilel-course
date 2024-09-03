using AutomationExercise;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class Common (IPage page, string baseUrl) : BasePage(page: page, baseUrl)
{
    //Selectors & Locators
    /* Header menu */
    private ILocator ContactUsButton => page.Locator("[href='/contact_us']");
    private ILocator ProductMenuButton => page.Locator("[href='/products']");
    private ILocator CartMenuButton => page.Locator(".nav [href='/view_cart']");
    private ILocator DeleteAccountButton => page.Locator("[href='/delete_account']");
    public ILocator HomePageTitle => page.Locator("//h2[contains(text(), 'Features Items')]");
    private ILocator HomeButton => page.Locator(".btn-success");
    /* footer */
    private ILocator Footer => page.Locator("#footer");
    public ILocator SubscriptionTitle => page.Locator("//div/h2[text()= 'Subscription']");
    private ILocator SubscribeEmailInput => page.Locator("#susbscribe_email");
    private ILocator SubscribeButton => page.Locator("button#subscribe");
    public ILocator SubscribeSuccessMessage => page.Locator("#success-subscribe");
    
    //Methods
    public async Task ClickContactUsButton()
    {
        await ContactUsButton.ClickAsync();
        await page.WaitForURLAsync(baseUrl +"/contact_us");
    }
    
    public async Task ClickHomeButton()
    {
        await HomeButton.ClickAsync();
        await page.Locator(".features_items").ClickAsync();
    }
    
    public async Task ClickProductsButton()
    {
        await ProductMenuButton.ClickAsync();
        await page.WaitForURLAsync(baseUrl + "/products");
    }
    
    public async Task ClickCartButton()
    {
        await CartMenuButton.ClickAsync();
        await page.WaitForURLAsync(baseUrl + "/view_cart");
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
        await page.WaitForURLAsync(baseUrl + "/delete_account");
    }
}