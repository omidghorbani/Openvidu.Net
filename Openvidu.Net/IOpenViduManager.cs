using System.Collections.Generic;
using Openvidu.Net.DTOs;

namespace Openvidu.Net
{
    public interface IOpenViduManager
    {
        Session CreateSession();
        Session CreateSession(Session session);
        List<Session> GetActiveSessions();
        Session GetActiveSession(string sessionId);
        bool FetchSessions();
        bool CloseSession(string sessionId);
        Connection CreateConnection(string sessionId, string metadata = "");
        Connection RetrieveConnection(string sessionId, string connectionId);
        List<Connection> RetrieveAllConnections(string sessionId);
        Connection ModifyConnection(string sessionId, string connectionId, string role, bool record);
        bool CloseConnections(string sessionId, string connectionId);
        void StartRecordingSession(string sessionId, RecordingProperties properties);
        void StartRecordingSession(string sessionId, string name = "");
        void StopRecordingSession(string recordingId);
        Recording RetrieveRecording(string recordingId);
        void RetrieveAllRecording();
        void DeleteRecording(string recordingId);
        void RetrieveMediaNode();
        void RetrieveAllMediaNodes();
        void AddMediaNode();
        void RemoveMediaNode();
        void ModifyMediaNode();
        void AutoDiscoverMediaNodes();
        void UnpublishStreamConnection();
        void SendSignalSession();
        void GetServerActiveConfiguration();
        void CheckServerHealth();
        void RestartMediaServer();
    }
}