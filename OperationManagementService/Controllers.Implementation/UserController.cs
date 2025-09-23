using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationManagementService.Models;
using System.ComponentModel.DataAnnotations;

namespace OperationManagementService.Controllers.Implementation
{
    [ApiVersion("0.1")]
    [ApiController]
    [Authorize]
    public class UserController : UserControllerBase
    {
        [HttpPost]
        [Route("~/{version::apiVersion}/Users/")]
        public override Task<IActionResult> Create([FromRoute, RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$"), Required] string version, [Required] string userName, [Required] string password)
        {
            try
            {
                return Task.FromResult<IActionResult>(Ok(new { success = true, message = "Datos guardados correctamente" }));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IActionResult>(BadRequest(new { success = false, message = ex.Message }));
            }
        }
    }
}
