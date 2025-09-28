using Microsoft.AspNetCore.Mvc;
using OperationManagementService.OperationExceptions;
using System;
using System.Collections.Generic;

namespace OperationManagementService.Models;

public class CustomResponse : ActionResult
{
    public string Message { get; }
    public Guid UserId { get; }
    public Guid? SessionId { get; }
    public object Data { get; }

    public CustomResponse(string message, Guid userId, Guid? sessionId = null, object data = null)
    {
        this.Message = message;
        this.UserId = userId;
        this.SessionId = sessionId;
        this.Data = data;
    }

    public override async Task ExecuteResultAsync(ActionContext context)
    {
        var response = new
        {
            success = true,
            message = Message,
            data = Data
        };

        var objectResult = new ObjectResult(response)
        {
            StatusCode = StatusCodes.Status200OK
        };

        await objectResult.ExecuteResultAsync(context);
    }
}
