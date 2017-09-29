using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BestHTTP;
using UnityEngine;

namespace Assets.ConnectSdk.Scripts.Unity
{
    public interface IMessage : IDisposable
    {
        string Payload { get; set; }
        string UrlPath { get;set;  }
        string Method { get; set;  }
        string BaseUrl { get; set;  }
        void Process(Dictionary<string, string> substitutionDictionary = null);
    }

    public class Message : IMessage
    {

        public string Payload { get; set; }
        public string UrlPath { get; set; }
        public string Method { get; set; }
        public string BaseUrl { get; set; }

        public void Process(Dictionary<string, string> substitutionDictionary = null)
        {
            var url = new Uri((BaseUrl + UrlPath).Substitute(substitutionDictionary));
            ConnectLog.Write(string.Format("HTTP_Message: {0}, {1}", Payload, url));
            var body = Payload.Substitute(substitutionDictionary);
            HTTPRequest request = new HTTPRequest(url, HTTPMethods.Post, false, true, OnRequestFinished);
            WWWForm form = new WWWForm();
            form.AddField("data", body);
            request.SetFields(form);
            // ConnectUnitySdk.HttpClient.SendHttpRequest(request);
            ConnectLog.Write("HTTP_Message sent!");
        }

        public HTTPRequest ToHTTPRequest(Dictionary<string, string> substitutionDictionary = null) {
            var url = new Uri((BaseUrl + UrlPath).Substitute(substitutionDictionary));
            var body = Payload.Substitute(substitutionDictionary);
            HTTPRequest request = new HTTPRequest(url, HTTPMethods.Post, false, true, OnRequestFinished);
            WWWForm form = new WWWForm();
            form.AddField("data", body);
            request.SetFields(form);

            string logMessage = string.Format("Http Request [path: {0}], [message: {1}]", UrlPath, body);
            ConnectLog.Write(logMessage);

            return request;
        }

        void OnRequestFinished(HTTPRequest request, HTTPResponse response)
        {
            var msg = string.Format("Http response [path: {0}], [message: {1}], [state: {2}], [statusCode: {3}], [data: {4}]",
                                            request.Uri.AbsolutePath,
                                            response.Message,
                                            request.State.ToString(),
                                            response.StatusCode,
                                            response.DataAsText);
            ConnectLog.Write(msg);
            if (request.State == HTTPRequestStates.ConnectionTimedOut || request.State == HTTPRequestStates.TimedOut)
            {
                request.Clear();
                request.Send();
                return;  
            }
            this.Dispose();
        }

        public void Dispose()
        {
            Payload = BaseUrl = UrlPath = Method = null;
        }

        private static byte[] BodyBytes(string postData)
        {
            if (postData.IsNullOrEmpty()) return new byte[0];

            var urlEncoded = string.Format("data={0}", WWW.EscapeURL(postData));
            return Encoding.UTF8.GetBytes(urlEncoded);
        }
    }
}