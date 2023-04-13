using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoTarefas.Data;
using ProjetoTarefas.Interfaces;
using ProjetoTarefas.Models;

namespace ProjetoTarefas.Controllers
{
    [Route("[controller]")]
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
            ViewBag.Nivel = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value);
            ViewBag.Nome = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).Select(x => x.Value);
            ViewBag.UsuarioId = HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value);

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
            try{
                if (ModelState.IsValid)
                {
                    _context.Tarefas.Add(tarefa);
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"ERRO: {e}");
            }
            
            return View("Criar");
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