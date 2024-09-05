using AutomationExercise;
using AutomationExercise.TestData;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class Cart (IPage page) : BasePage(page)
{
    private readonly IPage _page = page;
    //Selectors & Locators
    public ILocator GoShoppingButton => _page.Locator("#cartModal .btn-success");
    private ILocator ViewCartLink => _page.Locator("#cartModal [href='/view_cart']");
    private ILocator ProceedToCheckoutButton => _page.Locator(".btn.check_out");

    //Methods
    public async Task AddToCart(string productName)
    {
        if (productName == null) throw new ArgumentNullException(nameof(productName));
        var cartButton = _page.Locator($"//div[@class='features_items']//div[contains(@class,'productinfo ')]/p[contains(text(),'{productName}')]");
        var addToCartButton = _page.Locator($"//div[@class='features_items']//div[@class='product-overlay']//p[contains(text(),'{productName}')]/..//a[contains(@class, 'add-to-cart')]");
        await cartButton.ScrollIntoViewIfNeededAsync();
        await cartButton.HoverAsync();
        await addToCartButton.ClickAsync();
        await GoShoppingButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
    }
    
    public async Task ClickViewCartButton()
    {
        await ViewCartLink.ClickAsync();
        await _page.WaitForURLAsync(Constants.BaseUrl + "/view_cart");
    }  
    
    public async Task<bool> IsCartPageVisible()
    {
        await _page.WaitForURLAsync(Constants.BaseUrl + "/view_cart");
        return await _page.Locator("#cart_info").IsVisibleAsync();
    }
    
    public async Task<string> GetProductQuantity(string productName)
    {
        if (productName == null) throw new ArgumentNullException(nameof(productName));
        var products = await _page.Locator("//tr[contains(@id, 'product')]").AllAsync();
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
        await _page.WaitForURLAsync(Constants.BaseUrl + "/checkout");
    }
}