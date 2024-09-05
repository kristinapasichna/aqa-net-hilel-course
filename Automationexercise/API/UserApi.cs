using System.Net;
using AutomationExercise.TestData;

namespace AutomationExercise.API;
public class UserApi(string baseAddress)
{
    private readonly HttpClient _client = new() { BaseAddress = new Uri(baseAddress) };
    
    public async Task<bool> CreateUserAsync(Constants user)
    {
        var formData = new MultipartFormDataContent
        {
                { new StringContent(Constants.Name), "name" },
                { new StringContent(Constants.Email), "email" },
                { new StringContent(Constants.Password), "password" },
                { new StringContent(Constants.Title), "title" },
                { new StringContent(Constants.BirthDate), "birth_date" },
                { new StringContent(Constants.BirthMonth), "birth_month" },
                { new StringContent(Constants.BirthYear), "birth_year" },
                { new StringContent(Constants.FirstName), "firstname" },
                { new StringContent(Constants.LastName), "lastname" },
                { new StringContent(Constants.Company), "company" },
                { new StringContent(Constants.Address1), "address1" },
                { new StringContent(Constants.Address2), "address2" },
                { new StringContent(Constants.Country), "country" },
                { new StringContent(Constants.Zipcode), "zipcode" },
                { new StringContent(Constants.State), "state" },
                { new StringContent(Constants.City), "city" },
                { new StringContent(Constants.MobileNumber), "mobile_number" }
        }; 
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/createAccount")
        {
            Content = formData
        };
        var response = await _client.SendAsync(requestMessage);
        Assert.That(await response.Content.ReadAsStringAsync(), Is.EqualTo("{\"responseCode\": 201, \"message\": \"User created!\"}"), $"Error: {response.StatusCode}");
        return response.StatusCode == HttpStatusCode.OK;
    }
    
    public async Task<string> GetUserDetailByEmailAsync(string email)
    {
        if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email cannot be null or empty", nameof(email));
        var response = await _client.GetAsync($"api/getUserDetailByEmail?email={email}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    
    public async Task<string> DeleteUserAsync(Constants user)
    {
        var formData = new MultipartFormDataContent
        {
                { new StringContent(Constants.Email), "email" },
                { new StringContent(Constants.Password), "password" }
        };
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, "api/deleteAccount")
        {
                Content = formData
        };
        var response = await _client.SendAsync(requestMessage);
        return await response.Content.ReadAsStringAsync();
    }
}