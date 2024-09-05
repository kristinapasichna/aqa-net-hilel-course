using AutomationExercise;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class ProductsPage (IPage page) : BasePage(page)
{
    private readonly IPage _page = page;
    
    // Selectors & Locators
    private ILocator ProductList => _page.Locator(".features_items");
    private ILocator SearchInput => _page.Locator("[placeholder='Search Product']");
    private ILocator SearchButton => _page.Locator(".fa-search");
    
    //Methods
    public async Task OpenViewProductPage(string productName)
    {
        var productLocator = await _page.QuerySelectorAsync($"//p[contains(text(), '{productName}')]");
        if (productLocator == null) throw new InvalidOperationException($"{productName} product not found.");
        var viewProductLink = await productLocator.QuerySelectorAsync("//ancestor::div[@class=\"product-image-wrapper\"]//div[@class='choose']//a[contains(text(), 'View Product')]");
        if (viewProductLink == null) throw new InvalidOperationException("View Product link not found.");
        await viewProductLink.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.Load);
        await _page.Locator(".product-information").IsVisibleAsync(); 
    }
    
    public async Task<bool> ProductListIsVisible()
    {
        var list = await ProductList.AllAsync();
        return list.Count > 0;
    }
    
    public async Task SearchProduct(string productName)
    {
        await SearchInput.PressSequentiallyAsync(productName);
        await SearchButton.ClickAsync();
        await _page.WaitForLoadStateAsync();
    }
    
    public async Task<bool> IsProductListContainsRelatedProducts(string productName)
    {
        if(productName == null) throw new ArgumentNullException(nameof(productName));
        var productList = await _page.Locator("//div[contains(@class, 'productinfo')]/p").AllInnerTextsAsync();
        foreach (var product in productList)
        {
            if (product.ToLower().Contains(productName.ToLower()))
            {
                return true;
            }
        }
        return false;
    }
}