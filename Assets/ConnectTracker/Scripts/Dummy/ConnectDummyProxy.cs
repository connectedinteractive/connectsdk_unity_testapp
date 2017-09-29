using System;
using Assets.ConnectSdk.Scripts;
using com.connectedinteractive.sdk;

namespace Assets.ConnectSdk.Scripts.Dummy
{
    internal class ConnectDummyProxy : IConnectTrackerProxy
    {

        private ConnectLogLevel minLogLevel = ConnectLogLevelExtension.defaultLevel();
        public void OnPause()
        {
            Log(ConnectLogLevel.Debug, "OnPause called");
        }

        public void OnResume()
        {
            Log(ConnectLogLevel.Debug, "OnResume called");
        }

        public void TrackEvent(string key, string value = "")
        {
            string message = string.Format("TrackEvent called: [key: {0}], [value: {1}]", key, value);
            Log(ConnectLogLevel.Debug, message);

        }

        public void Init(string apiKey, string url = Constants.ApiUrl, bool sandbox = false, bool locationServices = false)
        {
            string message = string.Format("Init called: [apiKey: {0}], [url: {1}], [sandbox mode: {2}]", apiKey, url, sandbox.ToString());
            Log(ConnectLogLevel.Debug, message);
        }

        private void Log(ConnectLogLevel level, string message)
        {
            if (level < minLogLevel) return;
            ConnectLog.Write(string.Format("ConnectSdk [Dummy] [{0}]:  {1}", level.lowercaseToString(), message));
        }

        public void SetLogLevel(ConnectLogLevel level)
        {
            minLogLevel = level;
        }

        public void Update()
        {
        }
    }
}