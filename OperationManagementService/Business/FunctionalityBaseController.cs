using EntitiesCustom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationExceptions;
using System.Security.Cryptography;

namespace OperationManagementService.Business
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
        public async Task<CustomResponse> InitSession(Guid userId)
        {
            try
            {
                // set custom session log object
                SessionLog sessionLog = new SessionLog(userId: userId, sessionId: Guid.NewGuid(), initSession: DateTime.Now);

                // save the operation log object
                using (var context = new Models.OperationContext())
                {
                    // init data list
                    List<object> data = new List<object>();
                    // check for existing sessions for the user and close them
                    var existingSession = await context.SessionLogs.Where(s => s.UserId == userId && s.EndSession == null).ToListAsync();
                    // close existing sessions
                    foreach (var sessionitem in existingSession)
                    {
                        var result = CloseSession(sessionitem.SessionId);
                        // add the result to the data list
                        data.Add(result.Result);
                    }
                    // map the operation log object to the entity model
                    var session = Mapster.TypeAdapter.Adapt<Models.SessionLog>(sessionLog);
                    context.SessionLogs.Add(session);
                    await context.SaveChangesAsync();
                    return new CustomResponse(statusCode: StatusCodes.Status200OK, message: "Session initialized successfully.", userId: userId, sessionId: sessionLog.SessionId, data: data);
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

        public async Task<CustomResponse> CloseSession(Guid sessionId)
        {
            try
            {
                using (var context = new Models.OperationContext())
                {
                    // find the session log by session id
                    var sessionLog = await context.SessionLogs.FirstOrDefaultAsync(s => s.SessionId == sessionId && s.EndSession == null);
                    if (sessionLog == null || sessionId == null)
                    {
                        // if the session log is not found, throw an error
                        var exception = this._errorService.GetError("OMS-SESSION-ERROR");
                        throw new OperationException(errorCode: exception.Code, message: exception.Message, details: exception.Details);
                    }
                    // close the session
                    sessionLog.EndSession = DateTime.Now;
                    context.SessionLogs.Update(sessionLog);
                    await context.SaveChangesAsync();
                    // prepare the data to return
                    Dictionary<string, object> data = new Dictionary<string, object>
                    {
                        { "UserId", sessionLog.UserId },
                        { "SessionId", sessionLog.SessionId },
                        { "InitSession", sessionLog.InitSession },
                        { "EndSession", sessionLog.EndSession }
                    };
                    return new CustomResponse(statusCode: StatusCodes.Status200OK, message: "Session closed successfully.", userId: sessionLog.UserId.GetValueOrDefault(), sessionId: sessionLog.SessionId, data: data);
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
