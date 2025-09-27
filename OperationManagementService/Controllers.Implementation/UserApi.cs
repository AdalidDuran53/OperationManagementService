using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using OperationManagementService.Functionality;
using OperationManagementService.Models;
using OperationManagementService.OperationExceptions;
using System.ComponentModel.DataAnnotations;

namespace OperationManagementService.Controllers.Implementation
{
    [ApiVersion("0.1")]
    [ApiController]
    [Authorize]
    public class UserApi : UserControllerBase
    {

        private readonly UserFunctionality _userFunctionality;
        private readonly ServiceBaseFunctionality _serviceBaseFunctionality;
        private readonly ErrorServiceModel _errorService = new ErrorServiceModel();
        public UserApi(UserFunctionality userFunctionality, ServiceBaseFunctionality serviceBaseFunctionality)
        {
            _userFunctionality = userFunctionality;
            _serviceBaseFunctionality = serviceBaseFunctionality;
            _errorService = new ErrorServiceModel();
        }

        [HttpPost]
        [Route("~/{version::apiVersion}/Users/")]
        public override async Task<IActionResult> AddUser([FromRoute, RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$"), Required] string version, [Required] string userName, [Required] string password)
        {
            // Log the request
            Dictionary<string, object> request = new Dictionary<string, object> { { "CreateNewUserRequest", new object[] { "version: " + version, "userName: " + userName } } };
            try
            {
                // Call the implementation
                var result = await _userFunctionality.AddUser(userName, password);
                // Log the response
                Dictionary<string, object> response = new Dictionary<string, object> { { "CreateNewUserResponse", result } };
                // Log the operation
                _serviceBaseFunctionality.LogOperation(request, response);
                // return the result
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                Dictionary<string, object> response = new Dictionary<string, object> { { "ErrorCreateNewUserResponse", ex } };
                _serviceBaseFunctionality.LogOperation(request, response);
                // if the exception is an OperationException, return a bad request with the error details
                OperationException excep = ((OperationException)ex);
                return this.BadRequest(new { success = false, code = excep.ErrorCode, message = excep.Message, details = excep.Details});
            }
        }
    }
}
