using System;
using System.Collections.Generic;

namespace OperationManagementService.Models;

public partial class User
{
    public User()
    {
    }
    public User(Guid userId, string userName, string password)
    {
        this.UserId = userId;
        this.UserName = userName;
        this.PasswordHash = password;
    }

    public Guid UserId { get; set; }

    public string? UserName { get; set; }

    public string PasswordHash { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public virtual ICollection<SessionLog> SessionLogs { get; set; } = new List<SessionLog>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
