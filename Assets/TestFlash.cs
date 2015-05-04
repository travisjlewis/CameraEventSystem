using UnityEngine;
using System.Collections;

public class TestFlash : MonoBehaviour {
	
	bool fired = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!fired)
		{
			CameraEventHandler.G.FireEvent(CameraEvent.RedFlash, 5.0f, 0.7f);
			fired = true;
		}
	
	}
}
