using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoTarefas.Data;
using ProjetoTarefas.Interfaces;
using ProjetoTarefas.Models;

namespace ProjetoTarefas.Controllers
{
    [Route("[controller]"), Authorize]
    public class TarefaController : Controller
    {
        private readonly ProjetoContext _context;
        private readonly ICookieService _cookieService;

        public TarefaController(ProjetoContext context, ICookieService cookieService)
        {
            _cookieService = cookieService;
            _context = context;
        } 
        
        [AllowAnonymous]
        public void AddSessao()
        {
            ViewBag.Id = User.FindFirst("Id")?.Value;
            ViewBag.Nome = User.FindFirst(ClaimTypes.Name)?.Value;
            ViewBag.Email = User.FindFirst(ClaimTypes.Email)?.Value;
            ViewBag.Nivel = User.FindFirst(ClaimTypes.Role)?.Value;
        }
        [HttpGet("Index")]
        public IActionResult Index()
        {

            var UsuarioId = User.FindFirst("Id")?.Value;
            var tarefas = _context.Tarefas.Where(x => x.UsuarioId.ToString() == UsuarioId).ToList();

            AddSessao();

            return View(tarefas);
        }

        [HttpGet("Criar")]
        public IActionResult Criar()
        {
            AddSessao();
            return View();
        }

        [HttpPost]
        public IActionResult CriarTarefa(Tarefa tarefa)
        {
            if (ModelState.IsValid)
            {
                var novaTarefa = NovaTarefa(tarefa);
                _context.Tarefas.Add(novaTarefa);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        private Tarefa NovaTarefa(Tarefa tarefa)
        {
            var novaTarefa = new Tarefa
            {
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                DataTarefa = tarefa.DataTarefa,
                UsuarioId = tarefa.UsuarioId
            };
                return novaTarefa;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var tarefa = _context.Tarefas.Find(id);

            if (tarefa == null)
                return NotFound();

            return Ok(tarefa);
        }

        [HttpGet("ObterPorNome")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            var tarefa = _context.Tarefas.Where(x => x.Titulo.Contains(titulo));

            return Ok(tarefa);
        }

        [HttpGet("Editar/{id}")]
        public IActionResult Editar(int id)
        {
            var tarefaBanco = _context.Tarefas.Where(x => x.TarefaId == id).FirstOrDefault();
            
            if(tarefaBanco != null) return View("Criar", tarefaBanco);

            return RedirectToAction("Index");
        }

        [HttpPut("EditarTarefa")]
        public IActionResult EditarTarefa(Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Where(x => x.TarefaId == tarefa.TarefaId).FirstOrDefault();

            if(tarefaBanco == null)
                return RedirectToAction("Criar", tarefaBanco);

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.DataTarefa = tarefa.DataTarefa;

            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}