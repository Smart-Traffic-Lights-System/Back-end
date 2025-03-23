using System;

namespace UserData.Entities;

public class UserActionLog
{
    public int ActionLogId { get; set; }
    public int UserId { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public DateTime ActionTimestamp { get; set; } = DateTime.UtcNow;
    public string ActionDetails { get; set; } = string.Empty;
}
