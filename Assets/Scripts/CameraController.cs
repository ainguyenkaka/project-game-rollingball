using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

	public GameObject player;

	private Vector3 offset;
	private bool stop;

	// Use this for initialization
	void Start ()
	{
		offset = transform.position - player.transform.position;
		stop = false;
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		if (!stop) {
			transform.position = player.transform.position + offset;
		}
	}

	public void Stop ()
	{
		stop = true;
	}
}
