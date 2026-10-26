using AutoMapper;

using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using Parth_Traders.Data.DataModel.Admin;

using Parth_Traders.Domain.Entity.Admin;

using Parth_Traders.Domain.RepositoryInterfaces.Admin;

using Parth_Traders.Dto.Admin;

namespace Parth_Traders.Controllers.Admin

{

    [ApiController]

    [Route("api/authentication")]

    public class AuthenticationController : ControllerBase

    {

        private readonly UserManager<AdminDataModel> _userManager;

        private readonly IAuthenticationManager _authManager;

        private readonly IMapper _mapper;

        public AuthenticationController(IMapper mapper,

                                        UserManager<AdminDataModel> adminManager,

                                        IAuthenticationManager authManager)

        {

            _mapper = mapper;

            _userManager = adminManager;

            _authManager = authManager;

        }

        [HttpPost, Authorize(Roles = "Administrator")]

        [Consumes("application/json")]

        public async Task<IActionResult> RegisterUser(AdminForRegistrationDto adminForRegistration)

        {

            var adminUser = _mapper.Map<AdminDataModel>(adminForRegistration);

            var result = await _userManager.CreateAsync(adminUser, adminForRegistration.Password);

            if (!result.Succeeded)

            {

                foreach (var error in result.Errors)

                {

                    ModelState.TryAddModelError(error.Code, error.Description);

                }

                return BadRequest(ModelState);

            }

            return StatusCode(201);

        }

        [HttpPost("login")]

        public async Task<IActionResult> Authenticate([FromBody] AdminForAuthenticationDto admin)

        {

            if (!await _authManager.ValidateAdmin(_mapper.Map<AdminForAuthentication>(admin)))

            {

                return Unauthorized();

            }

            var account = await _userManager.FindByNameAsync(admin.UserName);
            return Ok(new { Token = await _authManager.CreateToken(), Admin = new { account.Id, account.UserName, account.Email, account.FirstName, account.LastName, account.PhoneNumber } });

        }

        public sealed record AdminProfileUpdate(string? FirstName, string? LastName, string? Email, string? PhoneNumber);
        [HttpPost("admin-details/{adminId}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateAdminDetailsAsync(string adminId, [FromBody] AdminProfileUpdate profile)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != adminId) return Forbid();
            var admin = await _userManager.FindByIdAsync(adminId);
            if (admin == null) return NotFound();
            admin.FirstName = profile.FirstName ?? admin.FirstName;
            admin.LastName = profile.LastName ?? admin.LastName;
            admin.Email = profile.Email ?? admin.Email;
            admin.PhoneNumber = profile.PhoneNumber ?? admin.PhoneNumber;
            var result = await _userManager.UpdateAsync(admin);
            if (!result.Succeeded) return BadRequest(new { message = "Profile could not be updated. Check the email and other details." });
            return Ok(new { Admin = new { admin.Id, admin.UserName, admin.FirstName, admin.LastName, admin.Email, admin.PhoneNumber } });
        }

        [HttpDelete("{adminId}"), Authorize(Roles = "Administrator")]

        public async Task<IActionResult> DeleteAdmin(string adminId)

        {

            var admin = await _userManager.FindByIdAsync(adminId);

            if(admin != null)

            {

                IdentityResult result = await _userManager.DeleteAsync(admin);

                if (!result.Succeeded)

                {

                    foreach (var error in result.Errors)

                    {

                        ModelState.TryAddModelError(error.Code, error.Description);

                    }

                    return BadRequest(ModelState);

                }

            }

            else

            {

                return BadRequest("No user found!");

            }

            return Ok("user deleted successfully.");

        }

    }

}
