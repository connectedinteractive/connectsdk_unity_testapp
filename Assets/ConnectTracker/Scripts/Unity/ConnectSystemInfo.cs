using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.ConnectSdk.Scripts.Unity
{

    enum OperatingSystemFamily
    {
        Android,
        iOS,
        WindowsPhone
    }
    public interface IConnectSystemInfo
    {
        bool AdvertisingIdRegistered { get; }
        Dictionary<string, string> Info { get; }
        void CollectAdvertisingId();
        event EventHandler AdvertisingIdCollectedEvent;
    }

    public class ConnectSystemInfo : IConnectSystemInfo
    {
        private string _advertisingId;
        private bool _trackingEnabled;

        public bool AdvertisingIdRegistered { get; private set; }

        private Dictionary<string, string> _collected;
        private static ConnectSystemInfo _instance;

        private static bool _locationServicesEnabled = false;

        internal static bool LocationServicesEnabled
        {
            get { return _locationServicesEnabled && UnityEngine.Input.location.isEnabledByUser; }
            set
            {
                _locationServicesEnabled = value;
                StartLocationServices();
            }
        }

        private static void StartLocationServices()
        {
            if (!LocationServicesEnabled) return;

            Input.location.Start(desiredAccuracyInMeters: 500f, updateDistanceInMeters: 50f);
        }


        public Dictionary<string, string> Info
        {
            get { return _collected ?? (_collected = Collect()); }
        }

        public static bool IsFirstRun()
        {
            if (PlayerPrefs.GetInt("ConnectSdk_firstInstall", 1) != 1) return false;
            PlayerPrefs.SetInt("ConnectSdk_firstInstall", 0);
            return true;
        }

        public static string InstallReferrer
        {
            get { return PlayerPrefs.GetString("ConnectSdk_InstallReferrer", ""); }
            set { PlayerPrefs.SetString("ConnectSdk_InstallReferrer", value); }
        }

        public string TrackingId
        {
            get
            {
                var trackingId = PlayerPrefs.GetString("ConnectSdk_Tracking_ID");
                if (!trackingId.Equals("")) return trackingId;
                trackingId = Guid.NewGuid().ToString();
                PlayerPrefs.SetString("ConnectSdk_Tracking_ID", trackingId);
                return trackingId;
            }
        }

        private Dictionary<string, string> Collect()
        {
            return new Dictionary<string, string>() {
                { "timezone", TimeZone.CurrentTimeZone.StandardName },
                { "advertising_id", _advertisingId },
                { "opt_out_enabled", (!_trackingEnabled).ToString() },
                { "app_name", Application.productName },
                { "model", SystemInfo.deviceModel },
                { "device_id", SystemInfo.deviceUniqueIdentifier },
                { "mac", string.Empty },
                { "os", OperatingSystemFamily() },
                { "os_version", OperatingSystemVersion() },
                { "resolution", GetResolution() },
                { "tracking_id", TrackingId },
                { "platform", string.Format("Unity {0}", Application.unityVersion) },
                { "pmm_sdk_version", ConnectSdkVersion },
                { "app_version", Application.version },
                { "device_name", SystemInfo.deviceName},
                { "accept-language", Application.systemLanguage.ToString() }
            };

        }

        private string OperatingSystemVersion()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return AndroidInfo.OsVersion();
                default:
                    return SystemInfo.operatingSystem;
            }
        }

        public event EventHandler AdvertisingIdCollectedEvent;

        private string GetResolution()
        {
            return string.Format("{0}x{1}", Screen.currentResolution.width, Screen.currentResolution.height);
        }

        internal static object Location()
        {
            if (!LocationServicesEnabled) return "disabled";

            return UnityEngine.Input.location.lastData;
        }

        internal static string OperatingSystemFamily()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                default:
                    return Application.platform.ToString();
            }
        }

        public void RegisterAdvertisingId(string advertisingId, bool trackingEnabled, string error)
        {
            ConnectLog.Write(string.Format("Advertising ID results: [enabled: {0}], [ID: {1}], [error: {2}]", trackingEnabled, advertisingId, error));
            _advertisingId = advertisingId;
            _trackingEnabled = trackingEnabled;
            AdvertisingIdRegistered = true;
            // ReSharper disable once UseNullPropagation
            if (AdvertisingIdCollectedEvent != null) AdvertisingIdCollectedEvent.Invoke(this, EventArgs.Empty);
        }

        internal static string ConnectSdkVersion { get { return "140"; } }

        public static ConnectSystemInfo Instance()
        {
            return _instance ?? (_instance = new ConnectSystemInfo());
        }

        public void CollectAdvertisingId()
        {
            Application.RequestAdvertisingIdentifierAsync(RegisterAdvertisingId);
        }
    }
}