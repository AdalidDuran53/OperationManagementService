using System;
using System.Collections.Generic;

namespace OperationManagementService.Models;

public partial class SessionLog
{
    public SessionLog()
    {
    }

    public SessionLog(Guid sessionId, Guid? userId, DateTime initSession)
    {
        this.SessionId = sessionId;
        this.UserId = userId;
        this.InitSession = initSession;
    }
    public Guid SessionId { get; set; }

    public Guid? UserId { get; set; }

    public DateTime InitSession { get; set; }

    public DateTime? EndSession { get; set; }

    public virtual ICollection<OperationLog> OperationLogs { get; set; } = new List<OperationLog>();

    public virtual User? User { get; set; }
}
