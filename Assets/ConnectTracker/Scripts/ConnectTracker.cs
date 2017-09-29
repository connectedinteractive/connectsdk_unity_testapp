using Assets.ConnectSdk.Scripts;
using Assets.ConnectSdk.Scripts.Dummy;
using Assets.ConnectSdk.Scripts.Unity;
using UnityEngine;

// ReSharper disable UnusedMember.Local
// ReSharper disable once CheckNamespace
public class ConnectTracker : MonoBehaviour
{
    private static IConnectTrackerProxy _instance;

    [Tooltip("API Key for Connected Interactive - REQUIRED")] public string ApiKey = "REQUIRED";

    [Tooltip("In test mode, all API calls are simulated. Default: false")] [ContextMenuItem("Reset", "ResetTestMode")] public bool TestMode;


    public bool EnableLocationServices = true;

    private static IConnectTrackerProxy Instance
    {
        get { return _instance ?? (_instance = CreateInstance()); }
    }

    private void ResetTestMode()
    {
        TestMode = false;
    }


    // Called by Android native (connectedsdk_android_stripped android project - ConnectedTracker.java) java when INSTALL_REFERRER intent is received
    void RegisterReferrer(string referrer)
    {
        ConnectLog.Write(string.Format("INSTALL_REFERRER: {0}", referrer));
        ConnectSystemInfo.InstallReferrer = referrer;
    }

    // Use this for initialization
    void Start()
    {
        // Keep SDK active throughout scene loads
        DontDestroyOnLoad(this);
        Instance.Init(apiKey: ApiKey, sandbox: TestMode);
    }
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Instance.Update();
    }

    public void Init()
    {
    }


    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            Instance.OnPause();
        else
            Instance.OnResume();
    }

    private static IConnectTrackerProxy CreateInstance()
    {
        return new ConnectTrackerUnity();
    }

    public static void TrackEvent(string key, string value = "")
    {
        Instance.TrackEvent(key, value);
    }

    private void Log(string message)
    {
        Debug.Log(string.Format("ConnectSdk [Unity]:  <{0}>", message));
    }
}