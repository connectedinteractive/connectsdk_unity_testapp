using UnityEngine;

namespace Assets.ConnectSdk.Scripts
{
    class ConnectLog
    {
        public static void Write(string message)
        {
            Debug.Log(string.Format("[ConnectSdk]: {0}", message));
        }
    }
}
