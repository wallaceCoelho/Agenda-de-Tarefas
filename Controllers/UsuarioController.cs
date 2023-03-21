using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ProjetoTarefas.Data;
using ProjetoTarefas.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace ProjetoTarefas.Controllers
{
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        private Hash hash = new Hash(SHA256.Create());
        private readonly ProjetoContext _context;

        public UsuarioController(ProjetoContext context)
        {
            _context = context;
        }
        
        [Route("Index")]
        public IActionResult Index()
        {
            var usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Usuario usuario)
        {
            string senhaDigitada = hash.CriptografarSenha(usuario.Senha.ToString());

            if(_context.Usuarios.Any(x => x.Email == usuario.Email)
                && _context.Usuarios.Any(x => x.Senha == senhaDigitada))
            {
                return View("Views/Tarefa/Index.cshtml");
            }
            else
            {
                Console.WriteLine("Usuário ou senha inválidos");
                return View(usuario);
            }
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