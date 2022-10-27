using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Openvidu.Net.DTOs;
using Openvidu.Net.Enums;
using Openvidu.Net.Exceptions;
using RestSharp;
using RestSharp.Authenticators;

namespace Openvidu.Net
{
    public class OpenViduManager : IOpenViduManager
    {

        private string secret;
        private string username = "OPENVIDUAPP";
        private string hostname;
        protected RestClient httpClient;
        protected Dictionary<string, Session> activeSessions = new Dictionary<string, Session>();
        protected Dictionary<string, Recording> activeRecordings = new Dictionary<string, Recording>();

        protected const string API_PATH = "openvidu/api";
        protected const string API_SESSIONS = API_PATH + "/sessions";
        protected const string API_TOKENS = API_PATH + "/tokens";
        protected const string API_RECORDINGS = API_PATH + "/recordings";
        protected const string API_RECORDINGS_START = API_RECORDINGS + "/start";
        protected const string API_RECORDINGS_STOP = API_RECORDINGS + "/stop";
        protected const string API_CONNECTION = "connection";
        protected const string API_MEDIANODE = API_PATH + "/media-nodes";


        private readonly IConfigurationRoot _appSetting;

        public OpenViduManager(IConfigurationRoot appSetting)
        {
            hostname = appSetting.GetSection("Router:Hostname").Value;
            username = appSetting.GetSection("Router:Username").Value;
            secret = appSetting.GetSection("Router:Secret").Value;
            _appSetting = appSetting;

            if (!hostname.EndsWith("/"))
                hostname += "/";

            httpClient = new RestClient(hostname);

            httpClient.Authenticator = new HttpBasicAuthenticator(username, secret);

        }

        #region [ Sessions ]

        public Session InitializeSession(Session session)
        {
            var request = new RestRequest(API_SESSIONS, Method.Post);
            var body = JsonSerializer.Serialize(session ?? new Session());

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new OpenViduHttpException(response.StatusCode.ToString());

            if (response.Content != null)
                session = JsonSerializer.Deserialize<Session>(response.Content);

            RetrieveAllSessions();

            return session;
        }

        public object InitializeSession() => InitializeSession(new Session());

        public Session RetrieveSession(string sessionId)
        {
            var request = new RestRequest($"{API_SESSIONS}/{sessionId}");

            request.AddHeader("Content-Type", "application/json");

            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new OpenViduHttpException(response.StatusCode.ToString());
            // else
            //throw new OpenViduClientException($"the session {sessionId} not found");

            var json = JsonNode.Parse(response.Content ?? string.Empty);
            var data = json["content"].ToString();
            var session = JsonSerializer.Deserialize<Session>(data);

            return session;
        }

        public List<Session> RetrieveAllSessions()
        {
            var request = new RestRequest(API_SESSIONS);

            request.AddHeader("Content-Type", "application/json");

            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new OpenViduHttpException(response.StatusCode.ToString());

            var json = JsonNode.Parse(response.Content ?? string.Empty);
            var data = json["content"].ToString();
            var list = JsonSerializer.Deserialize<Session[]>(data);

            if (activeSessions.Any())
                activeSessions.Clear();

            foreach (var session in list)
                activeSessions.Add(session.Id, session);

            return list.ToList();

        }

