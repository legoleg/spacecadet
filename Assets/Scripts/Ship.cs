using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

	public float tiltThreshold = .3f;
	public float freezeTime = 0.2f;
	public float fireRate;// = 0.5217391f;

	public GameObject explosion;
	public GameObject bullet;
	public int bulletSpeed = 200;
	public bool canShoot = false;
	public bool canMove = false;

	public GameObject[] spawns;

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
		fireRate = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().tempo;
		//TODO try timing the shot in another way than coroutines
		StartCoroutine(Shoot());
	}

	IEnumerator Shoot ()
	{
		if (canShoot)
		{
			GameObject bulletInstance = (GameObject)Instantiate (bullet, bulletSpawnTransform.position, Quaternion.identity);
			bulletInstance.rigidbody2D.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Force);
		}

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
		if (canMove) {
			iTween.MoveTo (this.gameObject, iTween.Hash (
				"position", new Vector3 (spawns [currPos].transform.position.x, transform.position.y, transform.position.z), 
				"easetype", iTween.EaseType.spring, 
				"time", .4f
				));
		}
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		if (collision.gameObject.CompareTag("asteroid"))
		{
			gameController.lives--;
			gameController.HideNextHeart();
			if (gameController.lives <= 0)
			{
				gameController.Lose ();
				//TODO fade-in time from zero
			}
		}
		
		Destroy(collision.gameObject);
		GameObject explosionInstance = (GameObject)Instantiate (explosion, collision.contacts[0].point, Quaternion.identity);
		Destroy (explosionInstance, 3f);
//		Destroy (gameObject);
	}

}
