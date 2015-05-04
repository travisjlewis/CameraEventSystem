using UnityEngine;
using UnityEngine.UI;
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
		}
	}
	
	private CameraEventHandler ()
	{
	}
	#endregion

	#region Public Designer-Configurable Variables
	public GameObject cameraTarget;
	public GameObject blindPanel;
	public GameObject redFlashPanel;

	public float fadeInTime = 0.5f;
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
		float currentAngle = 0.0f;
		Quaternion initialRotation = myCameraTransform.rotation;

		while(counter > 0.0f)
		{
			//TODO: How often we shake should be related to intensity.
			if(shakeCounter > 0.02f)
			{
				shakeCounter = 0.0f;
				shakeAngle = (shakeAngle + Mathf.PI * 0.7f) % (Mathf.PI * 2f);
			}
			myCameraTransform.rotation *= Quaternion.Euler(Mathf.Cos(shakeAngle) * intensity, Mathf.Sin(shakeAngle) * intensity, 0.0f);
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
		
		float fadeCounter = 0f;

		Image flashImage = redFlashPanel.GetComponent<Image> ();


		float pingPongRange = intensity * 0.33f;
		float pingPongOffset = intensity - pingPongRange;

		//Lerp it in
		while(fadeCounter < fadeInTime)
		{
			flashImage.color = new Color(1, 1, 1, Mathf.Lerp(0.0f, intensity, (fadeCounter / fadeInTime)));
			fadeCounter += Time.deltaTime;
			yield return null;
		}
		
		fadeCounter = 0.0f;

		//if a time was provided, wait that time then lerp it back
		if(time != 0.0f)
		{
			while(counter > 0.0f)
			{
				float alphaValue = Mathf.PingPong(counter / 2.0f, pingPongRange) + pingPongOffset;
				flashImage.color = new Color(1, 1, 1, alphaValue);
				counter -= Time.deltaTime;
				yield return null;
			}
			
			//And lerp it back.
			while(fadeCounter < fadeInTime)
			{
				flashImage.color = new Color(1, 1, 1, Mathf.Lerp(intensity, 0.0f, (fadeCounter / fadeInTime)));
				fadeCounter += Time.deltaTime;
				yield return null;
			}
		}
		yield return null;
	}

	private IEnumerator Blind(object[] args)
	{
		float time = (float)args[0];
		float intensity = (float)args[1];
		float counter = time;

		float fadeCounter = 0f;

		//Image.CrossFadeAlpha is so hilariously broken. Fading to a higher alpha just plain DOESN'T work. So I have to lerp it myself.
		//Also, set the color to full black in code, so that it doesn't completely block the view in the editor
		Image blindImage = blindPanel.GetComponent<Image> ();
		CanvasRenderer blindImageRenderer = blindImage.canvasRenderer;

		//Lerp it in
		while(fadeCounter < fadeInTime)
		{
			blindImage.color = new Color(0, 0, 0, Mathf.Lerp(0.0f, intensity, (fadeCounter / fadeInTime)));
			fadeCounter += Time.deltaTime;
			yield return null;
		}

		fadeCounter = 0.0f;

		//Wait the blind duration
		while(counter > 0.0f)
		{
			counter -= Time.deltaTime;
			yield return null;
		}

		//And lerp it back.
		while(fadeCounter < fadeInTime)
		{
			blindImage.color = new Color(0, 0, 0, Mathf.Lerp(intensity	, 0.0f, (fadeCounter / fadeInTime)));
			fadeCounter += Time.deltaTime;
			yield return null;
		}
		yield return null;
	}
	#endregion
}
