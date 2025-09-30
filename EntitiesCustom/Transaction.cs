using OperationExceptions;

namespace EntitiesCustom;

public partial class Transaction : IValidation
{
    public Transaction()
    {
    }
    public Transaction(int transactionId, int? operationId, Guid? userId, string transactionName, decimal amount, DateTime transactionDate, bool? isDeleted)
    {
        this.TransactionId = transactionId;
        this.OperationId = operationId;
        this.UserId = userId;
        this.TransactionName = transactionName;
        this.Amount = amount;
        this.TransactionDate = transactionDate;
        this.IsDeleted = isDeleted;
    }
    public int TransactionId { get; set; }

    public int? OperationId { get; set; }

    public Guid? UserId { get; set; }

    public string TransactionName { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual OperationLog? Operation { get; set; }

    public virtual User? User { get; set; }

    public string Validate(string operationExceptionCode)
    {
        throw new NotImplementedException();
    }
}
