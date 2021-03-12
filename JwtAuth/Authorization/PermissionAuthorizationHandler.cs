using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JwtAuth.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            if (context.User != null)
            {
                var userIdClaim = context.User.FindFirst(_ => _.Type == JwtClaimTypes.Id);
                if (userIdClaim != null)
                {
                    var userId = userIdClaim.Value;

                    // 根据userId查询数据库权限 获取权限code
                    // 对比权限code 是否包含requirement.Code

                    if (requirement.Code == "3002")
                    {
                        context.Succeed(requirement);
                    }
                }
                else
                {
                    context.Fail();
                }
                
            }
            return Task.CompletedTask;
        }
    }
}
