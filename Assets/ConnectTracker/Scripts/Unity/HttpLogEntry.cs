using System;

namespace Assets.ConnectSdk.Scripts.Unity
{
    public struct HttpLogEntry
    {
        public string Url;
        public DateTime Timestamp;
        public string Error;
        public string ResponseBody;
    }
}