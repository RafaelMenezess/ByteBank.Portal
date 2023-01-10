using System;
using System.Net;
using System.Text;

namespace ByteBank.Portal.Infraestrutura
{
    public class WebApplication
    {
        private readonly string[] _prefixos;
        public WebApplication(string[] prefixos)
        {
            if (prefixos == null)
                throw new ArgumentNullException(nameof(prefixos));

            _prefixos = prefixos;
        }
        public void Iniciar()
        {
            var httpListiner = new HttpListener();

            foreach (var prefixo in _prefixos)
            {
                httpListiner.Prefixes.Add(prefixo);
            }

            httpListiner.Start();

            var contexto = httpListiner.GetContext();
            var requisicao = contexto.Request;
            var resposta = contexto.Response;

            var respostaConteudo = "Hello World";
            var respostaConteudoBytes = Encoding.UTF8.GetBytes(respostaConteudo);

            resposta.ContentType = "text/html; charset=utf-8";
            resposta.StatusCode = 200;
            resposta.ContentLength64 = respostaConteudoBytes.Length;

            resposta.OutputStream.Write(respostaConteudoBytes, 0, respostaConteudoBytes.Length);

            resposta.OutputStream.Close();

            httpListiner.Stop();

        }
    }
}
