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
using Openvidu.Net.Exceptions;
using RestSharp;
using RestSharp.Authenticators;

namespace Openvidu.Net
{
    public class OpenViduManager : IOpenViduManager
    {

        private string secret;
        private string username= "OPENVIDUAPP";
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
        private readonly IConfigurationRoot _appSetting;

        public OpenViduManager( IConfigurationRoot appSetting)
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
            var request = new RestRequest($"{API_SESSIONS}/{sessionId}/{API_CONNECTION}", Method.Get);

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