using System;
using System.Net;
using System.Reflection;

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
            while (true)
            {
                ManipularRequisicao();
            }
        }

        private void ManipularRequisicao()
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

            var path = requisicao.Url.AbsolutePath;

            var assembly = Assembly.GetExecutingAssembly();
            var nameResource = Utilidades.ConverterPathParaNomeAssembly(path);

            var resourceStream = assembly.GetManifestResourceStream(nameResource);
            var bytesResource = new byte[resourceStream.Length];

            resourceStream.Read(bytesResource, 0, bytesResource.Length);

            resposta.ContentType = Utilidades.ObterTipoDeConteudo(path);
            resposta.StatusCode = 200;
            resposta.ContentLength64 = resourceStream.Length;

            resposta.OutputStream.Write(bytesResource, 0, bytesResource.Length);

            resposta.OutputStream.Close();

            httpListiner.Stop();
        }
    }
}
