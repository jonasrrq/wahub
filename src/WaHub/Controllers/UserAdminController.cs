using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // For ToListAsync()
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaHub.Data; // For ApplicationUser

namespace WaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserAdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserAdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public class UserViewModel // Expected by Roles.razor
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string FirstName {get; set; }
            public string LastName {get; set; }
        }

        [HttpGet("users")]
        public async Task<ActionResult<List<UserViewModel>>> GetUsersAsync()
        {
            var users = await _userManager.Users
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                })
                .ToListAsync();
            return Ok(users);
        }
    }
}
