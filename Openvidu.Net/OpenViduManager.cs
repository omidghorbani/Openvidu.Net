using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Openvidu.Net.DTOs;
using Openvidu.Net.Exceptions;
using RestSharp;
using RestSharp.Authenticators;

namespace Openvidu.Net
{
    public class OpenViduManager : IOpenViduManager
    {

        private string secret;
        protected string hostname;
        protected RestClient httpClient;
        protected Dictionary<string, Session> activeSessions = new Dictionary<string, Session>();
        protected Dictionary<string, Recording> activeRecordings = new Dictionary<string, Recording>();

        protected const string API_PATH = "openvidu/api";
        protected const string API_SESSIONS = API_PATH + "/sessions";
        protected const string API_TOKENS = API_PATH + "/tokens";
        protected const string API_RECORDINGS = API_PATH + "/recordings";
        protected const string API_RECORDINGS_START = API_RECORDINGS + "/start";
        protected const string API_RECORDINGS_STOP = API_RECORDINGS + "/stop";
        protected const string API_CONNECTION = "/connection";

        
        public OpenViduManager(string hostname, string secret)
        {
            this.hostname = hostname;
            this.secret = secret;

            if (!this.hostname.EndsWith("/"))
            {
                this.hostname += "/";
            }

            this.httpClient = new RestClient(hostname);

            httpClient.Authenticator = new HttpBasicAuthenticator("OPENVIDUAPP", secret);

        }

        #region [ Sessions ]

        public Session CreateSession()
        {

            return CreateSession(new Session());

        }

        public Session CreateSession(Session session)
        {
            var request = new RestRequest(API_SESSIONS, Method.Post);
            var body = JsonSerializer.Serialize(session);

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new OpenViduHttpException(response.StatusCode.ToString());

            if (response.Content != null)
                session = JsonSerializer.Deserialize<Session>(response.Content);

            FetchSessions();

            return session;
        }

        public List<Session> GetActiveSessions()
        {
            return activeSessions.Values.ToList();
        }

        public Session GetActiveSession(string sessionId)
        {
            if (activeSessions.ContainsKey(sessionId))
                return activeSessions[sessionId];
            throw new OpenViduClientException($"the session {sessionId} not found");
        }

        public bool FetchSessions()
        {
            try
            {
                var request = new RestRequest(API_SESSIONS, Method.Get);

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
                {
                    activeSessions.Add(session.Id, session);
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
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

        public Connection CreateConnection(string sessionId, string metadata = "")
        {
            throw new NotImplementedException();
        }

        public Connection RetrieveConnection(string sessionId, string connectionId)
        {
            throw new NotImplementedException();
        }

        public List<Connection> RetrieveAllConnections(string sessionId)
        {
            throw new NotImplementedException();
        }

        public Connection ModifyConnection(string sessionId, string connectionId, string role, bool record)
        {
            //{
            //    "role": "PUBLISHER",
            //    "record": true
            //}
            throw new NotImplementedException();
        }

        public bool CloseConnections(string sessionId, string connectionId)
        {
            throw new NotImplementedException();
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

        public void RetrieveAllRecording()
        {
            var request = new RestRequest(API_RECORDINGS, Method.Get);
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

        public void RetrieveMediaNode() { throw new NotImplementedException(); }
        public void RetrieveAllMediaNodes() { throw new NotImplementedException(); }
        public void AddMediaNode() { throw new NotImplementedException(); }
        public void RemoveMediaNode() { throw new NotImplementedException(); }
        public void ModifyMediaNode() { throw new NotImplementedException(); }
        public void AutoDiscoverMediaNodes() { throw new NotImplementedException(); }

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