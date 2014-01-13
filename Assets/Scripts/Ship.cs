using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {
	public bool btnLeftDown = false;
	public bool btnRightDown = false;
	public float tiltThreshold = .3f;
	public float freezeTime = 0.2f;
	public static float fireRate = .4f;//0.5217391f;
	public GameObject bullet;
	public int bulletSpeed = 200;
	public Transform bulletSpawnTransform;
	public GameObject[] spawns;

	SpawnController spawnController;
	int currPos = 1;

	void Start ()
	{
		spawnController = GameObject.Find ("SpawnController").GetComponent<SpawnController> ();
		spawns = spawnController.spawns;
		StartCoroutine(Shoot());
	}

	IEnumerator Shoot ()
	{
		GameObject bulletInstance = (GameObject)Instantiate (bullet, bulletSpawnTransform.position, Quaternion.identity);
		bulletInstance.rigidbody.AddForce(Vector2.up * bulletSpeed, ForceMode.Force);
		yield return new WaitForSeconds (fireRate);
		StartCoroutine(Shoot());
	}

	void Update ()
	{
#if UNITY_EDITOR
		// get input from keyboard
		if (Input.GetKeyDown("left"))
		{
			MoveLeft();
		}	
		else if (Input.GetKeyDown("right"))
		{
			MoveRight();
		}
#endif
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
			"time", .4f
			));
	}
}
