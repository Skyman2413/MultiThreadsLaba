using System.Collections.Concurrent;
using ConsoleApp1;

class Program
{
    public static  BlockingCollection<Dictionary<string, string>> ServerQueue = new ();
    public static Dictionary<string, string> JobTemplate = new()
    {
        {"org_name", "sample_name"},
        {"count_price", "199234"},
        {"date", "12.04.2023"},
        {"product_list", "productA, productB"}
    };
    private static void worker()
    {

    }

    public static void Main(string[] args)
    {
        var serv = new Server();
        var server = new Thread(serv.Start_working);
        // TODO сделать тредпул для воркеров
        const int threadsCount = 8;
        Thread[] consumers = new Thread[threadsCount];

        for (int i = 0; i < threadsCount; i++)
        {
            consumers[i] = new Thread(worker);
            consumers[i].Start();
        }

        server.Start();
        server.Join();
        for (int i = 0; i < threadsCount; i++)
        {
            consumers[i].Join();
        }
    }
}


