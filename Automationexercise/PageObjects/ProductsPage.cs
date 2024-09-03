using AutomationExercise;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class ProductsPage (IPage page, string baseUrl) : BasePage(page: page, baseUrl)
{
    // Selectors & Locators
    private ILocator ProductList => page.Locator(".features_items");
    private ILocator SearchInput => page.Locator("[placeholder='Search Product']");
    private ILocator SearchButton => page.Locator(".fa-search");
    
    //Methods
    public async Task ClickViewProductButton(string productName)
    {
        if(productName == null) throw new ArgumentNullException(nameof(productName));
        await page.Locator($"[href='/product_details/{productName}']").ClickAsync(); //TODO fix locator
        await page.WaitForLoadStateAsync(LoadState.Load);
        await page.Locator(".product-information").IsVisibleAsync(); 
    }
    
    public async Task<bool> ProductListIsVisible()
    {
        var list = await ProductList.AllAsync();
        if (list.Count == 0 || list == null) throw new InvalidOperationException("Product list is not visible");
        var result = list.Count > 0;
        return result;
    }
    
    public async Task SearchProduct(string productName)
    {
        if(productName == null) throw new ArgumentNullException(nameof(productName));
        await SearchInput.PressSequentiallyAsync(productName);
        await SearchButton.ClickAsync();
        await page.WaitForLoadStateAsync();
    }
    
    public async Task<bool> IsProductListContainsRelatedProducts(string productName)
    {
        if(productName == null) throw new ArgumentNullException(nameof(productName));
        var productList = await page.Locator("//div[contains(@class, 'productinfo')]/p").AllInnerTextsAsync();
        if (productList == null) throw new InvalidOperationException("Product list is not visible");
        var result = false;
        foreach (var product in productList)
        {
            if (product.ToLower().Contains(productName.ToLower()))
            {
                result = true;
                break;
            }
        }
        return result;
    }

}