using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	public string name;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GoToOtherScene()
	{
		Debug.Log("Clicked");
		if (SceneManager.GetActiveScene().name == "Scene1")
		{
			SceneManager.LoadScene("Scene2");
		}
		else
		{
			SceneManager.LoadScene("Scene1");	
		}
	}

	public void ButtonClickLeft() 
	{
		//ConnectTracker.TrackEvent(string.Format("{0}_button_click_left", name));
	}

	public void ButtonClickRight() 
	{
		//ConnectTracker.TrackEvent(string.Format("{0}_button_click_right", name), "value_test");
	}
}
