using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using TestTaskVK.Models;

namespace TestTaskVK.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        ApplicationContext db;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ApplicationContext context) : base(options, logger, encoder, clock)
        {
            db = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Authorization header was not found");

            try
            {
                var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter!);
                string[] credentials = Encoding.UTF8.GetString(bytes).Split(":");
                string login = credentials[0];
                string password = credentials[1];
                User? user = db.Users.Where(x => x.Login == login && x.Password == password && x.UserState.Code == CodeState.Active).FirstOrDefault();
                if (user == null)
                {
                    if (db.Users.Count() == 0)
                    {
                        return AuthenticateResult.Success(GetTiket(""));
                    }
                    AuthenticateResult.Fail("Invalid login or password");
                }
                else
                {
                    return AuthenticateResult.Success(GetTiket(user.Login));
                }
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("Error has occured");
            }
            return AuthenticateResult.Fail("");

            AuthenticationTicket GetTiket(string name)
            {
                var claims = new[] { new Claim(ClaimTypes.Name, name) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                return new AuthenticationTicket(principal, Scheme.Name);
            }
        }
    }
}
