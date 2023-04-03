using System.Collections.Concurrent;
using ConsoleApp1;


class Job
{
    public string OrgName { get; set; }
    public long Price { get; set; }
    public string Date { get; set; }
    public List<string>? ProductList { get; set; }
    public string State { get; set; }
    public string WorkerInfo { get; set; }
    
}
class Program
{
    public static BlockingCollection<Job> ServerQueue = new ();
    private static void worker()
    {

    }

    public static void Main(string[] args)
    {
        var serv = new Server();
        var server = new Thread(serv.Start_working);
        // TODO сделать тредпул для воркеров
        const int threadsCount = 8;
        //Thread[] consumers = new Thread[threadsCount];

        /*for (int i = 0; i < threadsCount; i++)
        {
            consumers[i] = new Thread(worker);
            consumers[i].Start();
        }*/

        server.Start();
        server.Join();
        /*for (int i = 0; i < threadsCount; i++)
        {
            consumers[i].Join();
        }
        */
    }
}


