using Azure;
using EntitiesCustom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OperationExceptions;
using OperationManagementService.Business;
using OperationManagementService.Models;
using System.ComponentModel.DataAnnotations;

namespace OperationManagementService.Controllers.Implementation
{
    [ApiVersion("0.1")]
    [ApiController]
    [Authorize]
    public class TransactionApi : TransactionControllerBase
    {

        private readonly ServiceBaseFunctionality _serviceBaseFunctionality;
        private readonly TransactionFunctionality _transactionFunctionality;
        private readonly ErrorServiceModel _errorService = new ErrorServiceModel();
        public TransactionApi(TransactionFunctionality transactionFunctionality, ServiceBaseFunctionality serviceBaseFunctionality)
        {
            _transactionFunctionality = transactionFunctionality;
            _serviceBaseFunctionality = serviceBaseFunctionality;
            _errorService = new ErrorServiceModel();
        }

        [HttpPost]
        [Route("~/{version::apiVersion}/Transactions/AddTransaction")]
        public override async Task<IActionResult> AddTransaction([FromRoute, RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$"), Required] string version, [Required] Guid userId, [Required] Guid sessionId, [Required] string transactionName, [Required] decimal transactionAmount)
        {
            // Log the request
            Dictionary<string, object> request = new Dictionary<string, object> { { "AddTransactionRequest", new object[] { "version: " + version, "userId: " + userId, "sessionId: " + sessionId, "transactionName: " + transactionName , "transactionAmount: " + transactionAmount } } };
            try
            {
                await _transactionFunctionality.BasicValidateTransaction(userId: userId, sessionId: sessionId, transactionName: transactionName, transactionAmount: transactionAmount, isUpdate: false);
                // Call the implementation
                var operationId = await _serviceBaseFunctionality.LogOperation(request, new Dictionary<string, object>(), sessionId);
                var result = await _transactionFunctionality.AddTransaction(userId, sessionId, transactionName, transactionAmount, (int)operationId.Data);
                await _serviceBaseFunctionality.UpdateOperation((int)operationId.Data, request, new Dictionary<string, object> { { "AddTransactionResponse", result } }, sessionId);
                // return the result
                return Ok(new CustomResponse(statusCode: StatusCodes.Status200OK, message: result.Message, userId: result.UserId, sessionId: sessionId));
            }
            catch (Exception ex)
            {
                // Log the exception
                Dictionary<string, object> response = new Dictionary<string, object> { { "ErrorAddTransactionResponse", ex } };
                await _serviceBaseFunctionality.LogOperation(request, response);
                // if the exception is an OperationException, return a bad request with the error details
                OperationException excep = ((OperationException)ex);
                return this.BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, code = excep.ErrorCode, message = excep.Message, details = excep.Details});
            }
        }

        [HttpPut]
        [Route("~/{version::apiVersion}/Transactions/UpdateTransaction")]
        public override async Task<IActionResult> UpdateTransaction([FromRoute, RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$"), Required] string version, [Required] Guid userId, [Required] Guid sessionId, [Required] int transactionId, string transactionName, decimal transactionAmount)
        {
            // Log the request
            Dictionary<string, object> request = new Dictionary<string, object> { { "UpdateTransactionRequest", new object[] { "version: " + version, "userId: " + userId, "sessionId: " + sessionId, "transactionName: " + transactionName, "transactionAmount: " + transactionAmount } } };
            try
            {
                await _transactionFunctionality.BasicValidateTransaction(userId: userId, sessionId: sessionId, transactionName: transactionName, transactionAmount: transactionAmount, isUpdate: true);
                // Call the implementation
                var operationId = await _serviceBaseFunctionality.LogOperation(request, new Dictionary<string, object>(), sessionId);
                var result = await _transactionFunctionality.UpdateTransaction(userId, sessionId, transactionId, transactionName, transactionAmount, (int)operationId.Data);
                await _serviceBaseFunctionality.UpdateOperation((int)operationId.Data, request, new Dictionary<string, object> { { "UpdateTransactionResponse", result } }, sessionId);
                // return the result
                return Ok(new CustomResponse(statusCode: StatusCodes.Status200OK, message: result.Message, userId: result.UserId, sessionId: sessionId));
            }
            catch (Exception ex)
            {
                // Log the exception
                Dictionary<string, object> response = new Dictionary<string, object> { { "ErrorUpdateTransactionResponse", ex } };
                await _serviceBaseFunctionality.LogOperation(request, response);
                // if the exception is an OperationException, return a bad request with the error details
                OperationException excep = ((OperationException)ex);
                return this.BadRequest(new { StatusCode =StatusCodes.Status400BadRequest, code = excep.ErrorCode, message = excep.Message, details = excep.Details });
            }
        }


        [HttpDelete]
        [Route("~/{version::apiVersion}/Transactions/DeleteTransaction")]
        public override async Task<IActionResult> DeleteTransaction([FromRoute, RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$"), Required] string version, [Required] Guid userId, [Required] Guid sessionId)
        {
            // Log the request
            Dictionary<string, object> request = new Dictionary<string, object> { { "DeleteTransactionRequest", new object[] { "version: " + version, "userId: " + userId, "sessionId: " + sessionId } } };
            try
            {
                // return the result
                return Ok("ok");
            }
            catch (Exception ex)
            {
                // Log the exception
                Dictionary<string, object> response = new Dictionary<string, object> { { "ErrorDeleteTransactionResponse", ex } };
                await _serviceBaseFunctionality.LogOperation(request, response, sessionId);
                // if the exception is an OperationException, return a bad request with the error details
                OperationException excep = ((OperationException)ex);
                return this.BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, code = excep.ErrorCode, message = excep.Message, details = excep.Details });
            }
        }

        [HttpGet]
        [Route("~/{version::apiVersion}/Transactions/GetTransactions")]
        public override async Task<IActionResult> GetTransactions([FromRoute][Required][RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$")] string version, [Required] Guid userId, [Required] Guid sessionId)
        {
            // Log the request
            Dictionary<string, object> request = new Dictionary<string, object> { { "GetTransactionsRequest", new object[] { "version: " + version, "userId: " + userId, "sessionId: " + sessionId } } };
            try
            {
                // return the result
                return Ok("ok");
            }
            catch (Exception ex)
            {
                // Log the exception
                Dictionary<string, object> response = new Dictionary<string, object> { { "ErrorGetTransactionsResponse", ex } };
                await _serviceBaseFunctionality.LogOperation(request, response, sessionId);
                // if the exception is an OperationException, return a bad request with the error details
                OperationException excep = ((OperationException)ex);
                return this.BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, code = excep.ErrorCode, message = excep.Message, details = excep.Details });
            }
        }
    }
}
