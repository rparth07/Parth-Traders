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

        [HttpGet("admin-details")]
        public AdminDataModel GetAdminsDetails(string adminId)
        {
            return _userManager.Users.FirstOrDefault(_ => _.Id == adminId);
        }
    }
}
