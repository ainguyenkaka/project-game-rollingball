using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

	public float speed = 1;
	public Text scoreText;
	public AudioClip pickAudio;

	public static string SCORE_KEY = "Count_Key";
	private Rigidbody rb;
	private int score;
	private GameController gameController;
	private GameObject pickups;
	private AudioSource mAudio;
	private bool paused;
	private Vector3 savedVelocity;
	private Vector3 savedAngularVelocity;
	private bool joystickUse;

	void Awake()
	{
		gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
		pickups = GameObject.Find ("Pick Ups");
	
	}

	// Use this for initialization
	void Start () {
		score = 0;
		paused = false;
		joystickUse = false;
		rb = GetComponent<Rigidbody> ();
		mAudio = GetComponent<AudioSource> ();

		UpdateCount ();



	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!paused) {
			
			float moveHorizontal = 0;
			float moveVertical = 0;
			if (joystickUse) {
				 moveHorizontal = CrossPlatformInputManager.GetAxis ("Horizontal");
				 moveVertical = CrossPlatformInputManager.GetAxis ("Vertical");
			} else {

				#if (UNITY_IPHONE || UNITY_ANDROID)
				moveHorizontal = Input.acceleration.x * 2;
				moveVertical = Input.acceleration.y * 2;
				#else
				 moveHorizontal = Input.GetAxis ("Horizontal");
				 moveVertical = Input.GetAxis ("Vertical");
				#endif

			}
	
			Vector3 movement = new Vector3 (moveHorizontal, 0, moveVertical);
			rb.AddForce (movement * speed);
		}
	}

	void LateUpdate() 
	{
		if (pickups.transform.childCount == 0) {
			gameController.Win ();
		}


	}

	public bool TooglePause(){
		paused = !paused;

		if (paused) {
			savedVelocity = rb.velocity;
			savedAngularVelocity = rb.angularVelocity;
			rb.isKinematic = true;
		} else {
			rb.isKinematic = false;
			rb.AddForce (savedVelocity, ForceMode.VelocityChange);
			rb.AddTorque (savedAngularVelocity, ForceMode.VelocityChange);
		}
		return paused;
	}

	void OnTriggerEnter(Collider other)
	{
		GameObject obj = other.gameObject;
		if (obj.CompareTag ("Pick Up")) {
			Destroy (other.gameObject);
			PickUp ();
		} else if (obj.CompareTag ("Trap")) {
			
			LostByTrap ();
		}
	}

	void PickUp() {
		score++;
		mAudio.clip = pickAudio;
		mAudio.Play ();
		UpdateCount ();
	}

	private void LostByTrap() 
	{
		
		gameController.Explosion (this.transform.position);
		this.gameObject.SetActive (false);

	}

	private void LostByFailed()
	{
		
	}

	private void UpdateCount() {
		scoreText.text = "Score: " + score.ToString ();

	}

	public void SetSoundMute(bool mute) {
		mAudio.mute = mute;
	}


	public bool ToggleJoystick() {
		joystickUse = !joystickUse;
		return joystickUse;
	}
}
