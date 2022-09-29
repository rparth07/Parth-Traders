using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parth_Traders.Data.DataModel.Admin;
using Parth_Traders.Domain.Entity.Admin;
using Parth_Traders.Domain.RepositoryInterfaces.Admin;
using Parth_Traders.Dto.Admin;

namespace Parth_Traders.Controllers.Admin
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AdminUser> _adminManager;
        private readonly IAuthenticationManager _authManager;
        public AuthenticationController(IMapper mapper,
                                        UserManager<AdminUser> adminManager,
                                        IAuthenticationManager authManager)
        {
            _mapper = mapper;
            _adminManager = adminManager;
            _authManager = authManager;
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
