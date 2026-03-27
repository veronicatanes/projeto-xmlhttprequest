using System;
using System.ComponentModel.Design;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {

        const bool webRequestExemplo = true;

        if (webRequestExemplo)
        {

            using (var client = new HttpClient())
            {
                // 1. open() - Abrir uma solicitação HTTP personalizada
                var request = new HttpRequestMessage(HttpMethod.Get, "https://fakestoreapi.com/products");

                // 4. setRequestHeader() - Definir o valor de um cabeçalho de solicitação HTTP personalizado
                request.Headers.Add("User-Agent", "MyCustomApp/1.0");

                // 2. send() - Enviar a solicitação HTTP para o servidor
                var cancellationTokenSource = new CancellationTokenSource();
                try
                {
                    var response = await client.SendAsync(request, cancellationTokenSource.Token);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Resposta da solicitação:");
                        Console.WriteLine(content);
                    }
                    else
                    {
                        Console.WriteLine($"Erro na solicitação: {response.StatusCode}");
                    }
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Solicitação cancelada.");
                }
            }
        } else 
        {
            try
            {
                // 1. Criar o WebRequest (equivalente ao open())
                WebRequest request = WebRequest.Create("https://jsonplaceholder.typicode.com/todos/");

                // 2. Definir método HTTP (GET, POST, etc.)
                request.Method = "GET";

                // 3. Definir cabeçalhos (equivalente ao setRequestHeader())
                request.Headers.Add("User-Agent", "MyCustomApp/1.0");

                // 4. Enviar a requisição e obter resposta (equivalente ao send())
                using (WebResponse response = request.GetResponse())
                {
                    // Verificar o status da resposta
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine($"Status: {httpResponse.StatusCode}");

                    // 5. Ler o conteúdo da resposta
                    using (Stream dataStream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        string responseFromServer = reader.ReadToEnd();
                        Console.WriteLine("Resposta da solicitação:");
                        Console.WriteLine(responseFromServer);
                    }
                }
            }
            catch (WebException e)
            {
                Console.WriteLine($"Erro na solicitação: {e.Status}");
                using (var stream = e.Response?.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
        }
    }
}
 
