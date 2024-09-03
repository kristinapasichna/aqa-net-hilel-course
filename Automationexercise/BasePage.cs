using Microsoft.Playwright;

namespace AutomationExercise;

public abstract class BasePage (IPage page, string baseUrl)
{
    protected IPage Page => page;
    protected string BaseUrl => baseUrl;
}