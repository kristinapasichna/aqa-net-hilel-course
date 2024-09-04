using AutomationExercise;
using AutomationExercise.Models;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class CheckoutPage (IPage page, string baseUrl) : BasePage(page: page, baseUrl)
{
    //Selectors & Locators
    private ILocator OrderComment => Page.Locator("[name='message']");
    private ILocator PlaceOrderButton => Page.Locator(".btn[href='/payment']");
    
    //Methods
    public async Task<bool> IsAddressDetailsVisible(object user) 
    {
        if (user == null) throw new ArgumentNullException(nameof(user));    
        
        var deliveryAddress = await Page.Locator("//ul[@id=\"address_delivery\"]/li").AllAsync();
        if (deliveryAddress == null) throw new InvalidOperationException("Address details are not visible");
        var userDetails = new List<string>
        {
            $"{((User)user).Title} {((User)user).FirstName} {((User)user).LastName}",
            ((User)user).Company,
            ((User)user).Address1,
            ((User)user).Address2,
            $"{((User)user).City}, {((User)user).State} {((User)user).Zipcode}",
            ((User)user).Country,
            ((User)user).MobileNumber
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
        var product = await Page.Locator($"//tr[contains(@id, 'product')]").AllAsync();
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
        await Page.WaitForURLAsync(BaseUrl + "/payment");
    }
}