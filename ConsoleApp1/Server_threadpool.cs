using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ConsoleApp1;

public class Server
{
    private HttpListener _listener;
    public Server() {
        // Создаем листенер и конечные адреса для запросов
        _listener = new HttpListener();
        // такой запрос должен быть POST (см REST API), т.к. он изменяет ресурс сервера
        _listener.Prefixes.Add("http://127.0.0.1:8080/new_item/");
        // такой запрос должен быть GET (см REST API), т.к. он получает ресурс сервера
        _listener.Prefixes.Add("http://127.0.0.1:8080/current_state/");

    }

    public void Start_working()
    {
        _listener.Start();
        try
        {
            while (true)
            {
                // каждый запрос выполняется в отдельном потоке (почему это плохо, вопрос для студентов?)
                ThreadPool.QueueUserWorkItem(Process, _listener.GetContext());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _listener.Stop();
            _listener.Close();
        }
    }
    
    private void Process(object o) {
        var context = o as HttpListenerContext;
        // логика обработки запроса
        var request = context.Request;
        var endpoint = request.RawUrl;
        var response = context.Response;
        var encoding = response.ContentEncoding ?? Encoding.UTF8;
        response.KeepAlive = false;
        var responseData = "";
        
        // так же есть более симпатичная реализация с маппингом, но она сложна для чтения и понимая,
        // тут стараюсь использовать как можно более базовые вещи. Так писать не стоит
        switch (endpoint)
        {
            case "/new_item/":
            {
                // для создания новой задачи
                try
                {
                    CreateNewItem();
                }
                catch
                {
                    response.StatusCode = 400;
                    response.StatusDescription = "Bad Request";
                }

                break;
            }
            case "/current_state/":
            {
                //TODO для получения данных об очереди
                break;
            }
            default:
            {
                //TODO
                break;
            }
        }

        byte[] buffer = Encoding.UTF8.GetBytes(responseData);
        // получаем поток ответа и пишем в него ответ
        response.ContentLength64 = buffer.Length;
        using (var output = response.OutputStream)
        {
            output.Write(buffer);
        }
        

        void CreateNewItem()
        {
            // создание нового элемента очереди
            if (request.HttpMethod.ToLower() != "post")
            {
                response.StatusCode = 405;
                response.StatusDescription = "Method is not allowed";
                return;
            }

            if (request.ContentType is not ("application/json" or "text/json"))
            {
                response.StatusCode = 415;
                response.StatusDescription = "Unsupported Media Type";
                return;
            }

            var job = GetDataFromRequest();
            if (job is not null) Program.ServerQueue.Add(job);
            else
            {
                response.StatusCode = 500;
                response.StatusDescription = "Internal Server Error";
            }
        }

        Job? GetDataFromRequest()
        {
            // преобразовываем инфо из запроса в Job-объект
            var res = "";
            using (var receiveStream = request.InputStream)
            {
                using (var readStream = new StreamReader(receiveStream, encoding))
                {
                    res = readStream.ReadToEnd();
                }
            }
            
            return JsonSerializer.Deserialize<Job>(res);
        }
            
    
    }
    

}