using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoTarefas.Models
{
    public class Tarefa
    {
        [Display(Name = "Cód.")]
        public int TarefaId { get; set; }
        //
        [Required(ErrorMessage = "*Título obrigatório!")]
        [MaxLength(100, ErrorMessage = "O título não pode ultrapaçar 20 caracteres!")]
        [Display(Name = "Título da tarefa")]
        public string Titulo { get; set; }
        //
        [Required(ErrorMessage = "*Descrição obrigatório!")]
        [MaxLength(100, ErrorMessage = "A Descrição não pode ultrapaçar 250 caracteres!")]
        [Display(Name = "Descrição da tarefa")]
        public string Descricao { get; set; }
        //
        [Required(ErrorMessage = "*Data obrigatório!")]
        [Display(Name = "Data da tarefa")]
        public DateTime DataTarefa { get; set; }
        
        [ForeignKey("UsuarioId")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}