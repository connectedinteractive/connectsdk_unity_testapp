using System.Collections;
using BestHTTP;
using UnityEngine;

namespace Assets.ConnectSdk.Scripts.Unity
{

    public class HttpClient : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
			Debug.Log("Setting up HttpClient sender");
			BestHTTP.HTTPManager.Setup();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SendHttpRequest(Message request) {
            StartCoroutine(AsyncHttpRequest(request));
        }

        private IEnumerator AsyncHttpRequest(Message request)
        {
            yield return StartCoroutine(request.ToHTTPRequest().Send());
        }        
    }

}