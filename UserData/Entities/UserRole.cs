using System;

namespace UserData.Entities;

public class UserRole
{
    public long RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string RoleDescription { get; set; } = string.Empty;
}
