using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoTarefas.Data;
using ProjetoTarefas.Models;

namespace ProjetoTarefas.Controllers
{
    [Route("[controller]"), Authorize]
    public class TarefaController : Controller
    {
        private readonly ProjetoContext _context;

        public TarefaController(ProjetoContext context) => _context = context;
        
        [Route("Index")]
        public IActionResult Index()
        {
            ViewBag.UserIdentity = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier);
            var tarefas = _context.Tarefas.ToList();
            return View(tarefas);
        }

        [HttpGet("Criar")]
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost("CriarTarefa")]
        public IActionResult CriarTarefa(Tarefa tarefa)
        {
            if (ModelState.IsValid)
            {
                _context.Tarefas.Add(tarefa);
                _context.SaveChanges();
                return RedirectToRoute("Index", tarefa);
            }
            return View();
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