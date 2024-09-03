using AutomationExercise;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class DeleteAccountPage (IPage page, string baseUrl) : BasePage(page: page, baseUrl)
{
    public ILocator AccountDeletedMessage => page.Locator("[data-qa='account-deleted']");
    private ILocator ContinueButton => page.Locator("[data-qa='continue-button']");
        
    public async Task ClickContinueButton()
    {
        await ContinueButton.ClickAsync();
        await ContinueButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Hidden });
    }
}