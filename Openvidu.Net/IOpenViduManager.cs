using System.Collections.Generic;
using Openvidu.Net.DTOs;
using Openvidu.Net.Enums;

namespace Openvidu.Net
{
    public interface IOpenViduManager
    {
        Session InitializeSession(Session session);
        object InitializeSession();
        Session RetrieveSession(string sessionId);
        List<Session> RetrieveAllSessions();
        bool CloseSession(string sessionId);
        Connection InitializeConnection(string sessionId, Connection connection);
        Connection RetrieveConnection(string sessionId, string connectionId);
        List<Connection> RetrieveAllConnections(string sessionId);
        Connection ModifyConnection(string sessionId, string connectionId, string role, bool record);
        bool CloseConnections(string sessionId, string connectionId);
        Recording StartRecordingSession(string sessionId, RecordingProperties properties);
        Recording StartRecordingSession(string sessionId, string name = "");
        void StopRecordingSession(string recordingId);
        Recording RetrieveRecording(string recordingId);
        List<Recording> RetrieveAllRecording();
        void DeleteRecording(string recordingId);
        MediaNode RetrieveMediaNode(string mediNodeId, bool sessions = false, bool recordings = false, bool extraInfo = false);
        List<MediaNode> RetrieveAllMediaNodes(bool sessions = false, bool recordings = false, bool extraInfo = false);
        MediaNode AddMediaNode(MediaNodeOnPremises node, bool wait = false);
        MediaNode AddMediaNode(MediaNodeAws node, bool wait = false);
        void RemoveMediaNode(string mediNodeId, MediaNodeStrategy deletionStrategy = MediaNodeStrategy.if_no_sessions, bool wait = false);
        MediaNode ModifyMediaNode(string mediNodeId, MediaNodeStatus status);
        List<MediaNode> AutoDiscoverMediaNodes();
        void UnpublishStreamConnection();
        void SendSignalSession();
        void GetServerActiveConfiguration();
        void CheckServerHealth();
        void RestartMediaServer();
    }
}