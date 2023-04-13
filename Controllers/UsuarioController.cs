using Microsoft.AspNetCore.Mvc;
using ProjetoTarefas.Data;
using ProjetoTarefas.Models;
using Microsoft.AspNetCore.Authorization;
using ProjetoTarefas.Interfaces;
using ProjetoTarefas.Services;
using System.Security.Cryptography;
using System.Text.Json;

namespace ProjetoTarefas.Controllers
{
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        HashPassword hash = new HashPassword(SHA256.Create());
        private readonly ProjetoContext _context;
        private readonly ICookieService _cookieService;

        public UsuarioController(
            ProjetoContext context, 
            ICookieService cookieService)
        {
            _cookieService = cookieService;
            _context = context;
        }
        
        [HttpGet("Index"), AllowAnonymous]
        public IActionResult Index(bool erroLogin)
        {
            if(erroLogin) ViewBag.Erro = "Login e/ou senha inválidos!";

            return View();
        }

        [HttpGet("Login"), AllowAnonymous]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            var usuarios = await _cookieService.ValidarUsuario(usuario.Email, usuario.Senha, HttpContext);

            if (usuarios == null)
            {
                return RedirectToAction("Index", new { erroLogin = true});
            }

            return RedirectToAction("Index", "Tarefa");
        }

        [HttpGet("Logout"), Authorize]
        public async Task<IActionResult> Logout()
        {
            await _cookieService.Logout(HttpContext);
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
                var novoUsuario = NovoUsuario(usuario, senhaSecreta);
                _context.Usuarios.Add(novoUsuario);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        private Usuario NovoUsuario(Usuario usuario, string senhaSecreta)
        {
            var novoUsuario = new Usuario
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Senha = senhaSecreta,
                    Nivel = usuario.Nivel,
                    DataNascimento = usuario.DataNascimento
                };
            return novoUsuario;
        }
    }
}