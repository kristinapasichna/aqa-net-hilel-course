using System.Net;
using AutomationExercise.Models;

namespace AutomationExercise.API;
public class UserApi(string baseAddress)
{
    private readonly HttpClient _client = new() { BaseAddress = new Uri(baseAddress) };
    
    public async Task<bool> CreateUserAsync(User user)
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
    
    public async Task<string> GetUserDetailByEmailAsync(string email)
    {
        if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email cannot be null or empty", nameof(email));
        var response = await _client.GetAsync($"api/getUserDetailByEmail?email={email}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    
    public async Task<string> DeleteUserAsync(User user)
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
        return await response.Content.ReadAsStringAsync();
    }
}