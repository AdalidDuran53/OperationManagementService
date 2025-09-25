using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationManagementService.Implementation;
using OperationManagementService.Models;
using System.ComponentModel.DataAnnotations;

namespace OperationManagementService.Controllers.Implementation
{
    [ApiVersion("0.1")]
    [ApiController]
    [Authorize]
    public class UserApi : UserControllerBase
    {

        private readonly UserImplementation _userImplementation;
        private readonly OperationLogImplementation _operationLogImplementation;
        public UserApi(UserImplementation userImplementation, OperationLogImplementation operationLogImplementation)
        {
            _userImplementation = userImplementation;
            _operationLogImplementation = operationLogImplementation;
        }

        [HttpPost]
        [Route("~/{version::apiVersion}/Users/")]
        public override async Task<IActionResult> Create([FromRoute, RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$"), Required] string version, [Required] string userName, [Required] string password)
        {
            Dictionary<string, object> request = new Dictionary<string, object> { { "CreateNewUserRequest", new object[] { "version: " + version, "userName: " + userName } } };
            try
            {
                var result = await _userImplementation.AddUser(userName, password);
                Dictionary<string, object> response = new Dictionary<string, object> { { "CreateNewUserResponse", result } };
                _operationLogImplementation.LogOperation(request, response);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Dictionary<string, object> response = new Dictionary<string, object> { { "ErrorCreateNewUserResponse", ex } };
                _operationLogImplementation.LogOperation(request, response);
                return this.BadRequest(ex.Message);
            }
        }
    }
}
