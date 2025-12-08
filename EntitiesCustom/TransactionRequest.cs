using Microsoft.AspNetCore.Mvc;

namespace EntitiesCustom;

public class TransactionRequest : ActionResult
{
    public TransactionRequest()
    {
    }
    public TransactionRequest(string transactionName, decimal transactionAmount)
    {
        TransactionName = transactionName;
        TransactionAmount = transactionAmount;
    }
    public string TransactionName { get; }
    public decimal TransactionAmount { get; }

}
