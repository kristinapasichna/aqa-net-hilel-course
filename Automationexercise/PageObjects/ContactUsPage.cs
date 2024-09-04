using AutomationExercise;
using Microsoft.Playwright;

namespace Automationexercise.PageObjects;

public class ContactUsPage(IPage page, string baseUrl) : BasePage(page: page, baseUrl)
{
    //Selectors & Locators
    private ILocator NameField => Page.GetByRole(AriaRole.Textbox, new() { Name = "Name" });
    private ILocator EmailField => Page.Locator("[placeholder='Email']");
    private ILocator SubjectField => Page.GetByRole(AriaRole.Textbox, new() { Name = "Subject" });
    private ILocator ChoseFileButton => Page.Locator("[name='upload_file']");
    private ILocator SubmitButton => Page.GetByRole(AriaRole.Button, new() { Name = "Submit" });
    public ILocator SuccessMessage => Page.Locator("//div[@class='contact-form']/div[contains(@class,'alert-success')]");
    
    //Methods
    public async Task FillInName(string name)
    {
        await NameField.PressSequentiallyAsync(name);
        await Assertions.Expect(Page.GetByPlaceholder("Name")).ToHaveValueAsync(name);
    }
    
    public async Task FillInEmail(string email)
    {
        await EmailField.PressSequentiallyAsync(email);
        await Assertions.Expect(EmailField).ToHaveValueAsync(email);
    }
    
    public async Task FillInSubject(string subject)
    {
        await SubjectField.PressSequentiallyAsync(subject);
        await Assertions.Expect(Page.GetByPlaceholder("Subject")).ToHaveValueAsync(subject);
    }
    
    public async Task UploadFileAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
        await ChoseFileButton.SetInputFilesAsync(filePath);
    }
    
    public async Task ClickSubmitButton()
    {
        await SubmitButton.ClickAsync();
    }
}