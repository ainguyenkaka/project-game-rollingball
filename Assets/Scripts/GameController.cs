using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

	public GameObject explosion;
	public AudioClip explodeAudio;
	public Text mainText;
	public GameObject joystick;

	private AudioSource mAudio;
	private PlayerController player;
	private bool paused;
	private bool lost;


	void Start ()
	{
		mAudio = GetComponent<AudioSource> ();
		player = GameObject.Find ("Player").GetComponent<PlayerController> ();
		mainText.text = "Play !";
		StartCoroutine (StartPlaying ());
		paused = false;
		lost = false;
		GUI.UnfocusWindow ();

		graphicSetting ();
	}

	void graphicSetting() {


	}

	IEnumerator StartPlaying(){
		yield return new WaitForSeconds (1);
		mainText.text = "";
	}

	void LateUpdate(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
			
	}

	public void Explosion (Vector3 position)
	{
		mAudio.clip = explodeAudio;
		mAudio.Play ();
		GameObject.Instantiate (explosion, position, explosion.transform.rotation);
		Lose ();
	}

	public void Lose ()
	{
		lost = true;
		mainText.text = "You Lost!";

		StartCoroutine (MyRestart ());
	}

	public void Win ()
	{
		mainText.text = "You Win!";
	}

	public void StopCamera ()
	{
		CameraController camera = GameObject.Find ("Main Camera").GetComponent<CameraController> ();
		camera.Stop ();
	}

	IEnumerator MyRestart ()
	{
		yield return new WaitForSeconds (1);
		if (!paused) {
			UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);    
		}
	}


//	void FixedUpdate() {
//		if (Input.GetKeyDown ("enter") || Input.GetKeyDown ("return")) {
//			Pause ();
//		}
//	}

	public void Pause(){
		if (paused && lost) {
			StartCoroutine (MyRestart ());
		}

		paused = player.TooglePause ();
		if (paused) {
			mainText.text = "Paused!";
		} else {
			mainText.text = "";
		}

	
	}


	public void ToggleSound() {
		if (mAudio.mute) {
			mAudio.mute = false;
		} else {
			mAudio.mute = true;
		}

		player.SetSoundMute (mAudio.mute);
	}

	public void ToggleJoystick() {
		bool joystickUse = player.ToggleJoystick ();

		if (joystickUse) {
			joystick.SetActive (true);
		} else {
			joystick.SetActive (false);
		}
			
	}

}
