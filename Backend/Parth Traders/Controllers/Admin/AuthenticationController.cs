using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
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
            return Ok(new { Token = await _authManager.CreateToken(),
                            Admin = _userManager.Users
                                        .FirstOrDefault(_ => _.UserName == admin.UserName)});
        }

        [HttpPost("admin-details/{adminId}")]
        public async Task<IActionResult> UpdateAdminDetailsAsync(string adminId,
                                                                 [FromBody] AdminDataModel adminDetailsToUpdate)
        {
            var admin = await _userManager.FindByIdAsync(adminId);
            admin.FirstName = adminDetailsToUpdate.FirstName;
            admin.LastName = adminDetailsToUpdate.LastName;
            admin.UserName = adminDetailsToUpdate.UserName;
            admin.NormalizedUserName = admin.UserName.ToUpper();
            admin.Email = adminDetailsToUpdate.Email; //confirm this
            admin.NormalizedEmail = adminDetailsToUpdate.Email.ToUpper();
            admin.PhoneNumber = adminDetailsToUpdate.PhoneNumber; // confirm this

            await _userManager.UpdateAsync(admin);
            AdminForAuthenticationDto adminDto = new AdminForAuthenticationDto()
            {
                UserName = admin.UserName,
                Password = admin.PasswordHash
            };
            return await Authenticate(adminDto);
        }

        [HttpDelete("{adminId}")]
        public async Task<IdentityResult> DeleteAdmin(string adminId)
        {
            var admin = await _userManager.FindByIdAsync(adminId);
            return await _userManager.DeleteAsync(admin);
        }

    }
}
