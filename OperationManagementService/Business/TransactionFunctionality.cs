using Azure;
using EntitiesCustom;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationExceptions;
using System.ComponentModel.DataAnnotations;

namespace OperationManagementService.Business
{
    public class TransactionFunctionality : FunctionalityBaseController
    {
        public async Task<CustomResponse> BasicValidateTransaction(Guid userId, Guid sessionId, string transactionName, decimal? transactionAmount, bool isUpdate)
        {
            try
            {
                Transaction newTransaction = new Transaction(operationId: 0, userId: userId, transactionName: transactionName, amount: transactionAmount, transactionDate: DateTime.Now, isDeleted: false, isUpdate: isUpdate);
                this.ValidateModel(newTransaction);
                await this.ValidateSession(userId, sessionId);
                // return the result
                return new CustomResponse(statusCode: StatusCodes.Status200OK, message: "Validated transaction successfully.", userId: userId, sessionId: sessionId); ;
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
        public async Task<CustomResponse> AddTransaction(Guid userId, Guid sessionId, string transactionName, decimal transactionAmount, int operationId)
        {
            try
            {
                Transaction newTransaction = new Transaction(operationId: operationId, userId: userId, transactionName: transactionName, amount: transactionAmount, transactionDate: DateTime.Now, isDeleted: false, isUpdate: false);
                using (var context = new Models.OperationContext())
                {
                    var transaction = Mapster.TypeAdapter.Adapt<Models.Transaction>(newTransaction);
                    context.Transactions.Add(transaction);
                    await context.SaveChangesAsync();
                }

                // return the result
                return new CustomResponse(statusCode: StatusCodes.Status200OK, message: "Data saved successfully.", userId: userId, sessionId: sessionId); ;
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

        public async Task<CustomResponse> UpdateTransaction(Guid userId, Guid sessionId, int trasactionId, string transactionName, decimal? transactionAmount, int operationId)
        {
            try
            {
                using (var context = new Models.OperationContext())
                {
                    // check if the transaction exists
                    var existingTransaction = await context.Transactions.FirstOrDefaultAsync(t => t.UserId == userId && t.TransactionId == trasactionId && t.IsDeleted == false);
                    // if not, throw an exception
                    if (existingTransaction == null)
                    {
                        var exception = this._errorService.GetError("OMS-TRANSACTION-NOT-FOUND");
                        throw new OperationException(errorCode: exception.Code, message: exception.Message, details: exception.Details, new Guid());
                    }
                    // validate if transactionName is different from existing value
                    if (existingTransaction.TransactionName != transactionName)
                        existingTransaction.TransactionName = transactionName;
                    // validate if transactionAmount has value and is different from existing value
                    if (existingTransaction.Amount != transactionAmount && transactionAmount.HasValue)
                        existingTransaction.Amount = transactionAmount.GetValueOrDefault();
                    // update the transaction
                    context.Transactions.Update(existingTransaction);
                    await context.SaveChangesAsync();
                }

                // return the result
                return new CustomResponse(statusCode: StatusCodes.Status200OK, message: "Data updated successfully.", userId: userId, sessionId: sessionId); ;
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

        public async Task<CustomResponse> DeleteTransaction(Guid userId, Guid sessionId, int trasactionId)
        {
            try
            {
                // validate the session
                await this.ValidateSession(userId, sessionId);
                using (var context = new Models.OperationContext())
                {
                    // check if the transaction exists
                    var existingTransaction = await context.Transactions.FirstOrDefaultAsync(t => t.UserId == userId && t.TransactionId == trasactionId && t.IsDeleted == false);
                    // if not, throw an exception
                    if (existingTransaction == null)
                    {
                        var exception = this._errorService.GetError("OMS-TRANSACTION-NOT-FOUND");
                        throw new OperationException(errorCode: exception.Code, message: exception.Message, details: exception.Details, new Guid());
                    }
                    // mark the transaction as deleted
                    existingTransaction.IsDeleted = true;
                    // update the transaction
                    context.Transactions.Update(existingTransaction);
                    await context.SaveChangesAsync();
                }

                // return the result
                return new CustomResponse(statusCode: StatusCodes.Status200OK, message: "Deleted successfully.", userId: userId, sessionId: sessionId); ;
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

        public async Task<CustomResponse> GetTransactions(Guid userId, Guid sessionId, int? trasactionId)
        {
            try
            {
                // validate the session
                await this.ValidateSession(userId, sessionId);
                using (var context = new Models.OperationContext())
                {
                    // init the list of transactions
                    List<Models.Transaction> existingTransactions = new List<Models.Transaction>();
                    // if trasactionId has value, get the specific transaction
                    if (trasactionId.HasValue)
                    {
                        // check if the transaction exists
                        existingTransactions.Add(await context.Transactions.FirstOrDefaultAsync(t => t.UserId == userId && t.TransactionId == trasactionId && t.IsDeleted == false));
                        // if not, throw an exception
                        if (existingTransactions.Count == 0)
                        {
                            var exception = this._errorService.GetError("OMS-TRANSACTION-NOT-FOUND");
                            throw new OperationException(errorCode: exception.Code, message: exception.Message, details: exception.Details, new Guid());
                        }
                    } 
                    else
                    {
                        // get all transactions for the user
                        existingTransactions = await context.Transactions.Where(t => t.UserId == userId && t.IsDeleted == false).ToListAsync();
                    }

                    var transactionsResult = existingTransactions.Adapt<List<Transaction>>();
                    // return the result
                    return new CustomResponse(statusCode: StatusCodes.Status200OK, message: "Get data successfully.", userId: userId, sessionId: sessionId, data: transactionsResult);
                }

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
