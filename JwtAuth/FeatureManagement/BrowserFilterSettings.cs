using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuth.FeatureManagement
{
    public class BrowserFilterSettings
    {
        public string[] AllowedBrowsers { get; set; }
    }

    [FilterAlias("BrowserFilter")]
    public class BrowserFeatureFilter : IFeatureFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BrowserFeatureFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            var userAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
            var settings = context.Parameters.Get<BrowserFilterSettings>();
            return Task.FromResult(settings.AllowedBrowsers.Any(c=>userAgent.Contains(c)));
        }
    }
}
