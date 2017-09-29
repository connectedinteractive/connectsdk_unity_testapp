using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.ConnectSdk.Scripts.Unity
{
    public struct TrackedEvent
    {
        public string Key;
        public string Value;

        public DateTime Timestamp;

        public static Hashtable EventHash(List<TrackedEvent> trackedEvents, string apiKey)
        {
            var actions = new List<Dictionary<string, string>>();

            foreach(var thisEvent in trackedEvents){
                actions.Add(thisEvent.ToSerializableDictionary());
            }

            var hash = new Hashtable()
            {
                {
                    "actions", actions
                },
                {
                    "system", ConnectSystemInfo.Instance().Info
                },
                {
                    "app_key", apiKey
                },
                {
                    "location", ConnectSystemInfo.Location()
                }
            };

            return hash;
        }
    }
}
