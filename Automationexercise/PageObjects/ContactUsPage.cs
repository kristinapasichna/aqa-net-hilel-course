using AutomationExercise;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class ContactUsPage(IPage page) : BasePage(page)
{
    private readonly IPage _page = page;
    
    //Selectors & Locators
    private ILocator NameField => _page.GetByRole(AriaRole.Textbox, new() { Name = "Name" });
    private ILocator EmailField => _page.Locator("[placeholder='Email']");
    private ILocator SubjectField => _page.GetByRole(AriaRole.Textbox, new() { Name = "Subject" });
    private ILocator ChoseFileButton => _page.Locator("[name='upload_file']");
    private ILocator SubmitButton => _page.GetByRole(AriaRole.Button, new() { Name = "Submit" });
    public ILocator SuccessMessage => _page.Locator("//div[@class='contact-form']/div[contains(@class,'alert-success')]");
    
    //Methods
    public async Task FillInName(string name)
    {
        await NameField.PressSequentiallyAsync(name);
        await Assertions.Expect(_page.GetByPlaceholder("Name")).ToHaveValueAsync(name);
    }
    
    public async Task FillInEmail(string email)
    {
        await EmailField.PressSequentiallyAsync(email);
        await Assertions.Expect(EmailField).ToHaveValueAsync(email);
    }
    
    public async Task FillInSubject(string subject)
    {
        await SubjectField.PressSequentiallyAsync(subject);
        await Assertions.Expect(_page.GetByPlaceholder("Subject")).ToHaveValueAsync(subject);
    }
    
    public async Task UploadFileAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
        await ChoseFileButton.SetInputFilesAsync(filePath);
    }
    
    public async Task SubmitForm()
    {
        _page.Dialog += async (_, dialog) =>
        {
            await dialog.AcceptAsync();
        };
        await SubmitButton.ClickAsync();
    }
}