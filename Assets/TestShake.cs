using UnityEngine;
using System.Collections;


public class TestShake : MonoBehaviour {

	bool fired = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(!fired)
		{
			CameraEventHandler.G.FireEvent(CameraEvent.Shake, 0.5f);
			fired = true;
		}
	}
}
