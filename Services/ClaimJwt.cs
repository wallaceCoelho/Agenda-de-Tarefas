using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ProjetoTarefas.Models;

namespace ProjetoTarefas.Services
{
    public class ClaimsJwt
    {
        public async Task Login(HttpContext ctx, Usuario ususario)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, ususario.UsuarioId.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, ususario.Nome));
            claims.Add(new Claim(ClaimTypes.Role, ususario.Nivel));

            var claimsIdentity =
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme
                    )
                );

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.Now.AddHours(8),
                IssuedUtc = DateTime.Now
            };

            await ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsIdentity, authProperties);
            
        } 
        public async Task Logoff(HttpContext ctx)
        {
            await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}