using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using trainline.rates.services.Interfaces;

namespace trainline.rates.services
{
    /// <summary>
    /// JWT token service
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly string _secret;
        private readonly string _issuer;

        public JwtService(IConfiguration config)
        {
            _secret = config.GetSection("JwtConfig").GetSection("secret").Value;
            _issuer = config.GetSection("JwtConfig").GetSection("Issuer").Value;
        }

        /// <summary>
        /// Generate a JWT Token
        /// </summary>
        /// <returns>Valid JWT token</returns>
        public async Task<string> GenerateSecurityToken()
        {
            var task = Task.Run(() => {
                if (string.IsNullOrWhiteSpace(_secret) || string.IsNullOrWhiteSpace(_issuer))
                    return string.Empty;

                // initialize key and credentials
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                // Create new JWT token
                var token = new JwtSecurityToken(_issuer,
                    _issuer,
                    null,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: credentials);

                // Serialize JWT token
                return new JwtSecurityTokenHandler().WriteToken(token);
            });

            return await task;
        }
    }
}