        public bool CloseSession(string sessionId)
        {
            try
            {
                var request = new RestRequest($"{API_SESSIONS}/{sessionId}", Method.Delete);

                request.AddHeader("Content-Type", "application/json");

                RestResponse response = httpClient.Execute(request);

                if (response.StatusCode != HttpStatusCode.NoContent)
                    throw new OpenViduHttpException(response.StatusCode.ToString());

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region [ Connections ]

        public Connection InitializeConnection(string sessionId, Connection connection)
        {
            var request = new RestRequest($"{API_SESSIONS}/{sessionId}/{API_CONNECTION}", Method.Post);
            var body = JsonSerializer.Serialize(connection);

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new OpenViduHttpException(response.StatusCode.ToString());

            var json = JsonNode.Parse(response.Content ?? string.Empty);
            var data = json["content"].ToString();
            connection = JsonSerializer.Deserialize<Connection>(data);
            return connection;
        }

        public Connection RetrieveConnection(string sessionId, string connectionId)
        {
            var request = new RestRequest($"{API_SESSIONS}/{sessionId}/{API_CONNECTION}/{connectionId}");
            request.AddHeader("Content-Type", "application/json");

            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new OpenViduHttpException(response.StatusCode.ToString());
            // else
            //throw new OpenViduClientException($"the session {sessionId} not found");

            var json = JsonNode.Parse(response.Content ?? string.Empty);
            var data = json["content"].ToString();
            var connection = JsonSerializer.Deserialize<Connection>(data);

            return connection;
        }

        public List<Connection> RetrieveAllConnections(string sessionId)
        {
            //openvidu/api/sessions/SESSION_ID/connection 
            var request = new RestRequest($"{API_SESSIONS}/{sessionId}/{API_CONNECTION}");

            request.AddHeader("Content-Type", "application/json");

            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new OpenViduHttpException(response.StatusCode.ToString());

            var json = JsonNode.Parse(response.Content ?? string.Empty);
            var data = json["content"].ToString();
            var list = JsonSerializer.Deserialize<Connection[]>(data);

            return list.ToList();
        }

        public Connection ModifyConnection(string sessionId, string connectionId, string role, bool record)
        {
            // /openvidu/api/sessions/SESSION_ID/connection/CONNECTION_ID
            //{
            //    "role": "PUBLISHER",
            //    "record": true
            //}
            var connection = RetrieveConnection(sessionId, connectionId);
            var request = new RestRequest($"{API_SESSIONS}/{connection.SessionId}/{API_CONNECTION}/{connection.Id}", Method.Post);
            var body = JsonSerializer.Serialize(new
            {
                role = role,
                record = record
            });

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new OpenViduHttpException(response.StatusCode.ToString());

            var json = JsonNode.Parse(response.Content ?? string.Empty);
            var data = json["content"].ToString();
            connection = JsonSerializer.Deserialize<Connection>(data);
            return connection;
        }

        public bool CloseConnections(string sessionId, string connectionId)
        {
            try
            {
                var request = new RestRequest($"{API_SESSIONS}/{sessionId}/{API_CONNECTION}/{connectionId}", Method.Delete);

                request.AddHeader("Content-Type", "application/json");

                RestResponse response = httpClient.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NoContent:
                        return true;
                    case HttpStatusCode.BadRequest:
                        throw new OpenViduException($"No Session exists for the passed {sessionId}");
                    case HttpStatusCode.NotFound:
                        throw new OpenViduException($"No Connection for the passed {connectionId}");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region [ Recordings ]

        public void StartRecordingSession(string sessionId, RecordingProperties properties)
        {

            var request = new RestRequest(API_RECORDINGS_START, Method.Post);

            request.AddHeader("Content-Type", "application/json");

            var body = JsonNode.Parse(properties.ToJson())?.AsObject();

            body.Add("session", sessionId);

            request.AddParameter("application/json", body.ToJsonString(), ParameterType.RequestBody);

            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
                RetrieveAllRecording();
            else
                throw new OpenViduHttpException(response.StatusCode.ToString());


        }

        public void StartRecordingSession(string sessionId, string name = "")
        {
            StartRecordingSession(sessionId, new RecordingProperties()
            {
                Name = name
            });
        }

        public void StopRecordingSession(string recordingId)
        {
            var recording = RetrieveRecording(recordingId);

            var request = new RestRequest($"{API_RECORDINGS_STOP}/{recordingId}", Method.Post);

            request.AddHeader("Content-Type", "application/json");

            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
                RetrieveAllRecording();
            else
                throw new OpenViduHttpException(response.StatusCode.ToString());

        }

        public Recording RetrieveRecording(string recordingId)
        {
            foreach (var recording in activeRecordings)
                if (recording.Value.Id == recordingId)
                    return recording.Value;

            throw new OpenViduException($"No recording exists for the passed {recordingId}.");
        }

        public List<Recording> RetrieveAllRecording()
        {
            var request = new RestRequest(API_RECORDINGS);
            request.AddHeader("Content-Type", "application/json");

            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new OpenViduHttpException(response.StatusCode.ToString());

            var json = JsonNode.Parse(response.Content ?? string.Empty);
            var data = json?["items"]?.ToString();
            var list = JsonSerializer.Deserialize<Recording[]>(data).ToList();

            if (activeRecordings.Any())
                activeRecordings.Clear();

            foreach (var recording in list)
                activeRecordings.Add(recording.SessionId, recording);

            return list.ToList();
        }

        public void DeleteRecording(string recordingId)
        {
            var request = new RestRequest($"{API_RECORDINGS}/{recordingId}", Method.Delete);
            request.AddHeader("Content-Type", "application/json");

            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode != HttpStatusCode.NoContent)
                throw new OpenViduHttpException(response.StatusCode.ToString());

        }

        #endregion

        #region [ MediaNode ]

        public MediaNode RetrieveMediaNode(string mediNodeId, bool sessions = false, bool recordings = false, bool extraInfo = false)
        {
            var request = new RestRequest($"{API_MEDIANODE}/{mediNodeId}");

            request.AddHeader("Content-Type", "application/json");
            request.AddQueryParameter("sessions", sessions);
            request.AddQueryParameter("recordings", recordings);
            request.AddQueryParameter("extra-info", extraInfo);

            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new OpenViduHttpException($"No Media Node exists for the passed {mediNodeId}");

            if (response.StatusCode != HttpStatusCode.OK)
                throw new OpenViduHttpException(response.StatusCode.ToString());

            var json = JsonNode.Parse(response.Content ?? string.Empty);
            var result = json.Deserialize<MediaNode>();

            return result;

        }

        public List<MediaNode> RetrieveAllMediaNodes(bool sessions = false, bool recordings = false, bool extraInfo = false)
        {
            var request = new RestRequest($"{API_MEDIANODE}");

            request.AddHeader("Content-Type", "application/json");
            request.AddQueryParameter("sessions", sessions);
            request.AddQueryParameter("recordings", recordings);
            request.AddQueryParameter("extra-info", extraInfo);

            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new OpenViduHttpException(response.StatusCode.ToString());

            var json = JsonNode.Parse(response.Content ?? string.Empty);
            var list = json["content"].Deserialize<MediaNode[]>();

            return list.ToList();
        }

        private MediaNode addMediaNode(object node, bool wait = false)
        {
            var request = new RestRequest($"{API_MEDIANODE}", Method.Post);
            var body = JsonSerializer.Serialize(node);

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode == (HttpStatusCode)400)
                throw new OpenViduHttpException("Problem with some body parameter");
            if (response.StatusCode == (HttpStatusCode)404)
                throw new OpenViduHttpException("The Media Node is not within reach of OpenVidu Server. This simply means that OpenVidu cannot establish a connection with it. This may be caused by multiple reasons: wrong IP, port or path, a network problem, too strict a proxy configuration...");
            if (response.StatusCode == (HttpStatusCode)405)
                throw new OpenViduHttpException("For OpenVidu Enterprise HA clusters this method is not allowed");
            if (response.StatusCode == (HttpStatusCode)409)
                throw new OpenViduHttpException("The Media Node was already registered in OpenVidu Server as part of the cluster");
            if (response.StatusCode == (HttpStatusCode)501)
                throw new OpenViduHttpException("The cluster is deployed On Premises and no uri parameter was passed in the body request");
            if (response.StatusCode == (HttpStatusCode)502)
                throw new OpenViduHttpException("The process of launching a new Media Node instance failed. This won't ever happen for On Premises deployments, where instances require to be previously launched");

            if (response.StatusCode != HttpStatusCode.OK)
                throw new OpenViduHttpException(response.StatusCode.ToString());

            var json = JsonNode.Parse(response.Content ?? string.Empty);
            var data = json["content"].ToString();
            var result = JsonSerializer.Deserialize<MediaNode>(data);
            return result;
        }
        public MediaNode AddMediaNode(MediaNodeOnPremises node, bool wait = false)
        {
            return addMediaNode(node);
        }
        public MediaNode AddMediaNode(MediaNodeAws node, bool wait = false)
        {
            return addMediaNode(node);
        }

        public void RemoveMediaNode(string mediNodeId, MediaNodeStrategy deletionStrategy = MediaNodeStrategy.if_no_sessions , bool wait = false)
        {

            var request = new RestRequest($"{API_MEDIANODE}/{mediNodeId}", Method.Delete);

            request.AddHeader("Content-Type", "application/json");
            request.AddQueryParameter("wait",wait);
            request.AddQueryParameter("deletion‑strategy", deletionStrategy.ToString());

            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode == (HttpStatusCode)202)
                throw new OpenViduHttpException($"If query parameter deletion-strategy is set to when-no-sessions, then it means that the Media Node to be deleted has ongoing sessions inside of it. Media Node status has been set to waiting-idle-to-terminate");
            if (response.StatusCode == (HttpStatusCode)204)
                return;
            //    throw new OpenViduHttpException($"The Media Node was successfully removed");
            if (response.StatusCode == (HttpStatusCode)404)
                throw new OpenViduHttpException($"No Media Node exists for the passed {mediNodeId}");
            if (response.StatusCode == (HttpStatusCode)405)
                throw new OpenViduHttpException($"For OpenVidu Enterprise HA clusters this method is not allowed");
            if (response.StatusCode == (HttpStatusCode)409)
                throw new OpenViduHttpException($"If query parameter deletion-strategy is set to if-no-sessions, then it means that the Media Node to be deleted has ongoing sessions inside of it. No Media Node deletion will take place at all");
            if (response.StatusCode == (HttpStatusCode)502)
                throw new OpenViduHttpException($"Error while terminating the Media Node instance. This won't ever happen for On Premises deployments, where instances require manual shut down");

            if (response.StatusCode != HttpStatusCode.OK)
                throw new OpenViduHttpException(response.StatusCode.ToString());

        }

        public MediaNode ModifyMediaNode(string mediNodeId,MediaNodeStatus status)
        {
            var request = new RestRequest($"{API_MEDIANODE}/{mediNodeId}", Method.Patch);

            request.AddHeader("Content-Type", "application/json");
            request.AddBody(JsonSerializer.Serialize(new
            {
                status = status.ToString()
            }));

            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode == (HttpStatusCode)204)
                throw new OpenViduHttpException($"The Media Node has not been modified because its status was the same as the provided through body parameters");
            if (response.StatusCode == (HttpStatusCode)400)
                throw new OpenViduHttpException($"Problem with some body parameter. This means the Media Node cannot transition from its current status to the indicated one in the status request body parameter");
            if (response.StatusCode == (HttpStatusCode)404)
                throw new OpenViduHttpException($"No Media Node exists for the passed MEDIA_NODE_ID");
            if (response.StatusCode == (HttpStatusCode)405)
                throw new OpenViduHttpException($"For OpenVidu Enterprise HA clusters this method is not allowed");


            if (response.StatusCode != HttpStatusCode.OK)
                throw new OpenViduHttpException(response.StatusCode.ToString());

            var json = JsonNode.Parse(response.Content ?? string.Empty);
            var result = json.Deserialize<MediaNode>();

            return result;
        }

        public List<MediaNode> AutoDiscoverMediaNodes()
        {
            var request = new RestRequest($"{API_MEDIANODE}",Method.Put);

            request.AddHeader("Content-Type", "application/json");


            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode != (HttpStatusCode)405)
                throw new OpenViduHttpException("For OpenVidu Enterprise HA clusters this method is not allowed");
            
            var json = JsonNode.Parse(response.Content ?? string.Empty);
            var list = json["content"].Deserialize<MediaNode[]>();

            return list.ToList();
        }

        #endregion

        #region [ Other ]

        public void UnpublishStreamConnection() { throw new NotImplementedException(); }
        public void SendSignalSession() { throw new NotImplementedException(); }
        public void GetServerActiveConfiguration() { throw new NotImplementedException(); }
        public void CheckServerHealth() { throw new NotImplementedException(); }
        public void RestartMediaServer() { throw new NotImplementedException(); }

        #endregion


    }
}