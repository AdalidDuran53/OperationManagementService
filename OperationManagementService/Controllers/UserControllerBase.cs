using Microsoft.AspNetCore.Mvc;
using OperationManagementService.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace OperationManagementService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class UserControllerBase : ControllerBase
    {

        [HttpPost]
        [Route("~/{version}/Users/")]
        [SwaggerOperation(OperationId = "CreateUser")]
        [SwaggerResponse(statusCode: 200, type: typeof(ActionResult), description: "Ok")]
        [SwaggerResponse(statusCode: 400, type: typeof(ActionResult), description: "Bab Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized")]
        public abstract Task<IActionResult> Create([FromRoute][Required][RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$")] string version, [Required] string userName, [Required] string password);

        [HttpPut]
        [Route("~/Update")]
        [SwaggerResponse(statusCode: 200, type: typeof(ActionResult), description: "Ok")]
        [SwaggerResponse(statusCode: 400, type: typeof(ActionResult), description: "Bab Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized")]
        public ActionResult Update()
        {
            return Ok(new { success = true, message = "Datos guardados correctamente" });
        }

        [HttpDelete]
        [Route("~/Delete")]
        [SwaggerResponse(statusCode: 200, type: typeof(ActionResult), description: "Ok")]
        [SwaggerResponse(statusCode: 400, type: typeof(ActionResult), description: "Bab Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized")]
        public ActionResult Delete()
        {
            return Ok(new { success = true, message = "Datos eliminados correctamente" });
        }

    }
}
