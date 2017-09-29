using UnityEngine;
using System.Collections;
using System;
 
 namespace Assets.ConnectSdk.Scripts.Unity
 {
    public class WaitBehaviour : MonoBehaviour {
        public void Wait(float seconds, Action action) {
            StartCoroutine(_wait(seconds, action));
        }

        IEnumerator _wait(float time, Action callback) {
            yield return new WaitForSeconds(time);
            callback();
        }
    }
 }