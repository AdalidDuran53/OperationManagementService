using Microsoft.AspNetCore.Mvc;
using OperationManagementService.Models;
using OperationManagementService.OperationExceptions;

namespace OperationManagementService.Functionality
{
    public class UserFunctionality : FunctionalityBaseController
    {
        public async Task<ActionResult> AddUser(string userName, string password)
        {
            try
            {
                // build the user object
                User newUser = new User(userId: Guid.NewGuid(), userName: userName, password: password);
                // validate the user object
                this.ValidateModel(newUser);
                // save the user object
                using (var context = new OperationContext())
                {
                    context.Users.Add(newUser);
                    await context.SaveChangesAsync();
                }
                // return the result
                var result = Ok(new { success = true, message = "Data saved successfully" });
                return result;
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
    }
}
