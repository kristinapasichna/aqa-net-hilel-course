using AutomationExercise;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class ProductDetailsPage (IPage page, string baseUrl) : BasePage(page: page, baseUrl)
{
    // Selectors & Locators
    private ILocator QuantityInput => Page.Locator("input#quantity");
    public ILocator ProductName => Page.Locator("//div[@class='product-information']/h2");
    public ILocator ProductCategory => Page.Locator("//div[@class=\"product-information\"]/p[contains(text(),'Category')]");
    public ILocator ProductPrice => Page.Locator("//div[@class=\"product-information\"]/span/span");
    public ILocator ProductAvailability => Page.Locator("//div[@class=\"product-information\"]/p/*[contains(text(),'Availability')]/..");
    public ILocator ProductCondition => Page.Locator("//div[@class=\"product-information\"]/p/*[contains(text(),'Condition:')]/..");
    public ILocator ProductBrand => Page.Locator("//div[@class=\"product-information\"]/p/*[contains(text(),'Brand:')]/..");
    private ILocator AddToCartButton => Page.Locator("button.cart");

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