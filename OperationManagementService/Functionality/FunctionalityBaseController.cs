using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationManagementService.Models;
using OperationManagementService.OperationExceptions;
using System.Security.Cryptography;

namespace OperationManagementService.Functionality
{
    public class FunctionalityBaseController : Controller
    {
        public readonly ErrorServiceModel _errorService = new ErrorServiceModel();
        public FunctionalityBaseController()
        {
            _errorService = new ErrorServiceModel();
        }

        #region ValidationModel
        protected virtual void ValidateModel(IValidation model)
        {
            // init operation exception code
            string operationExceptionCode = null;
            // validate the model
            operationExceptionCode = model.Validate(operationExceptionCode);
            // if there is an operation exception code, throw an operation exception
            if (!String.IsNullOrEmpty(operationExceptionCode))
            {
                // get the error item from the error service
                ErroritemServiceModel erroritemService = _errorService.GetError(operationExceptionCode);
                // throw the operation exception
                throw new OperationException(errorCode: erroritemService.Code, message: erroritemService.Message, details: erroritemService.Details);
            }
        }

        #endregion

        #region Password Hashing and Verification
        // hash the password
        public static (string Hash, string Salt) HashPassword(string password)
        {   // generate a salt
            var saltBytes = RandomNumberGenerator.GetBytes(16);
            var salt = Convert.ToBase64String(saltBytes);
            // hash the password with the salt
            using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256);
            var hash = Convert.ToBase64String(pbkdf2.GetBytes(32));

            return (hash, salt);
        }


        // verify the password
        public bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            // hash the password with the stored salt
            var saltBytes = Convert.FromBase64String(storedSalt);
            using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256);
            var hash = Convert.ToBase64String(pbkdf2.GetBytes(32));
            // compare the hash with the stored hash
            return hash == storedHash;
        }
        #endregion

        #region Session Management
        public async Task<ActionResult> InitSession(Guid userId)
        {
            try
            {
                SessionLog sessionLog = new SessionLog(userId: userId, sessionId: Guid.NewGuid(), initSession: DateTime.Now);

                // save the operation log object
                using (var context = new OperationContext())
                {
                    context.SessionLogs.Add(sessionLog);
                    await context.SaveChangesAsync();
                    return Ok(new { success = true, message = "Session initialized successfully.", sessionId = sessionLog.SessionId });
                }
            }
            catch (Exception ex)
            {
                // if the exception is an OperationException, rethrow it
                if (ex is OperationException)
                    throw ex;
                // otherwise, throw a general error
                var exception = this._errorService.GetError("OMS-GENERAL-ERROR");
                throw new OperationException(errorCode: exception.Code, message: exception.Message, details: exception.Details);
            }
        }
        #endregion
    }
}
