using Microsoft.AspNetCore.Mvc;
using OperationManagementService.Models;

namespace OperationManagementService.Implementation
{
    public class UserImplementation : ControllerBase
    {
        private readonly OperationContext _context;
        public UserImplementation(OperationContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> AddUser(string userName, string password)
        {

            User newUser = new User(userId: Guid.NewGuid(), userName: userName, password: password);
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            var result = Ok(new { success = true, message = "Datos guardados correctamente" });

            return null;
        }
    }
}
