using System;
using System.Collections.Generic;

namespace OperationManagementService.Models;

public partial class OperationLog
{
    public OperationLog()
    {
    }
    public OperationLog(Guid sessionId, DateTime operationDate, string request, string response)
    {
        this.SessionId = sessionId;
        this.OperationDate = operationDate;
        this.Request = request;
        this.Response = response;
    }
    public int OperationId { get; set; }

    public Guid? SessionId { get; set; }

    public DateTime? OperationDate { get; set; }

    public string? Request { get; set; }

    public string? Response { get; set; }

    public virtual SessionLog? Session { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
