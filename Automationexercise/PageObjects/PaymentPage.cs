using AutomationExercise;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class PaymentPage (IPage page) : BasePage(page)
{
    private readonly IPage _page = page;
    
    //Selectors & Locators
    private ILocator PayAndConfirmOrderButton => _page.Locator("[data-qa='pay-button']");
    
    //Methods
    public async Task EnterPaymentDetails(string nameOnCard, string cardNumber, string cvc, string expirationMonth, string expirationYear)
    {
        if (nameOnCard == null) throw new ArgumentNullException(nameof(nameOnCard));
        if (cardNumber == null) throw new ArgumentNullException(nameof(cardNumber));
        if (cvc == null) throw new ArgumentNullException(nameof(cvc));
        if (expirationMonth == null) throw new ArgumentNullException(nameof(expirationMonth));
        await _page.Locator("[data-qa='name-on-card']").FillAsync(nameOnCard);
        await _page.Locator("[data-qa='card-number']").FillAsync(cardNumber);
        await _page.Locator("[data-qa='cvc']").FillAsync(cvc);
        await _page.Locator("[data-qa='expiry-month']").FillAsync(expirationMonth);
        await _page.Locator("[data-qa='expiry-year']").FillAsync(expirationYear);
    }
    
    public async Task ClickPayAndConfirmOrderButton()
    {
        await PayAndConfirmOrderButton.ClickAsync();
    }
}