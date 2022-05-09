using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Praksa.DAL.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace API
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UserAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private Role _neededUserRole;


        public UserAuthorizationAttribute(Role neededUserRole = Role.Basic)
        {
            _neededUserRole = neededUserRole;
        }


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            SecurityToken validatedToken = new JwtSecurityToken(token);

            var key = Encoding.ASCII.GetBytes("DwLZfnVk394X1a8XReNfmx2XBpySsL7W");

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero

                }, out validatedToken);
            }
            catch
            {
                context.Result = new JsonResult(new { message = "Error: Invalid Token." }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            var jwtToken = (JwtSecurityToken)validatedToken;

            var claims = jwtToken.Claims.ToDictionary(e => e.Type, e => (object)e.Value);

            if ((Role)Enum.Parse(typeof(Role), claims["Role"].ToString()!) < _neededUserRole)
            {
                context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };

                return;
            }

            context.HttpContext.Items["Id"] = claims["Id"];
            context.HttpContext.Items["Role"] = claims["Role"];
            context.HttpContext.Items["UserName"] = claims["UserName"];
        }
    }
}
