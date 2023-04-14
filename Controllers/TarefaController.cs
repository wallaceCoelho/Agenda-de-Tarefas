using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        
        [HttpGet("Index"), AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Criar")]
        public IActionResult Criar()
        {
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

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if(tarefaBanco == null)
                return NotFound();

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.DataTarefa = tarefa.DataTarefa;

            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();

            return Ok(tarefaBanco);
        }
    }
}