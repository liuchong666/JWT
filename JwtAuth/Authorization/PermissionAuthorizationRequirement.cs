using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuth.Authorization
{
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public PermissionAuthorizationRequirement(string code)
        {
            Code = code;
        }

        public string Code { get; set; }
    }
}
