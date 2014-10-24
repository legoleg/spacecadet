using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

	public GameObject explosion;
	public GameObject bullet;
	public int bulletSpeed = 200;
	public static bool canShoot = false;
	public static bool canMove = false;

	public GameObject[] spawns;

	float tempo;
	Transform bulletSpawnTransform;
	GameController gameController;
	SpawnController spawnController;
	int currPos = 1;

	void Start ()
	{
		bulletSpawnTransform = GameObject.Find ("BulletSpawnPoint").transform;
		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		spawnController = GameObject.Find ("SpawnController").GetComponent<SpawnController> ();
		spawns = spawnController.spawns;
		tempo = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().tempo;
	}

	void Shoot ()
	{
		if (canShoot) {
			GameObject bulletInstance = (GameObject)Instantiate (bullet, bulletSpawnTransform.position, Quaternion.identity);
			bulletInstance.rigidbody2D.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Force);
		
			if (SpawnController.canSpawn == false) {
				SpawnController.canSpawn = true;
			}
		}
	}

	void Update ()
	{
#if UNITY_EDITOR
		// get input from keyboard
		if (Input.GetKeyDown("left")) {
			MoveLeft();
		}	
		else if (Input.GetKeyDown("right")) {
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
		if (canMove) {
			iTween.MoveTo (this.gameObject, iTween.Hash (
				"position", new Vector3 (spawns [currPos].transform.position.x, transform.position.y, transform.position.z)
				,"easetype", iTween.EaseType.spring
				,"time", tempo
				,"onstart", "Shoot"
				,"onstarttarget", gameObject
				));
		}
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		if (collision.gameObject.CompareTag("asteroid")) {
			gameController.LoseHeart();
		}
		
		Destroy(collision.gameObject);
		GameObject explosionInstance = (GameObject)Instantiate (explosion, collision.contacts[0].point, Quaternion.identity);
		Destroy (explosionInstance, 3f);
	}

}
