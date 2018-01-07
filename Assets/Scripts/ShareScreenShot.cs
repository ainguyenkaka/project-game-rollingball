using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

public class ShareScreenShot : MonoBehaviour {




	[DllImport("__Internal")]
	private static extern void sampleMethod (string iosPath, string message);
//	[DllImport("__Internal")]
//	private static extern void sampleTextMethod (string message);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Share() {
		StartCoroutine( ShareTrigger() );
	}

	IEnumerator ShareTrigger() {
		yield return new WaitForEndOfFrame();
		// create the texture
		Texture2D screenTexture = new Texture2D(Screen.width, Screen.height,TextureFormat.RGB24,false);
		// put buffer into texture
		screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height),0,0);
		// apply
		screenTexture.Apply();


		//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- PHOTO
		byte[] bytes = screenTexture.EncodeToPNG();
		string destination = Path.Combine(Application.persistentDataPath,"MyImage.png");
		File.WriteAllBytes(destination, bytes);

		if (Application.platform == RuntimePlatform.Android)
			ShareByAndroid (destination);
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
			ShareByIOS(destination);
	}

	void ShareByAndroid(string destination) {


		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		intentObject.Call<AndroidJavaObject>("setType", "image/*");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "Media Sharing ");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), "Media Sharing ");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Media Sharing Android Demo");

		AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");


		AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", destination);// Set Image Path Here
		AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);

		// string uriPath =  uriObject.Call("getPath");
		bool fileExist = fileObject.Call<bool>("exists");

		if (fileExist)
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		currentActivity.Call("startActivity", intentObject);



	}
	void ShareByIOS(string path) {
		
		string shareMessage = "Wow I Just Scored 10 ";
		sampleMethod (path, shareMessage);

	}

}
