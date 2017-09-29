using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Assets.ConnectSdk.Scripts.Unity
{
    public static class ExtensionMethods
    {
        public static string Substitute(this string str, Dictionary<string, string> substitutionDictionary)
        {
            if (substitutionDictionary == null) return str;
            foreach (var entry in substitutionDictionary)
            {
                var key = string.Format("%{0}%", entry.Key);
                var value = entry.Value;
                str = str.Replace(key, value);
            }
            return str;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return str == null || str.Trim() == "";
        }


        public static string ToJson(this Hashtable toSerialize)
        {
            var s = JsonConvert.SerializeObject(toSerialize);
            return s;
        }
        
        public static Dictionary<string, string> ToSerializableDictionary(this TrackedEvent trackedEvent) {
            return new Dictionary<string, string> {
                        {"action", trackedEvent.Key},
                        {"event", trackedEvent.Value},
                        {"timestamp", trackedEvent.Timestamp.ToString("o")},  // ISO8601 universal time interchange format
                        {"impression_id", ConnectSystemInfo.InstallReferrer }
                    };
            }
        }

    }
