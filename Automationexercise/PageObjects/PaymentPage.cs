using AutomationExercise;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class PaymentPage (IPage page, string baseUrl) : BasePage(page: page, baseUrl)
{
    //Selectors & Locators
    private ILocator PayAndConfirmOrderButton => Page.Locator("[data-qa='pay-button']");
    
    //Methods
    public async Task EnterPaymentDetails(string nameOnCard, string cardNumber, string cvc, string expirationMonth, string expirationYear)
    {
        if (nameOnCard == null) throw new ArgumentNullException(nameof(nameOnCard));
        if (cardNumber == null) throw new ArgumentNullException(nameof(cardNumber));
        if (cvc == null) throw new ArgumentNullException(nameof(cvc));
        if (expirationMonth == null) throw new ArgumentNullException(nameof(expirationMonth));
        await Page.Locator("[data-qa='name-on-card']").FillAsync(nameOnCard);
        await Page.Locator("[data-qa='card-number']").FillAsync(cardNumber);
        await Page.Locator("[data-qa='cvc']").FillAsync(cvc);
        await Page.Locator("[data-qa='expiry-month']").FillAsync(expirationMonth);
        await Page.Locator("[data-qa='expiry-year']").FillAsync(expirationYear);
    }
    
    public async Task ClickPayAndConfirmOrderButton()
    {
        await PayAndConfirmOrderButton.ClickAsync();
    }
}