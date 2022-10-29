using Openvidu.Net.Core;
using Openvidu.Net.DTOs;
using Openvidu.Net.Enums;
using Openvidu.Net.Options;

namespace Openvidu.Net.Test
{
    internal class Program
    {
        private const string host = "https://vr.atreya.co";
        private const string username = "OPENVIDUAPP";
        private const string secret = "OmidOmid";

        static readonly IOpenViduManager service;

        static Program()
        {
            service = new OpenViduManager(host, username, secret);
        }


        static void Main(string[] args)
        {
            var session = "test_init_session";

            Test_Session_InitializeSession(session);

            Test_Session_RetrieveAllSessions(session);



            Test_Session_InitializeSession(session);
            Test_Connection_InitializeConnection(session);

            var key = Test_Recording_StartRecordingSession(session);

            Test_Recording_StopRecordingSession(session);

            Test_Session_CloseSession(session);

            Console.ReadLine();
        }

        private static void Test_Recording_StopRecordingSession(string key)
        {
            service.StopRecordingSession(key);
        }

        private static string Test_Recording_StartRecordingSession(string session)
        {
            var key = $"rec_{session}";
            var rec = service.StartRecordingSession(session, name: key);
            return rec.Id;
        }

        private static bool Test_Session_RetrieveSession(string testInitSession)
        {
            var session = service.RetrieveSession(testInitSession);

            if (session != null && session.Id == testInitSession)
                return true;
            return false;
        }

        static void Test_Session_InitializeSession(string sessionId)
        {

            Console.WriteLine("check session exist.");

            if (Test_Session_RetrieveSession(sessionId))
                return;

            Console.WriteLine("session not found.");

            var session = service.InitializeSession(new Session()
            {
                MediaMode = MediaMode.ROUTED,
                RecordingMode = RecordingMode.MANUAL,
                CustomSessionId = "test_init_session",
                ForcedVideoCodec = VideoCodec.VP8,
                AllowTranscoding = false,
                DefaultRecordingProperties = new RecordingProperties()
                {
                    Name = "MyRecording",
                    HasAudio = true,
                    HasVideo = true,
                    OutputMode = RecordingOutputMode.COMPOSED,
                    RecordingLayout = "BEST_FIT",
                    Resolution = "1280x720",
                    FrameRate = 30,
                    ShmSize = 536870912,

                },
                MediaNodeId = "123"
            });

            Console.WriteLine("session created");

        }

        static void Test_Session_RetrieveAllSessions(string sessionId)
        {
            var data = service.RetrieveAllSessions();
            if (data.Any(a => a.CustomSessionId == sessionId))
                Console.WriteLine($"Session {sessionId} creating is complete.");
        }

        static void Test_Session_CloseSession(string sessionId)
        {
            Console.WriteLine($"Session {sessionId} is closed.");
            service.CloseSession(sessionId);
        }

        static void Test_Connection_InitializeConnection(string sessionId)
        {

            var con = service.RetrieveAllConnections(sessionId);

            if (con.Any(a => a.ServerData == "User-1"))
                return;

            var Connection = service.InitializeConnection(sessionId, new Connection()
            {
                Type = ConnectionType.WEBRTC,
                Data = "User-1",
                ClientData = "Client data test",
                Record = true,
                Role = "PUBLISHER",
                KurentoOptions = new KurentoOptions()
                {
                    AllowedFilters = new[]
                    {
                        "GStreamerFilter" , "ZBarFilter"
                    }.ToArray(),
                    VideoMaxRecvBandwidth = 1000,
                    VideoMinRecvBandwidth = 300,
                    VideoMaxSendBandwidth = 1000,
                    VideoMinSendBandwidth = 300,
                },
            });


        }
    }
}