using System;

namespace UserData.Entities;

public class User
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; } = false;
    public DateTime EmailVerifiedAt { get; set; } 
    public DateTime DateOfBirth { get; set; } 
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsPhoneVerified { get; set; } = false;
    public DateTime PhoneVerifiedAt { get; set; } 
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; } 
    public string Role { get; set; } = string.Empty;
    public ICollection<UserActionLog> UserActionLogs { get; set; } = new List<UserActionLog>();
}

    

