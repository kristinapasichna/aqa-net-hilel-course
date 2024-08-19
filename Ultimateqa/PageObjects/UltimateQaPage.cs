using Microsoft.Playwright;

namespace UltimateQa.PageObjects;

public class UltimateQaPage (IPage page)
{
    private ILocator ViewMoreCourses => page.GetByRole(AriaRole.Link, new() { Name = "View more courses" });
    public ILocator SignOut => page.GetByRole(AriaRole.Link, new() { Name = "Sign Out" });
    public ILocator MyAccount => page.GetByRole(AriaRole.Link, new() { Name = "My Account" });
    public ILocator Support => page.GetByRole(AriaRole.Link, new() { Name = "Support" });
    private ILocator SearchField => page.GetByPlaceholder("Search");
    private ILocator MyDashboard => page.GetByRole(AriaRole.Link, new() { Name = "My Dashboard" });
    private ILocator UserMenu => page.GetByLabel("Toggle menu");
    private ILocator UserMenuItems => page.Locator("ul.dropdown__menu");
    private ILocator WelcomeBackMessage => page.Locator(".student-dashboard__welcome");
    private ILocator CourseTitle => page.Locator($"h3.card__name");
    private ILocator SearchResultFor => page.Locator(".products__title strong");
    
    public async Task ClickViewMoreCoursesLink()
    {
        await ViewMoreCourses.ClickAsync();
        await page.WaitForLoadStateAsync();
    }

    public async Task OpenMyDashboard()
    {
        await MyDashboard.ClickAsync();
        await page.WaitForLoadStateAsync();
    }
    
    public async Task<bool> IsWelcomeMessageDisplayedCorrectly(string userName)
    {
        if (userName == null) throw new ArgumentNullException(nameof(userName));
        var welcomeBackMessage = await WelcomeBackMessage.InnerTextAsync();
        return welcomeBackMessage.Contains($"Welcome back, {userName}!");
    }
    
    public async Task OpenUserMenu()
    {
        await UserMenu.ClickAsync();
        await UserMenuItems.IsVisibleAsync();
    }
    
    public async Task SearchCourse(string courseName)
    {
        if(courseName == null) throw new ArgumentNullException(nameof(courseName));
        await SearchField.FillAsync(courseName);
        await SearchField.PressAsync("Enter");
        await page.WaitForLoadStateAsync();
        Assert.That(await SearchResultFor.InnerTextAsync(), Is.EqualTo(courseName), $"'Search Result for' doesn't contain: {courseName}");
    }
    
    public async Task<bool> AreAllSearchResultsMatchingCourseName(string courseName)
    {
        if (courseName == null) throw new ArgumentNullException(nameof(courseName));
        var searchResults = await CourseTitle.AllAsync();
        foreach (var searchResult in searchResults)
        {
            if (!(await searchResult.InnerTextAsync()).Contains(courseName))
            {
                return false;
            }
        }
        return true;
    }
}