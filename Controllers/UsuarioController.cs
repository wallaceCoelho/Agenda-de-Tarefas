using Microsoft.AspNetCore.Mvc;
using ProjetoTarefas.Data;
using ProjetoTarefas.Models;
using Microsoft.AspNetCore.Authorization;
using ProjetoTarefas.Interfaces;
using ProjetoTarefas.Services;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ProjetoTarefas.Controllers
{
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        // private string codigo;
        HashPassword hash = new HashPassword(SHA256.Create());
        private readonly ProjetoContext _context;
        private readonly IEnviarEmail _enviarEmail;
        private readonly ICookieService _cookieService;
        public UsuarioController(
            ProjetoContext context, 
            ICookieService cookieService,
            IEnviarEmail enviarEmail)
        {
            _enviarEmail = enviarEmail;
            _cookieService = cookieService;
            _context = context;
        }
        
        [HttpGet("Index"), AllowAnonymous]
        public IActionResult Index(bool erroLogin)
        {
            ClaimsPrincipal claims = HttpContext.User;
            if(erroLogin) ViewBag.Erro = "Login e/ou senha inválidos!";

            if(claims.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Tarefa");

            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            if (usuario.Email == null || usuario.Senha == null)
            {
                return RedirectToAction("Index", new { erroLogin = true});
            }

            var usuarios = _cookieService.ValidarUsuario(usuario.Email, usuario.Senha);

            if (usuarios == null)
            {
                return RedirectToAction("Index", new { erroLogin = true});
            }

            await _cookieService.GerarClaim(HttpContext, usuarios);

            return RedirectToAction("Index", "Tarefa");
        }

        [HttpGet("Logout"), Authorize]
        public async Task<IActionResult> Logout()
        {
            await _cookieService.Logout(HttpContext);
            return RedirectToAction("Index");
        }

        [HttpGet("Criar")]
        public IActionResult Criar(bool erroLogin)
        {
            ClaimsPrincipal claims = HttpContext.User;

            if(erroLogin) ViewBag.Erro = "Email já existe, ou algum campo está incompleto.";

            if(claims.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Tarefa");
                
            return View();
        }

        [HttpPost("CriarUsuario")]
        public IActionResult Criar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // codigo = _enviarEmail.GerarCodigo();

                // var msg = $"<h4>Prodigy</h4><br><br><h5>{codigo}</h5>";

                // await _enviarEmail.Email(usuario.Email, msg);
                var usuarios = _context.Usuarios.FirstOrDefault(x => x.Email == usuario.Email);

                string senhaSecreta = hash.CriptografarSenha(usuario.Senha.ToString());

                if(usuarios == null)
                {
                    //  using (var binaryReader = new BinaryReader(model.ImageUpload.InputStream))
                    // produto.Imagem = binaryReader.ReadBytes(model.ImageUpload.ContentLength);
                    // ViewBag.IdUsuarioEmail = novoUsuario.UsuarioId;
                    usuario.Senha = senhaSecreta;
                    _context.Usuarios.Add(usuario);
                    _context.SaveChanges();
                    
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction("Criar", new { erroLogin = true });
        }

        // [HttpGet("ConfirmarEmail")]
        // public IActionResult ConfirmarEmail(bool erroLogin)
        // {
        //     if(erroLogin) ViewBag.Erro = "O código não confere.";
        //     return View();
        // }

        // [HttpPost("ConfirmarEmailPost")]
        // public IActionResult ConfirmarEmail(string cod)
        // {
        //     string usuarioId = ViewBag.IdUsuarioEmail;

        //     var usuarios = _context.Usuarios
        //         .AsNoTracking()
        //         .FirstOrDefault(x => x.UsuarioId.ToString() == usuarioId);

        //     if(cod == codigo && usuarios != null)
        //     {
        //         usuarios.TokenEmail = cod;
        //         _context.Usuarios.Update(usuarios);
        //         _context.SaveChanges();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     _context.Usuarios.Remove(usuarios);
        //     _context.SaveChanges();
        //     return View(new { erroLogin = true });
        // }
    }
}