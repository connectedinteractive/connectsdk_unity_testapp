using System;
using System.Collections.Generic;
using com.connectedinteractive.sdk;
using Assets.ConnectSdk.Scripts;
using UnityEngine;

namespace Assets.ConnectSdk.Scripts.Unity
{
    public class ConnectTrackerUnity : IConnectTrackerProxy
    {
        private static TimedQueue<Message> _messageQueue;
        private static TimedQueue<TrackedEvent> _eventQueue;
        private static readonly float TimerInterval = 15f;

        private bool Initialized = false;

        private string _apiKey;
        private string _baseUrl;

        private bool _sandbox;

        private bool _sentAppOpen;
        private bool _sentAppInstall;

        private bool HttpEnabled = false;

        static GameObject _connectGameObject;

        static HttpClient _httpClient;

        static WaitBehaviour _waitBehaviour;

        internal static HttpClient HttpClient
        {
            get { return _httpClient ?? (_httpClient = ConnectGameObject.AddComponent<HttpClient>()); }
        }

        static GameObject ConnectGameObject
        {
            get { return _connectGameObject ?? (_connectGameObject = GameObject.FindWithTag("ConnectTracker")); }
        }

        static WaitBehaviour WaitBehaviour
        {
            get { return _waitBehaviour ?? (_waitBehaviour = ConnectGameObject.AddComponent<WaitBehaviour>()); }
        }

        private static TimedQueue<Message> MessageQueue
        {
            get { return _messageQueue ?? (_messageQueue = new TimedQueue<Message>(TimerInterval)); }
        }

        private static TimedQueue<TrackedEvent> EventQueue
        {
            get { return _eventQueue ?? (_eventQueue = new TimedQueue<TrackedEvent>(TimerInterval)); }
        }

        public void Init(string apiKey, string url = Constants.ApiUrl, bool sandbox = false, bool locationServices = false)
        {
            if (Initialized) return;
            ConnectLog.Write("Initializing...");
            _apiKey = apiKey;
            _baseUrl = url;
            _sandbox = sandbox;
            SetupAdvertisingIdAsyncEvents();
            SendInitMessage();
            SetupQueues();
            ConnectSystemInfo.LocationServicesEnabled = locationServices;
            Initialized = true;
        }

        private void SetupAdvertisingIdAsyncEvents()
        {
            ConnectSystemInfo.Instance().AdvertisingIdCollectedEvent += AdvertisingIdCollected;
            ConnectSystemInfo.Instance().CollectAdvertisingId();
        }

        private void AdvertisingIdCollected(object sender, EventArgs eventArgs)
        {
            ConnectLog.Write("Advertising ID registered, free to process queue");
            HttpEnabled = true;
            HttpClient.enabled = true;
            OnOpen();
        }

        private void SendInitMessage()
        {
            var initMessage = new Message();
            initMessage.BaseUrl = _baseUrl;
            initMessage.Payload = "";
            initMessage.UrlPath = string.Format("/api/init_connect/{0}", _apiKey);
            initMessage.Method = "POST";
            MessageQueue.Enqueue(initMessage);
        }

        public void Update()
        {
            if (!HttpEnabled) return;
            EventQueue_Elapsed(this, new EventArgs());
            MessageQueue_Elapsed(this, new EventArgs());
        }

        public void OnPause()
        {
        }

        public void OnResume()
        {
        }

        public void TrackEvent(string key, string value = "")
        {
            ConnectLog.Write(string.Format("Event tracked [key: {0}], [value: {1}]", key, value));
            EventQueue.Enqueue(new TrackedEvent { Key = key, Value = value, Timestamp = DateTime.UtcNow });
        }

        private void SetupQueues()
        {
            MessageQueue.Elapsed += MessageQueue_Elapsed;
            EventQueue.Elapsed += EventQueue_Elapsed;
        }

        private void EventQueue_Elapsed(object sender, EventArgs e)
        {
            if (EventQueue.Count > 0) 
            {
                var eventList = EventQueue.ToList();
                EventQueue.Clear();
                ProcessEvents(eventList);
            }
        }

        private void MessageQueue_Elapsed(object sender, EventArgs e)
        {
            if (_sandbox) return;
            // Testing sending a single message per frame instead of bulk sending every 15 seconds
            if (MessageQueue.Count > 0)
            {
                
                HttpClient.SendHttpRequest(MessageQueue.Dequeue());
            }
        }

        private void OnOpen()
        {
            if (Application.platform == RuntimePlatform.Android && string.IsNullOrEmpty(ConnectSystemInfo.InstallReferrer))
            {
                WaitForReferrerId(0);
            }
            else
            {
                SendDefaultEvents();
            }
        }

        private void WaitForReferrerId(int attempt)
        {
            ConnectLog.Write(string.Format("Attempt {0} of getting referrer ID.", attempt));

            if (string.IsNullOrEmpty(ConnectSystemInfo.InstallReferrer) && attempt < 5)
            {
                WaitBehaviour.Wait(0.5f, () => WaitForReferrerId(attempt + 1));
            }
            else
            {
                SendDefaultEvents();
            }
        }

        private void SendDefaultEvents()
        {
            if (ConnectSystemInfo.IsFirstRun())
            {
                EventQueue.Enqueue(new TrackedEvent { Key = "app_install" });
            }

            EventQueue.Enqueue(new TrackedEvent { Key = "app_open" });
        }

        private void ProcessEvents(List<TrackedEvent> trackedEvents) {
            if (_sandbox) return;

            var initMessage = new Message();
            initMessage.BaseUrl = _baseUrl;
            initMessage.Payload = TrackedEvent.EventHash(trackedEvents, _apiKey).ToJson();
            initMessage.UrlPath = string.Format("{0}/{1}/{2}", ApiEndpoint(), _apiKey, ConnectSystemInfo.OperatingSystemFamily().ToLower());
            initMessage.Method = "POST";

            MessageQueue.Enqueue(initMessage);
        }

        private void ProcessEvent(TrackedEvent trackedEvent)
        {
            ProcessEvents(new List<TrackedEvent> {trackedEvent});
        }

        private static string ApiEndpoint() { 
            return string.Format("/api/unity{0}", ConnectSystemInfo.ConnectSdkVersion); 
        }
    
    }
}