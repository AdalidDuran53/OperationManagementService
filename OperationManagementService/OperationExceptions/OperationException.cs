using System.Xml.Serialization;

namespace OperationManagementService.OperationExceptions
{
    public class OperationException : Exception
    {
        public OperationException(){ }
        public OperationException(string errorCode, string message,string details) {
            this.ErrorCode = errorCode;
            this.Message = message;
            this.Details = details;
        }
        public string ErrorCode { get; set; }
        public string Message {  get; set; }
        public string Details { get; set; }
    }
}
