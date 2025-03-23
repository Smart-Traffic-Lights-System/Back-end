using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models;

public class RegisterUserDto
{
    public long UserId { get; set; }
    [Required(ErrorMessage = "First Name is required")]
    public string FirstName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Last Name is required")]
    public string LastName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Date of Birth is required")]
    public DateTime DateOfBirth { get; set; } = DateTime.MinValue;
    [Required(ErrorMessage = "Phone Number is required")]
    public string PhoneNumber { get; set; } = string.Empty;
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; } = string.Empty;
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
