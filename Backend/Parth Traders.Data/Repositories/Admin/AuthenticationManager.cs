using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Parth_Traders.Data.DataModel.Admin;
using Parth_Traders.Domain.Entity.Admin;
using Parth_Traders.Domain.RepositoryInterfaces.Admin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Data.Repositories.Admin
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<AdminDataModel> _adminManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private AdminDataModel _admin;
        public AuthenticationManager(UserManager<AdminDataModel> adminManager, 
                                     IConfiguration configuration, 
                                     IMapper mapper)
        {
            _adminManager = adminManager;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<bool> ValidateAdmin(AdminForAuthentication admin)
        {
            var adminAuth = _mapper.Map<AdminForAuthenticationDataModel>(admin);
            _admin = await _adminManager.FindByNameAsync(adminAuth.UserName);

            return (_admin != null && await _adminManager.CheckPasswordAsync(_admin,
           adminAuth.Password));
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key =
           Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _admin.UserName)
            };
            var roles = await _adminManager.GetRolesAsync(_admin);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials
       signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken
            (
            issuer: jwtSettings.GetSection("validIssuer").Value,
            audience: jwtSettings.GetSection("validAudience").Value,
            claims: claims,
            expires:
           DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expires").Value)),
            signingCredentials: signingCredentials
            );
            return tokenOptions;
        }
    }
}
