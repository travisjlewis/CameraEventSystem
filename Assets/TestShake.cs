using UnityEngine;
using System.Collections;


public class TestShake : MonoBehaviour {

	float timer = 1.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (timer < 0.0f) {
			CameraEventHandler.G.FireEvent (CameraEvent.Shake, 0.1f);
			timer = 1.0f;
		}

		timer -= Time.deltaTime;
	}
}
