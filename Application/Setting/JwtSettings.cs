using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Setting
{
    public class JwtSettings : IAppSettings
    {
        public string Key { get; set; }

        public int TokenExpirationInMinutes { get; set; }

        public int RefreshTokenExpirationInDays { get; set; }
    }
}