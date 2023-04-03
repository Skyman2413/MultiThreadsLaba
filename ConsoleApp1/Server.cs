
using System.Net;
using System.Net.Sockets;

namespace ConsoleApp1;

public class Server
{
    
    public void Start_working()
    {
        var tcpListener = new TcpListener(IPAddress.Any, 8088);
        try
        {
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}