using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Parth_Traders.Data.DataModel.User;
using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Domain.RepositoryInterfaces.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Parth_Traders.Data.Repositories.User
{
    public class CustomerAuthManager : ICustomerAuthManager
    {
        private readonly UserManager<CustomerDataModel> _customerManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private CustomerDataModel _customer;
        public CustomerAuthManager(UserManager<CustomerDataModel> customerManager,
                                     IConfiguration configuration,
                                     IMapper mapper)
        {
            _customerManager = customerManager;
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

        public async Task<bool> ValidateCustomer(CustomerForAuthentication customer)
        {
            var customerAuth = _mapper.Map<CustomerForAuthDataModel>(customer);
            _customer = await _customerManager.FindByEmailAsync(customerAuth.Email);

            return (_customer != null 
                && await _customerManager
                .CheckPasswordAsync(_customer, customerAuth.Password));
        }

        private static SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes("PARTHTRADERSSECRETNEWLOGINVALIDATIONKEY");
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _customer.UserName)
            };
            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials,
                                                      List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken
            (
            issuer: jwtSettings.GetSection("validIssuer").Value,
            audience: jwtSettings.GetSection("validAudience").Value,
            claims: claims,
            expires: DateTime.Now
                .AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expires").Value)),
            signingCredentials: signingCredentials
            );
            return tokenOptions;
        }
    }
}
