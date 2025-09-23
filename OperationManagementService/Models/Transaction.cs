using System;
using System.Collections.Generic;

namespace OperationManagementService.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? OperationId { get; set; }

    public Guid? UserId { get; set; }

    public string TransactionName { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual OperationLog? Operation { get; set; }

    public virtual User? User { get; set; }
}
