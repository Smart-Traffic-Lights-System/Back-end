using System;

namespace UserData.Entities;

public class UserActionLog
{
    public long ActionLogId { get; set; }
    public long UserId { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public DateTime ActionTimestamp { get; set; } = DateTime.UtcNow;
    public string ActionDetails { get; set; } = string.Empty;
}
