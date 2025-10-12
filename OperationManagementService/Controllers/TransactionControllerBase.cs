using EntitiesCustom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace OperationManagementService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class TransactionControllerBase : ControllerBase
    {

        [HttpPost]
        [EnableRateLimiting("IpPolicy")]
        [Route("~/{version}/Transactions/")]
        [SwaggerResponse(statusCode: 200, type: typeof(ActionResult), description: "Ok")]
        [SwaggerResponse(statusCode: 400, type: typeof(ActionResult), description: "Bab Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized")]
        public abstract Task<IActionResult> AddTransaction([FromRoute][Required][RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$")] string version, [Required] Guid userId, [Required] Guid sessionId, [Required] string transactionName, [Required] decimal transactionAmount);

        [HttpPut]
        [EnableRateLimiting("IpPolicy")]
        [Route("~/{version}/Transactions/")]
        [SwaggerResponse(statusCode: 200, type: typeof(ActionResult), description: "Ok")]
        [SwaggerResponse(statusCode: 400, type: typeof(ActionResult), description: "Bab Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized")]
        public abstract Task<IActionResult> UpdateTransaction([FromRoute][Required][RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$")] string version, [Required] Guid userId, [Required] Guid sessionId, [Required] int transactionId, string transactionName = null, decimal? transactionAmount = null);

        [HttpDelete]
        [EnableRateLimiting("IpPolicy")]
        [Route("~/{version}/Transactions/")]
        [SwaggerResponse(statusCode: 200, type: typeof(ActionResult), description: "Ok")]
        [SwaggerResponse(statusCode: 400, type: typeof(ActionResult), description: "Bab Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized")]
        public abstract Task<IActionResult> DeleteTransaction([FromRoute][Required][RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$")] string version, [Required] Guid userId, [Required] Guid sessionId, [Required] int transactionId);

        [HttpGet]
        [EnableRateLimiting("IpPolicy")]
        [Route("~/{version}/Transactions/")]
        [SwaggerResponse(statusCode: 200, type: typeof(ActionResult), description: "Ok")]
        [SwaggerResponse(statusCode: 400, type: typeof(ActionResult), description: "Bab Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized")]
        public abstract Task<IActionResult> GetTransactions([FromRoute][Required][RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$")] string version, [Required] Guid userId, [Required] Guid sessionId, int? transactionId);

    }
}
