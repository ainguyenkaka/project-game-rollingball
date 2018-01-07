using UnityEngine;
using System.Collections;


public class GroundController : MonoBehaviour {




	void OnCollisionExit(Collision collisionInfo) {
		GameController gc = GameObject.Find ("Game Controller").GetComponent<GameController> ();
		gc.StopCamera ();
		gc.Lose ();
	}




}
