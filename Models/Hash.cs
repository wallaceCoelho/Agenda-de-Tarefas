using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoTarefas.Models
{
    public class Hash
    {
        private HashAlgorithm _algoritmo;

        public Hash(HashAlgorithm algoritmo) => _algoritmo = algoritmo;

        public string CriptografarSenha(string senha)
        {
            var valorHash = Encoding.UTF8.GetBytes(senha);
            var criptografiaSenha = _algoritmo.ComputeHash(valorHash);

            var sb = new StringBuilder();

            foreach (var caractere in criptografiaSenha)
            {
                sb.Append(caractere.ToString("X2"));
            }

            return sb.ToString();
        }

        public bool VerificarSenha(string senhaDigitada, string senhaCadastrada)
        {
            if (string.IsNullOrEmpty(senhaCadastrada))
                throw new NullReferenceException("Cadastre uma senha.");

            var criptografiaSenha = _algoritmo.ComputeHash(Encoding.UTF8.GetBytes(senhaDigitada));

            var sb = new StringBuilder();
            foreach (var caractere in criptografiaSenha)
            {
                sb.Append(caractere.ToString("X2"));
            }

            return sb.ToString() == senhaCadastrada;
        }
    }
}
