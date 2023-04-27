using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoTarefas.Interfaces
{
    public interface IEnviarEmail
    {
        Task Email(string email, string body);
        string GerarCodigo();
    }
}