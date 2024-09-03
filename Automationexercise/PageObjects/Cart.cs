using AutomationExercise;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class Cart (IPage page, string baseUrl) : BasePage(page, baseUrl)
{
    //Selectors & Locators
    public ILocator GoShoppingButton => page.Locator("#cartModal .btn-success");
    private ILocator ViewCartLink => page.Locator("#cartModal [href='/view_cart']");
    private ILocator ProceedToCheckoutButton => page.Locator(".btn.check_out");

    //Methods
    public async Task AddToCart(string productName)
    {
        if (productName == null) throw new ArgumentNullException(nameof(productName));
        var cartButton = page.Locator($"//div[@class=\"features_items\"]//div[contains(@class,'productinfo ')]/p[contains(text(),'{productName}')]");
        var addToCartButton = page.Locator($"//div[@class=\"features_items\"]//div[@class=\"product-overlay\"]//p[contains(text(),'{productName}')]/..//a[contains(@class, 'add-to-cart')]");
        await cartButton.ScrollIntoViewIfNeededAsync();
        await cartButton.HoverAsync();
        await addToCartButton.ClickAsync();
        await GoShoppingButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
    }
    
    public async Task ClickViewCartButton()
    {
        await ViewCartLink.ClickAsync();
        await page.WaitForURLAsync(baseUrl + "/view_cart");
    }  
    
    public async Task<bool> IsCartPageVisible()
    {
        await page.WaitForURLAsync(baseUrl + "/view_cart");
        var isVisible = await page.Locator("#cart_info").IsVisibleAsync();
        return isVisible;
    }
    
    public async Task<string> ProductQuantity(string productName)
    {
        if (productName == null) throw new ArgumentNullException(nameof(productName));
        var product = await page.Locator($"//tr[contains(@id, 'product')]").AllAsync();
        if (product == null) throw new InvalidOperationException("Product list is not visible");
        foreach (var item in product)
        {
            var name = await item.Locator($"[href='/product_details/{productName}']").IsVisibleAsync();
            if (name)
            {
                var quantity = await item.Locator(".cart_quantity").InnerTextAsync();
                return quantity;
            }
        }
        return string.Empty;
    }
    
    public async Task ClickProceedToCheckoutButton()
    {
        await ProceedToCheckoutButton.ClickAsync();
        await page.WaitForURLAsync(baseUrl + "/checkout");
    }
}