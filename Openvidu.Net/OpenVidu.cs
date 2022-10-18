using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Openvidu.Net.Exceptions;
using RestSharp;
using RestSharp.Authenticators;

namespace Openvidu.Net
{
    public class OpenVidu
    {

        private String secret;
        protected String hostname;
        protected RestClient httpClient;
        protected Dictionary<String, Session> activeSessions = new Dictionary<string, Session>();

        protected const string API_PATH = "openvidu/api";
        protected const string API_SESSIONS = API_PATH + "/sessions";
        protected const string API_TOKENS = API_PATH + "/tokens";
        protected const string API_RECORDINGS = API_PATH + "/recordings";
        protected const string API_RECORDINGS_START = API_RECORDINGS + "/start";
        protected const string API_RECORDINGS_STOP = API_RECORDINGS + "/stop";


        public OpenVidu(String hostname, String secret)
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

        public Session CreateSession()
        {
            return CreateSession(new Session());

        }
        public Session CreateSession(Session session)
        {
            var request = new RestRequest(API_SESSIONS, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            var body = JsonSerializer.Serialize(session);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new OpenViduHttpException(response.StatusCode.ToString());

            if (response.Content != null)
                session = JsonSerializer.Deserialize<Session>(response.Content);

            return session;
        }
        public Recording StartRecording(String sessionId, RecordingProperties properties)
        {
            var request = new RestRequest(API_RECORDINGS_START, Method.Post);

            request.AddHeader("Content-Type", "application/json");

            var body = JsonNode.Parse(properties.ToJson())?.AsObject();

            body.Add("session", sessionId);
            request.AddParameter("application/json", body.ToJsonString(), ParameterType.RequestBody);


            RestResponse response = httpClient.Execute(request);


            return null;


        }
        public Recording StartRecording(String sessionId, String name)
        {
            return StartRecording(sessionId, new RecordingProperties()
            {
                Name = name
            });
        }

        //public Recording StartRecording(String sessionId)
        //{
        //}
        //public Recording StopRecording(String recordingId)
        //{
        //}
        //public Recording GetRecording(String recordingId)
        //{
        //}

        //public List<Recording> ListRecordings()
        //{

        //}


        //public void DeleteRecording(String recordingId)
        //{

        //}

        public List<Session> GetActiveSessions()
        {
            return activeSessions.Values.ToList();
        }

        public Session GetActiveSession(String sessionId)
        {
            if (activeSessions.ContainsKey(sessionId))
                return activeSessions[sessionId];
            throw new OpenViduClientException($"the session {sessionId} not found");
        }

        public bool Fetch()
        {
            var request = new RestRequest(API_SESSIONS, Method.Get);
            request.AddHeader("Content-Type", "application/json");


            RestResponse response = httpClient.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new OpenViduHttpException(response.StatusCode.ToString());

            var json = JsonNode.Parse(response.Content ?? string.Empty);
            var data = json["content"].ToString();
            var list = JsonSerializer.Deserialize<Session[]>(data);

            foreach (var session in list)
            {
                activeSessions.Add(session.Id, session);
            }
            return false;
        }
    }
}