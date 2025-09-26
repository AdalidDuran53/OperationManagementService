using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationManagementService.Models;
using OperationManagementService.OperationExceptions;

namespace OperationManagementService.Functionality
{
    public class FunctionalityBaseController : Controller
    {
        public readonly ErrorServiceModel _errorService = new ErrorServiceModel();
        public FunctionalityBaseController()
        {
            _errorService = new ErrorServiceModel();
        }
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
    }
}
