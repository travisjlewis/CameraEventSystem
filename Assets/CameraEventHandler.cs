using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum CameraEvent {
	Shake,
	RedFlash,
	Blind
};

public class CameraEventHandler : MonoBehaviour {

	#region Singleton Behavior
	public static CameraEventHandler G { get; private set; }
	
	void Awake ()
	{
		if (G != null && G != this) {
			Destroy (gameObject);
		} else {
			G = this;
			// TODO: think about lifecycle
			//DontDestroyOnLoad (gameObject);
		}
	}
	
	private CameraEventHandler ()
	{
	}
	#endregion

	#region Public Designer-Configurable Variables
	public GameObject cameraTarget;
	#endregion

	#region Cache Variables
	private Transform myCameraTransform;
	#endregion

	#region Unity Functions
	void Start () {
		myCameraTransform = Camera.main.transform;
	}
	#endregion

	#region Public Functions
	public void FireEvent(CameraEvent eventType, float time, float intensity=1.0f)
	{
		object[] toPassArgs = new object[2]{time, intensity};
		StartCoroutine(eventType.ToString(), toPassArgs);
	}
	#endregion

	#region Hidden Helper Functions
	private IEnumerator Shake(object[] args)
	{
		float time = (float)args[0];
		float intensity = (float)args[1];
		float counter = time;

		float shakeCounter = 0.0f;
		float shakeAngle = 0.0f;
		Quaternion initialRotation = myCameraTransform.rotation;

		while(counter > 0.0f)
		{
			if(shakeCounter > 0.06f)
			{
				shakeCounter = 0.0f;
				shakeAngle = (shakeAngle + Mathf.PI * 0.7f) % (Mathf.PI * 2f);
				//TODO: Lerp between these values, instead of instantly jerking to that angle.
				myCameraTransform.rotation *= Quaternion.Euler(Mathf.Cos(shakeAngle) * intensity, Mathf.Sin(shakeAngle) * intensity, 0.0f);
			}
			shakeCounter += Time.deltaTime;
			counter -= Time.deltaTime;
			yield return null;
		}

		myCameraTransform.rotation = initialRotation;
		yield return null;
	}

	private IEnumerator RedFlash(object[] args)
	{
		float time = (float)args[0];
		float intensity = (float)args[1];
		float counter = time;
		
		while(counter > 0.0f)
		{
			counter -= Time.deltaTime;
			yield return null;
		}
		yield return null;
	}

	private IEnumerator Blind(object[] args)
	{
		float time = (float)args[0];
		float intensity = (float)args[1];
		float counter = time;
		
		while(counter > 0.0f)
		{
			counter -= Time.deltaTime;
			yield return null;
		}
		yield return null;
	}
	#endregion
}
