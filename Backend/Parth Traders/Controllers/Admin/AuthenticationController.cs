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
        private readonly UserManager<AdminAuthenticationDataModel> _userManager;
        private readonly IAuthenticationManager _authManager;
        private readonly IMapper _mapper;
        public AuthenticationController(IMapper mapper,
                                        UserManager<AdminAuthenticationDataModel> adminManager,
                                        IAuthenticationManager authManager)
        {
            _mapper = mapper;
            _userManager = adminManager;
            _authManager = authManager;
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> RegisterUser(AdminUserDto userForRegistration)
        {
            var adminUser = _mapper.Map<AdminAuthenticationDataModel>(userForRegistration);
            var result = await _userManager.CreateAsync(adminUser, userForRegistration.Password);
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
        public async Task<IActionResult> Authenticate([FromBody] AdminAuthenticationDto admin)
        {
            if (!await _authManager.ValidateAdmin(_mapper.Map<AdminAuthentication>(admin)))
            {
                return Unauthorized();
            }
            return Ok(new { Token = await _authManager.CreateToken() });
        }
    }

}
