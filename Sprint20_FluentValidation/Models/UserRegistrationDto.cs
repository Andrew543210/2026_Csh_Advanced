namespace Sprint20_FluentValidation.Models;

public class UserRegistrationDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public PersonalInfoDto? PersonalInfo { get; set; }
    public AddressInfoDto? Address { get; set; }
}