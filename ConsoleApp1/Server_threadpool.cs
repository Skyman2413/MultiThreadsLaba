
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApp1;

public class Server
{
    private HttpListener _listener;
    public Server() {
        // Создаем листенер и конечные адреса для запросов
        _listener = new HttpListener();
        _listener.Prefixes.Add("http://127.0.0.1:8080/new_item/");
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
        
    }

}