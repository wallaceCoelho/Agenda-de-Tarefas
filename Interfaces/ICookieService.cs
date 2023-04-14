using ProjetoTarefas.Models;

namespace ProjetoTarefas.Interfaces
{
    public interface ICookieService
    {
        Usuario ValidarUsuario(string email, string senha);
        Task GerarClaim(HttpContext context, Usuario usuario);
        Task Logout(HttpContext context);
    }
}