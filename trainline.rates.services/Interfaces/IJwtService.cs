using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace trainline.rates.services.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateSecurityToken();
    }
}
