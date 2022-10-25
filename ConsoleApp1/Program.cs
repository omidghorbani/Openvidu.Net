using Openvidu.Net;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var ov = new OpenViduManager("https://vr.atreya.co", 
                "MY_SECRET");
            var session = ov.CreateSession();

            ov.FetchSessions();
            ov.StartRecordingSession(session.Id, "11");
            var lsi = ov.GetActiveSessions();

            Console.WriteLine("Hello, World!");
        }
    }
}