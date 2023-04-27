using ProjetoTarefas.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ProjetoTarefas.Services
{
    public class EnviarEmail : IEnviarEmail
    {
        public async Task Email(string email, string body)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("wacoelho.dev@gmail.com"),
                Subject = "Prodigy - Confirmação de e-mail.",
                PlainTextContent = "and easy to do anywhere, even with C#",
                HtmlContent = body
            };
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
        }

        public string GerarCodigo()
        {
            Random randNum = new Random();
            int codigo = 0;
            
            for (int i = 0; i <= 5; i++)
                codigo += randNum.Next(1, 9);

            return codigo.ToString();
        }
    }
}