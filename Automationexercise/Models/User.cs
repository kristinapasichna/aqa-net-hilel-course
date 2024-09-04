using AutomationExercise.TestData;

namespace AutomationExercise.Models;

public class User
{
    public string Name { get; set; } = Helpers.DefaultName;
    public string Email { get; set; } = Helpers.DefaultEmail;
    public string Password { get; set; } = Helpers.DefaultPassword;
    public string Title { get; set; } = Helpers.DefaultTitle;
    public string BirthDate { get; set; } = Helpers.DefaultBirthDate;
    public string BirthMonth { get; set; } = Helpers.DefaultBirthMonth; 
    public string BirthYear { get; set; }  = Helpers.DefaultBirthYear;
    public string FirstName { get; set; } = Helpers.DefaultFirstName; 
    public string LastName { get; set; } = Helpers.DefaultLastName; 
    public string Company { get; set; } = Helpers.DefaultCompany; 
    public string Address1 { get; set; } = Helpers.DefaultAddress1; 
    public string Address2 { get; set; } = Helpers.DefaultAddress2; 
    public string Country { get; set; } = Helpers.DefaultCountry; 
    public string Zipcode { get; set; } = Helpers.DefaultZipcode; 
    public string State { get; set; } = Helpers.DefaultState; 
    public string City { get; set; } = Helpers.DefaultCity; 
    public string MobileNumber { get; set; } = Helpers.DefaultMobileNumber; 
}