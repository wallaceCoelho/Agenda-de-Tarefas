using System.Security.Cryptography;
using System.Text;

namespace ProjetoTarefas.Services
{
    public class HashPassword 
    {
        private HashAlgorithm _algoritmo;
        public HashPassword(HashAlgorithm algoritmo)
        {
            _algoritmo = algoritmo;
        }
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
    }
}