using AutomationExercise;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class CheckoutPage (IPage page, string baseUrl) : BasePage(page: page, baseUrl)
{
    //Selectors & Locators
    private ILocator OrderComment => page.Locator("[name='message']");
    private ILocator PlaceOrderButton => page.Locator(".btn[href='/payment']");
    
    //Methods
    public async Task<bool> IsAddressDetailsVisible(object user) 
    {
        if (user == null) throw new ArgumentNullException(nameof(user));    
        
        var deliveryAddress = await page.Locator("//ul[@id=\"address_delivery\"]/li").AllAsync();
        if (deliveryAddress == null) throw new InvalidOperationException("Address details are not visible");
        var userDetails = new List<string>
        {
            $"{((UiTestFixture.User)user).Title} {((UiTestFixture.User)user).FirstName} {((UiTestFixture.User)user).LastName}",
            ((UiTestFixture.User)user).Company,
            ((UiTestFixture.User)user).Address1,
            ((UiTestFixture.User)user).Address2,
            $"{((UiTestFixture.User)user).City}, {((UiTestFixture.User)user).State} {((UiTestFixture.User)user).Zipcode}",
            ((UiTestFixture.User)user).Country,
            ((UiTestFixture.User)user).MobileNumber
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
        var product = await page.Locator($"//tr[contains(@id, 'product')]").AllAsync();
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
        await page.WaitForURLAsync(baseUrl + "/payment");
    }

}