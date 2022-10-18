using Openvidu.Net;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var ov = new OpenVidu("https://medias.shooka.com:443", "MY_SECRET");
            var session = ov.CreateSession();

            ov.Fetch();
            ov.StartRecording(session.Id, "11");
            var lsi = ov.GetActiveSessions();

            Console.WriteLine("Hello, World!");
        }
    }
}