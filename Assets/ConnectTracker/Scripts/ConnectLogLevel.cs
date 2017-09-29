// ReSharper disable UnusedMember.Local

namespace com.connectedinteractive.sdk
{

    public enum ConnectLogLevel {
        Debug = 0,
        Error = 1,
        Suppress = 2
    }
    public static class ConnectLogLevelExtension {
        public static string lowercaseToString(this ConnectLogLevel logLevel) {
            return logLevel.ToString().ToLower();
        }
        
        public static ConnectLogLevel defaultLevel() {
            return ConnectLogLevel.Debug;
        }
    }

}