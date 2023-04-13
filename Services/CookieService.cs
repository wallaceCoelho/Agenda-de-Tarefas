using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ProjetoTarefas.Data;
using ProjetoTarefas.Interfaces;
using ProjetoTarefas.Models;

namespace ProjetoTarefas.Services
{
    public class CookieService : ICookieService
    {

        HashPassword hash = new HashPassword(SHA256.Create());
        private readonly ProjetoContext _context;
        public CookieService(
            ProjetoContext context)
        {
            _context = context;
        }
        public async Task GerarClaim(HttpContext context, Usuario usuario)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("Id", usuario.UsuarioId.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, usuario.Email));
            claims.Add(new Claim(ClaimTypes.Name, usuario.Nome));
            claims.Add(new Claim(ClaimTypes.Role, usuario.Nivel));

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
            
            await context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                claimsIdentity, 
                authProperties);
        } 
        public async Task Logout(HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<Usuario> ValidarUsuario(string email, string senha, HttpContext context)
        {
            var senhaCriptografada = hash.CriptografarSenha(senha);

            var user = _context.Usuarios.FirstOrDefault(
                x => x.Email == email &&
                x.Senha == senhaCriptografada);

            await GerarClaim(context, user);
            
            return user;
        }
    }
}