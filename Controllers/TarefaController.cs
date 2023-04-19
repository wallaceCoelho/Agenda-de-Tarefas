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
        public IActionResult Index(string input)
        {
            var UsuarioId = User.FindFirst("Id")?.Value;

            var tarefas = _context.Tarefas
                .Where(x => x.UsuarioId.ToString() == UsuarioId).ToList();
            AddSessao();
                       
            return View(tarefas);
        }

        [HttpGet("Criar")]
        public IActionResult Criar()
        {
            AddSessao();
            return View();
        }

        [HttpPost("CriarTarefa")]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (ModelState.IsValid)
            {
                var novaTarefa = NovaTarefa(tarefa);
                _context.Tarefas.Add(novaTarefa);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction(nameof(Criar));
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

        [HttpGet("Editar/{id?}")]
        public IActionResult Editar(int? id)
        {
            AddSessao();

            ViewBag.TarefaId = id;
            var tarefaBanco = _context.Tarefas.Find(id);
            if(tarefaBanco != null) return View(tarefaBanco);

            return View();
        }

        [HttpPost("EditarTarefa")]
        public IActionResult Editar(Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas
                .AsNoTracking()
                .FirstOrDefault(x => x.TarefaId == tarefa.TarefaId);

            if(ModelState.IsValid && tarefaBanco != null)
            {
                try
                {
                    tarefaBanco.Titulo = tarefa.Titulo;
                    tarefaBanco.Descricao = tarefa.Descricao;
                    tarefaBanco.DataTarefa = tarefa.DataTarefa;

                    _context.Update(tarefaBanco);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Erro ao salvar as asterações. " +
                        "Tente novamente, caso persista o erro, " +
                        "contate o administrador.");
                }
            }
            return RedirectToAction(nameof(Editar));
        }

        [HttpGet("Deletar/{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefas = _context.Tarefas
                .AsNoTracking()
                .FirstOrDefault(x => x.TarefaId == id);

            if(tarefas != null)
            {
                _context.Tarefas.Remove(tarefas);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}