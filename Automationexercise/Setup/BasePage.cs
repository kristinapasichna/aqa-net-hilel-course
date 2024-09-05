using Microsoft.Playwright;

namespace AutomationExercise;

public abstract class BasePage (IPage page)
{
    protected IPage Page => page;
  
}