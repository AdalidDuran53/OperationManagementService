using Azure;
using EntitiesCustom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationExceptions;
using System.ComponentModel.DataAnnotations;

namespace OperationManagementService.Business
{
    public class TransactionFunctionality : FunctionalityBaseController
    {
        public async Task<ActionResult> BasicValidateTransaction(Guid userId, Guid sessionId, string transactionName, decimal transactionAmount)
        {
            try
            {
                Transaction newTransaction = new Transaction(operationId: 0, userId: userId, transactionName: transactionName, amount: transactionAmount, transactionDate: DateTime.Now, isDeleted: false);
                this.ValidateModel(newTransaction);
                this.ValidateSession(userId, sessionId);
                // return the result
                var result = Ok(new { success = true, message = "Validated transaction successfully." });
                return result;
            }
            catch (Exception ex)
            {
                // if the exception is an OperationException, rethrow it
                if (ex is OperationException)
                    throw ex;
                // otherwise, throw a general error
                var exception = this._errorService.GetError("OMS-GENERAL-ERROR");
                throw new OperationException(errorCode: exception.Code, message: exception.Message, details: exception.Details, new Guid());
            }
        }
        public async Task<ActionResult> AddTransaction(Guid userId, Guid sessionId, string transactionName, decimal transactionAmount, int operationId)
        {
            try
            {
                Transaction newTransaction = new Transaction(operationId: operationId, userId: userId, transactionName: transactionName, amount: transactionAmount, transactionDate: DateTime.Now, isDeleted: false);
                using (var context = new Models.OperationContext())
                {
                    var transaction = Mapster.TypeAdapter.Adapt<Models.Transaction>(newTransaction);
                    context.Transactions.Add(transaction);
                    await context.SaveChangesAsync();
                }

                // return the result
                var result = Ok(new { success = true, message = "Data saved successfully." });
                return result;
            }
            catch (Exception ex)
            {
                // if the exception is an OperationException, rethrow it
                if (ex is OperationException)
                    throw ex;
                // otherwise, throw a general error
                var exception = this._errorService.GetError("OMS-GENERAL-ERROR");
                throw new OperationException(errorCode: exception.Code, message: exception.Message, details: exception.Details, new Guid());
            }
        }

    }
}
