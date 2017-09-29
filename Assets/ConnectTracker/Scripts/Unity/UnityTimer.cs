using System;
using System.Collections;
using UnityEngine;

namespace Assets.ConnectSdk.Scripts.Unity
{
    public class UnityTimer : MonoBehaviour
    {
        public float Interval  = 5f;

        public EventHandler Elapsed;

        private GameObject _connectGameObject;

        public UnityTimer()
        {           
            ConnectLog.Write(string.Format("TimerInterval: {0} seconds", Interval.ToString()));
        }

        void Start() {
            StartCoroutine(RunTimer());
        }

        IEnumerator RunTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(Interval);
                if (Elapsed != null) Elapsed(this, new EventArgs());
            }
        }
    }
} 