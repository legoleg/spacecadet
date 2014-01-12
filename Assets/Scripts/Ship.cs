using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

	int currPos = 2;

	public float tiltThreshold = .3f;
	public float freezeTime = 0.2f;
	public GameObject bullet;
	public Transform bulletSpawnTransform;
	public GameObject[] spawns;

	SpawnController spawnController;
	bool readyToMove = true;

	void Start ()
	{
		spawnController = GameObject.Find ("SpawnController").GetComponent<SpawnController> ();
		spawns = spawnController.spawns;
	}

	void Shoot ()
	{
		GameObject bulletInstance = (GameObject)Instantiate (bullet, bulletSpawnTransform.position, Quaternion.identity);
		bulletInstance.rigidbody.AddForce(Vector2.up * 200, ForceMode.Force);
	}

	IEnumerator FreezeMovement ()
	{
		readyToMove = false;
		yield return new WaitForSeconds (freezeTime);
		readyToMove = true;
	}
	
	void Update ()
	{
		if (Input.acceleration.normalized.x < -tiltThreshold && readyToMove)
		{
			StartCoroutine(FreezeMovement ());
			MoveLeft();
		}
		else if (Input.acceleration.normalized.x > tiltThreshold && readyToMove)
		{
			StartCoroutine(FreezeMovement ());
			MoveRight();
		}


		// get input
		if (Input.GetKeyDown("left"))
		{
			MoveLeft();
		}
		else if (Input.GetKeyDown("right"))
		{
			MoveRight();
		}
	}
	
	public void MoveLeft ()
	{
		//only move 
		if (currPos > 0) {
			currPos--;
			//do the movement
			Move ();
		}
	}
	
	public void MoveRight ()
	{
		if (currPos < spawns.Length-1) {
			currPos++;
			//do the movement
			Move ();
		}
	}
	
	void Move ()
	{
		iTween.MoveTo (this.gameObject, iTween.Hash (
			"position", new Vector3 (spawns [currPos].transform.position.x, transform.position.y, transform.position.z), 
			"easetype", iTween.EaseType.spring, 
			"time", .4f,
			"oncomplete", "Shoot"));
		//TODO continue shooting when not moving
	}
}
