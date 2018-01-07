using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class SharingImage : MonoBehaviour {
	private bool isProcessing = false;
	public Image buttonShare;
	public string message;

	//function called from a button
	public void ButtonShare ()
	{
		buttonShare.enabled = false;
		if(!isProcessing){
			StartCoroutine( ShareScreenshot() );
		}
	}
	public IEnumerator ShareScreenshot()
	{
		isProcessing = true;
		// wait for graphics to render
		yield return new WaitForEndOfFrame();
		//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- PHOTO
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

		if(!Application.isEditor)
		{
			// block to open the file and share it ------------START
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
			

		isProcessing = false;
		buttonShare.enabled = true;
	}
}