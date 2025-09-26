using Microsoft.AspNetCore.Mvc;

namespace OperationManagementService.OperationExceptions
{
    public interface IValidation
    {
        public string Validate(string operationExceptionCode);
    }
}
