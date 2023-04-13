using ProjetoTarefas.Models;

namespace ProjetoTarefas.Interfaces
{
    public interface ICookieService
    {
        Task<Usuario> ValidarUsuario(string email, string senha, HttpContext context);
        Task GerarClaim(HttpContext context, Usuario usuario);
        Task Logout(HttpContext context);
    }
}