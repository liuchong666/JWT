using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement.Mvc;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JwtAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        // GET: api/<AuthController>
        [FeatureGate("FeatureC")]
        [FeatureGate("BrowserFlag")]
        [HttpGet]
        public IActionResult Get()
        {
            using (MiniProfiler.Current.Step("观察者模式"))
            {
                _mediator.Publish(new LogEvent("Hello World"));
            }

            using (MiniProfiler.Current.Step("获取JWT Token"))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes("WeatherForecastControllerWeatherForecastController");
                var expiresAt = DateTime.Now.AddMinutes(30);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(JwtClaimTypes.Audience, "api"),
                    new Claim(JwtClaimTypes.Issuer, "api"),
                    new Claim(JwtClaimTypes.Id, "1"),
                    new Claim(JwtClaimTypes.Name, "zhangsan"),
                    new Claim(JwtClaimTypes.Email, "zhangsan@mail.com"),
                    new Claim(JwtClaimTypes.PhoneNumber, "123456")
                    }),
                    Expires = expiresAt,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new
                {
                    token = tokenString
                });
            }
            
        }

        // GET api/<AuthController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            using (MiniProfiler.Current.Step("Get方法"))
            {
                using (MiniProfiler.Current.Step("准备数据"))
                {
                    using (MiniProfiler.Current.CustomTiming("SQL", "SELECT * FROM Config"))
                    {
                        // 模拟一个SQL查询
                        Thread.Sleep(500);
                    }

                    using (MiniProfiler.Current.CustomTiming("SQL", "SELECT * FROM Config1"))
                    {
                        // 模拟一个SQL查询
                        Thread.Sleep(500);
                    }
                }


                using (MiniProfiler.Current.Step("使用从数据库中查询的数据，进行Http请求"))
                {
                    string url1 = "https://www.baidu.com";
                    string url2 = "https://www.sina.com.cn/";
                    using (MiniProfiler.Current.CustomTiming("HTTP", "GET " + url1))
                    {
                        var client = new WebClient();
                        var reply = client.DownloadString(url1);
                    }

                    using (MiniProfiler.Current.CustomTiming("HTTP", "GET " + url2))
                    {
                        var client = new WebClient();
                        var reply = client.DownloadString(url2);
                    }
                }
            }
            return "value";
        }

        // POST api/<AuthController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AuthController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
