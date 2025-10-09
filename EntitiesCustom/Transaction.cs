using OperationExceptions;

namespace EntitiesCustom;

public partial class Transaction : IValidation
{
    public Transaction()
    {
    }

    public Transaction(int? operationId, Guid? userId, string transactionName, decimal? amount, DateTime transactionDate, bool? isDeleted, bool isUpdate)
    {
        this.OperationId = operationId;
        this.UserId = userId;
        this.TransactionName = transactionName;
        this.Amount = amount;
        this.TransactionDate = transactionDate;
        this.IsDeleted = isDeleted;
        this.IsUpdate = isUpdate;
    }
    public int TransactionId { get; set; }

    public int? OperationId { get; set; }

    public Guid? UserId { get; set; }

    public string TransactionName { get; set; } = null!;

    public decimal? Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    public bool? IsDeleted { get; set; }
    public bool? IsUpdate { get; set; }

    public virtual OperationLog? Operation { get; set; }

    public virtual User? User { get; set; }

    public string Validate(string operationExceptionCode)
    {
        // When IsUpdate is true, TransactionName and Amount can be null (not updated)
        if (String.IsNullOrEmpty(this.TransactionName) && !this.IsUpdate.GetValueOrDefault())
            operationExceptionCode = "OMS-TRANSACTIONNAME-ERROR";
        // TransactionName max length is 50
        if (!String.IsNullOrEmpty(this.TransactionName) && this.TransactionName.Length > 50)
            operationExceptionCode = "OMS-TRANSACTIONNAME-ERROR";
        // When IsUpdate is true, Amount can be null (not updated)
        if (!this.Amount.HasValue && !this.IsUpdate.GetValueOrDefault())
            operationExceptionCode = "OMS-TRANSACTIONAMOUNT-ERROR";

        return operationExceptionCode;
    }
}
