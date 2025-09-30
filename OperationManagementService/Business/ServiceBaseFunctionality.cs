using EntitiesCustom;
using Newtonsoft.Json;

namespace OperationManagementService.Business
{
    public class ServiceBaseFunctionality : FunctionalityBaseController
    {

        internal async void LogOperation(Dictionary<string, object> operationRequest, Dictionary<string, object> operationResponse, Guid? sessionId = null)
        {
            try
            {
                // init request and response strings
                string request = string.Empty;
                string response = string.Empty;
                // serialize the request and response dictionaries
                foreach (var item in operationRequest)
                {
                    request += string.Concat(item.Key, " : ", JsonConvert.SerializeObject(item.Value));
                }
                foreach (var item in operationResponse)
                {
                    response += string.Concat(item.Key, " : ", JsonConvert.SerializeObject(item.Value));
                }
                // create a new operation log object
                OperationLog newLogOperation = new OperationLog(sessionId: sessionId, operationDate: DateTime.Now, request: request, response: response);
                // save the operation log object
                using (var context = new Models.OperationContext())
                {
                    // map the operation log object to the entity model
                    var newLog = Mapster.TypeAdapter.Adapt<Models.OperationLog>(newLogOperation);
                    context.OperationLogs.Add(newLog);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
            }

        }
    }
}
