using Microsoft.AspNetCore.Mvc;
using ProjetoTarefas.Data;
using ProjetoTarefas.Services;
using ProjetoTarefas.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
namespace ProjetoTarefas.Controllers
{
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        ClaimsJwt login = new ClaimsJwt();   
        private HashAuth hash = new HashAuth(SHA256.Create());
        private readonly ProjetoContext _context;

        public UsuarioController(ProjetoContext context)
        {
            _context = context;
        }
        
        [Route("Index")]
        public IActionResult Index(bool erroLogin)
        {
            var usuarios = _context.Usuarios.ToList();
            if (erroLogin)
            {
                ViewBag.Error = "Email e/ou senha inválidos";
            }
            return View(usuarios);
        }

        private bool ValidarUsuario(Usuario login)
        {
            string senhaDigitada = hash.CriptografarSenha(login.Senha.ToString());

            if(_context.Usuarios.Any(x => x.Email == login.Email)
                && _context.Usuarios.Any(x => x.Senha == senhaDigitada))
            {
                return true;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email e/ou senha inválido!");
                return false;
            }
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            bool resultado = ValidarUsuario(usuario);
            if (!resultado)
            {
                return RedirectToAction("Index", new { erroLogin = true});
            }
            await login.Login(HttpContext, usuario);

            return RedirectToPage("/Views/Tarefa/Index.cshtml");
        }

        [Authorize]
        public async Task<IActionResult> Logoff()
        {
            await login.Logoff(HttpContext);

            return RedirectToAction("Index");
        }

        [HttpGet("Criar")]
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost("CriarUsuario")]
        public IActionResult Criar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                string senhaSecreta = hash.CriptografarSenha(usuario.Senha.ToString());
                usuario.Senha = senhaSecreta;
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                return View("Index");
            }
            return View(usuario);
        }
    }
}