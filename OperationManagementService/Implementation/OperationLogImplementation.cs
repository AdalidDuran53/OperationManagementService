using Azure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OperationManagementService.Models;

namespace OperationManagementService.Implementation
{
    public class OperationLogImplementation
    {
        private readonly OperationContext _context;
        public OperationLogImplementation(OperationContext context)
        {
            _context = context;
        }

        internal void LogOperation(Dictionary<string, object> operationRequest, Dictionary<string, object> operationResponse, Guid? sessionId = null)
        {
            string request = string.Empty;
            foreach (var item in operationRequest)
            {
                request += string.Concat(item.Key, " : ", JsonConvert.SerializeObject(item.Value));
            }
            string response = string.Empty;
            foreach (var item in operationResponse)
            {
                response += string.Concat(item.Key, " : ", JsonConvert.SerializeObject(item.Value));
            }

            OperationLog newLogOperation = new OperationLog(sessionId: new Guid(), operationDate: DateTime.Now, request: request, response: response);
            _context.OperationLogs.Add(newLogOperation);
            _context.SaveChangesAsync();
        }
    }
}
