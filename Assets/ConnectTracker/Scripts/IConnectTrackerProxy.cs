using com.connectedinteractive.sdk;
using Assets.ConnectSdk.Scripts;

namespace Assets.ConnectSdk.Scripts
{
    public interface IConnectTrackerProxy
    {
        void Init(string apiKey, string url = Constants.ApiUrl, bool sandbox = false, bool locationServices = false);
        void OnPause();
        void OnResume();
        void Update();
        void TrackEvent(string key, string value = "");
        // void SetLogLevel(ConnectLogLevel level);
    }
}