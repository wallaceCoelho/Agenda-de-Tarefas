using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ProjetoTarefas.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        //
        [Required(ErrorMessage = "*Nome obrigatório!")]
        [MaxLength(100, ErrorMessage = "O nome não pode ultrapaçar 100 caracteres!")]
        [Display(Name = "Nome completo")]
        public string Nome { get; set; }
        //
        [Required(ErrorMessage = "*Email obrigatório!")]
        [MaxLength(100, ErrorMessage = "O e-mail não pode ultrapaçar 100 caracteres!")]
        [Display(Name = "E-mail válido")]
        public string Email { get; set; }
        public string Nivel { get; set; }
        //
        [Required(ErrorMessage = "*Email obrigatório!")]
        [Display(Name = "E-mail válido")]
        public DateTime DataNascimento { get; set; }
        //
        [Required(ErrorMessage = "*Senha obrigatório!")]
        [MaxLength(10, ErrorMessage = "A senha não pode ultrapaçar 10 caracteres!"), 
            MinLength(4, ErrorMessage = "A senha não pode conter menos que 4 caracteres!")]
        [Display(Name = "Nova senha (Entre 4 e 10 caracteres)")]
        [Column(TypeName = "varchar(128)")]
        public string Senha { get; set; }
    }
}