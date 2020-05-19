using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwt.DTOModels
{
    public class LoginRequestDTO
    {
        public string username { get; set; }
        public string password { get; set; }

    }
}
