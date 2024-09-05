using AutomationExercise;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class ProductDetailsPage (IPage page) : BasePage(page)
{
    private readonly IPage _page = page;
    
    // Selectors & Locators
    private ILocator QuantityInput => _page.Locator("input#quantity");
    public ILocator ProductName => _page.Locator("//div[@class='product-information']/h2");
    public ILocator ProductCategory => _page.Locator("//div[@class=\"product-information\"]/p[contains(text(),'Category')]");
    public ILocator ProductPrice => _page.Locator("//div[@class=\"product-information\"]/span/span");
    public ILocator ProductAvailability => _page.Locator("//div[@class=\"product-information\"]/p/*[contains(text(),'Availability')]/..");
    public ILocator ProductCondition => _page.Locator("//div[@class=\"product-information\"]/p/*[contains(text(),'Condition:')]/..");
    public ILocator ProductBrand => _page.Locator("//div[@class=\"product-information\"]/p/*[contains(text(),'Brand:')]/..");
    private ILocator AddToCartButton => _page.Locator("button.cart");

    //Methods
    public async Task SelectQuantity(string quantity)
    {
        if (quantity == null) throw new ArgumentNullException(nameof(quantity)); 
        await QuantityInput.FillAsync(quantity);
        await Assertions.Expect(QuantityInput).ToHaveValueAsync(quantity);
    }

    public async Task ClickAddToCartButton()
    {
        await AddToCartButton.ClickAsync();
    }
}