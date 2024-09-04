using AutomationExercise;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class ProductsPage (IPage page, string baseUrl) : BasePage(page: page, baseUrl)
{
    // Selectors & Locators
    private ILocator ProductList => Page.Locator(".features_items");
    private ILocator SearchInput => Page.Locator("[placeholder='Search Product']");
    private ILocator SearchButton => Page.Locator(".fa-search");
    
    //Methods
    public async Task ClickViewProductButton(string productName)
    {
        if(productName == null) throw new ArgumentNullException(nameof(productName));
        var productLocator = await Page.QuerySelectorAsync($"//p[contains(text(), '{productName}')]");
        if (productLocator == null) throw new InvalidOperationException($"{productName} product not found.");
        var viewProductLink = await productLocator.QuerySelectorAsync("//ancestor::div[@class='product-image-wrapper']//div[@class='choose']//a[contains(text(), 'View Product')]");
        if (viewProductLink == null) throw new InvalidOperationException("View Product link not found.");
        await viewProductLink.ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.Load);
        await Page.Locator(".product-information").IsVisibleAsync(); 
    }
    
    public async Task<bool> ProductListIsVisible()
    {
        var list = await ProductList.AllAsync();
        if (list.Count == 0 || list == null) throw new InvalidOperationException("Product list is not visible");
        return list.Count > 0;
    }
    
    public async Task SearchProduct(string productName)
    {
        if(productName == null) throw new ArgumentNullException(nameof(productName));
        await SearchInput.PressSequentiallyAsync(productName);
        await SearchButton.ClickAsync();
        await Page.WaitForLoadStateAsync();
    }
    
    public async Task<bool> IsProductListContainsRelatedProducts(string productName)
    {
        if(productName == null) throw new ArgumentNullException(nameof(productName));
        var productList = await Page.Locator("//div[contains(@class, 'productinfo')]/p").AllInnerTextsAsync();
        if (productList == null) throw new InvalidOperationException("Product list is not visible");
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