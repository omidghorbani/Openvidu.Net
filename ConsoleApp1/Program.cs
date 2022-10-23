using Openvidu.Net;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var ov = new OpenVidu("https://medias.shooka.com:443", 
                "grkWXM6aYwQsPrikkW2EPl6pBgx3yszHWz0W842JTYvT38LNJbBYOwUH5McH6xhF");
            var session = ov.CreateSession();

            ov.Fetch();
            ov.StartRecording(session.Id, "11");
            var lsi = ov.GetActiveSessions();

            Console.WriteLine("Hello, World!");
        }
    }
}