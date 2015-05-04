using UnityEngine;
using System.Collections;

public class TestBlind : MonoBehaviour {

	bool fired = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!fired)
		{
			CameraEventHandler.G.FireEvent(CameraEvent.Blind, 3.0f);
			fired = true;
		}
	
	}
}
