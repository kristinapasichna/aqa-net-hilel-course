using AutomationExercise;
using AutomationExercise.TestData;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class CheckoutPage (IPage page) : BasePage(page)
{
    private readonly IPage _page = page;
    
    //Selectors & Locators
    private ILocator OrderComment => _page.Locator("[name='message']");
    private ILocator PlaceOrderButton => _page.Locator(".btn[href='/payment']");
    
    //Methods
    public async Task<bool> IsAddressDetailsVisible(Constants user) 
    {
        if (user == null) throw new ArgumentNullException(nameof(user));    
        
        var deliveryAddress = await _page.Locator("//ul[@id='address_delivery']/li").AllAsync();
        if (deliveryAddress == null) throw new InvalidOperationException("Address details are not visible");
        var userDetails = new List<string>
        {
            $"{Constants.Title} {Constants.FirstName} {Constants.LastName}",
            Constants.Company,
            Constants.Address1,
            Constants.Address2,
            $"{Constants.City}, {Constants.State} {Constants.Zipcode}",
            Constants.Country,
            Constants.MobileNumber
        };

        foreach (var deliveryAddressLine in deliveryAddress)
        {
            var fieldValue = await deliveryAddressLine.InnerTextAsync();
            if (userDetails.Any(detail => fieldValue.Contains(detail)))
            {
                return true;
            }
        }
        return false;
    }
    
    public async Task<bool> IsReviewYourOrderVisible(string productName, string quantity)
    {
        if (productName == null) throw new ArgumentNullException(nameof(productName));
        var product = await _page.Locator($"//tr[contains(@id, 'product')]").AllAsync();
        if (product == null) throw new InvalidOperationException("Product list is not visible");
        var result = false;
        foreach (var item in product)
        {
            var name = await item.Locator($"//h4/a[contains(text(), '{productName}')]").IsVisibleAsync();
            if (name)
            {
                var itemQuantity = await item.Locator(".cart_quantity").InnerTextAsync();
                result = itemQuantity.Equals(quantity);
            }
        }
        return result;
    }
    
    public async Task EnterDescriptionInCommentField(string description)
    {
        if (description == null) throw new ArgumentNullException(nameof(description));
        await OrderComment.FillAsync(description);
        await Assertions.Expect(OrderComment).ToHaveValueAsync(description);
    }
    
    public async Task ClickPlaceOrderButton()
    {
        await PlaceOrderButton.ClickAsync();
        await _page.WaitForURLAsync(Constants.BaseUrl + "/payment");
    }
}