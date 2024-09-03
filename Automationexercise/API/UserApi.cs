using System.Net;
using Microsoft.Playwright;

namespace AutomationExercise.API;
public class UserApi(IPage page, string baseAddress)
{
    private readonly HttpClient _client = new() { BaseAddress = new Uri(baseAddress) };
    private readonly IPage _page = page;
    
    public async Task<bool> CreateUserAsync(UiTestFixture.User user)
    {
        var formData = new MultipartFormDataContent
        {
                { new StringContent(user.Name), "name" },
                { new StringContent(user.Email), "email" },
                { new StringContent(user.Password), "password" },
                { new StringContent(user.Title), "title" },
                { new StringContent(user.BirthDate), "birth_date" },
                { new StringContent(user.BirthMonth), "birth_month" },
                { new StringContent(user.BirthYear), "birth_year" },
                { new StringContent(user.FirstName), "firstname" },
                { new StringContent(user.LastName), "lastname" },
                { new StringContent(user.Company), "company" },
                { new StringContent(user.Address1), "address1" },
                { new StringContent(user.Address2), "address2" },
                { new StringContent(user.Country), "country" },
                { new StringContent(user.Zipcode), "zipcode" },
                { new StringContent(user.State), "state" },
                { new StringContent(user.City), "city" },
                { new StringContent(user.MobileNumber), "mobile_number" }
        }; 
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/createAccount")
        {
            Content = formData
        };
        var response = await _client.SendAsync(requestMessage);
        Assert.That(await response.Content.ReadAsStringAsync(), Is.EqualTo("{\"responseCode\": 201, \"message\": \"User created!\"}"), $"Error: {response.StatusCode}");
        return response.StatusCode == HttpStatusCode.OK;
    }
    
    public async Task<string> GetUserDetailByEmailAsync(UiTestFixture.User user)
    {
        if (string.IsNullOrEmpty(user.Email)) throw new ArgumentException("Email cannot be null or empty", nameof(user.Email));
        var response = await _client.GetAsync($"api/getUserDetailByEmail?email={user.Email}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        if (content == null) throw new InvalidOperationException("Failed to deserialize the response content into UserDetail");
        return content;
    }
    
    public async Task<string> DeleteUserAsync(UiTestFixture.User user)
    {
        var formData = new MultipartFormDataContent
        {
                { new StringContent(user.Email), "email" },
                { new StringContent(user.Password), "password" }
        };
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, "api/deleteAccount")
        {
                Content = formData
        };
        var response = await _client.SendAsync(requestMessage);
        var result = await response.Content.ReadAsStringAsync();
        return result;
        //Assert.That(await response.Content.ReadAsStringAsync(), Is.EqualTo("{\"responseCode\": 200, \"message\": \"Account deleted!\"}"), $"Error: {response.StatusCode}");
        //return response.StatusCode == HttpStatusCode.OK;
    }
}