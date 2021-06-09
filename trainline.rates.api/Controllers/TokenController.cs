using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trainline.rates.services.Interfaces;

namespace trainline.rates.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : Controller
    {
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _config;

        public TokenController(IJwtService jwtService, IConfiguration config)
        {
            _jwtService = jwtService;
            _config = config;
        }

        /// <summary>
        /// Authorize user for use of Api.
        /// Checks Client token against stored tokens for autentication and if successful then return a JWT token.
        /// JWT tokens are valid for 10 minutes.
        /// </summary>
        /// <param name="ClientToken">Client token used for authentication</param>
        /// <returns>Valid JWT token or a 404 error if exception caught</returns>
        [HttpGet]
        public async Task<IActionResult> Authorize([FromQuery] string ClientToken)
        {
            try
            {
                // Get stored client token to validate users input against
                var clientToken = _config.GetSection("FakeClient").GetSection("Token").Value;

                // Validate token
                if (ClientToken != clientToken)
                    return Unauthorized();

                // Create JWT token
                var token = await _jwtService.GenerateSecurityToken();

                return Ok(token);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
