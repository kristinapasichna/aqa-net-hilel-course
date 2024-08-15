using Microsoft.Playwright;

namespace DemoEverShop.PageObjects;

internal class DemoShopPage(IPage page)
{
    private ILocator KidsCatalogLink => page.GetByRole(AriaRole.Link, new() { Name = "Shop kids" });
    private ILocator AddToCartButton => page.GetByRole(AriaRole.Button, new() { Name = "ADD TO CART" });
    private ILocator ViewCartButton => page.Locator(".add-cart-popup-button");
    private ILocator CheckoutButton => page.GetByRole(AriaRole.Link, new() { Name = "CHECKOUT" });
    private ILocator Loader => page.Locator(".loading-bar");
    public ILocator CheckoutForm => page.Locator("#checkoutShippingAddressForm");

    public async Task GoToDemoEverShopPage()
    {
        await page.GotoAsync(UiTestFixture.BaseUrl);
    }
    
    public async Task OpenKidsShoesCatalog()
    {
        await KidsCatalogLink.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        await KidsCatalogLink.ClickAsync();
    }
    
    public async Task OpenProductDetails(string productName)
    {
        await page.Locator($"//a/span[text()='{productName}']").WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        await page.Locator($"//a/span[text()='{productName}']").ClickAsync();
        await Task.Delay(1500);
    }
    
    private async Task WaitForLoader()
    {
        await Loader.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        await Loader.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Hidden });
    }

    public async Task SelectColor(string color)
    {
        var colorOption = page.Locator($"//li/a[text()='{color}']");
        var colorSelected = page.Locator($"//li[@class='selected']/a[text()='{color}']");
        await colorOption.ClickAsync();
        await WaitForLoader();
        Assert.That(await colorSelected.IsVisibleAsync(), "Color is not selected");
    }

    public async Task SelectSize(string size)
    {
        var sizeOption = page.Locator($"//li/a[text()='{size}']");
        var sizeSelected = page.Locator($"//li[@class='selected']/a[text()='{size}']");
        await sizeOption.ClickAsync();
        await WaitForLoader();
        Assert.That(await sizeSelected.IsVisibleAsync(), "Size is not selected");
    }
    
    public async Task ClickAddToCartButton()
    {
        await AddToCartButton.ClickAsync();
        await ViewCartButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
    }
    
    public async Task ClickViewCartButton()
    {
        await ViewCartButton.ClickAsync();
        await CheckoutButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
    }

    public async Task ClickCheckoutButton()
    {
        await CheckoutButton.ClickAsync();
        await page.WaitForURLAsync(UiTestFixture.BaseUrl + "/checkout");
    }
}