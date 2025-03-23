using System;

namespace UserData.Entities;

public class User
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; } = false;
    public DateTime EmailVerifiedAt { get; set; } = DateTime.MinValue;
    public DateTime DateOfBirth { get; set; } = DateTime.MinValue;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsPhoneVerified { get; set; } = false;
    public DateTime PhoneVerifiedAt { get; set; } = DateTime.MinValue;
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public int RoleId { get; set; } 
    public UserRole Role { get; set; }
    public ICollection<UserActionLog> UserActionLogs { get; set; } = new List<UserActionLog>();
}

    

