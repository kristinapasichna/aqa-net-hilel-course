using AutomationExercise;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class Cart (IPage page, string baseUrl) : BasePage(page: page, baseUrl)
{
    //Selectors & Locators
    public ILocator GoShoppingButton => Page.Locator("#cartModal .btn-success");
    private ILocator ViewCartLink => Page.Locator("#cartModal [href='/view_cart']");
    private ILocator ProceedToCheckoutButton => Page.Locator(".btn.check_out");

    //Methods
    public async Task AddToCart(string productName)
    {
        if (productName == null) throw new ArgumentNullException(nameof(productName));
        var cartButton = Page.Locator($"//div[@class=\"features_items\"]//div[contains(@class,'productinfo ')]/p[contains(text(),'{productName}')]");
        var addToCartButton = Page.Locator($"//div[@class=\"features_items\"]//div[@class=\"product-overlay\"]//p[contains(text(),'{productName}')]/..//a[contains(@class, 'add-to-cart')]");
        await cartButton.ScrollIntoViewIfNeededAsync();
        await cartButton.HoverAsync();
        await addToCartButton.ClickAsync();
        await GoShoppingButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
    }
    
    public async Task ClickViewCartButton()
    {
        await ViewCartLink.ClickAsync();
        await Page.WaitForURLAsync(BaseUrl + "/view_cart");
    }  
    
    public async Task<bool> IsCartPageVisible()
    {
        await Page.WaitForURLAsync(BaseUrl + "/view_cart");
        return await Page.Locator("#cart_info").IsVisibleAsync();
    }
    
    public async Task<string> GetProductQuantity(string productName)
    {
        if (productName == null) throw new ArgumentNullException(nameof(productName));
        var products = await Page.Locator("//tr[contains(@id, 'product')]").AllAsync();
        if (products == null || products.Count == 0) throw new InvalidOperationException("Product list is not visible");
        foreach (var product in products)
        {
            if (await product.Locator($"//h4/a[contains(text(), '{productName}')]").IsVisibleAsync())
            {
             return await product.Locator(".cart_quantity").InnerTextAsync();
            }
        }
        return string.Empty;
    }
    
    public async Task ClickProceedToCheckoutButton()
    {
        await ProceedToCheckoutButton.ClickAsync();
        await Page.WaitForURLAsync(BaseUrl + "/checkout");
    }
}