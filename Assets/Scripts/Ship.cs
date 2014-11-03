using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

	public GameObject explosion;
	public GameObject bullet;
	public int bulletForce = 150;

	public AudioClip[] crashSounds;

	public bool canShoot = false;
	public bool canMove = true;

	private GameObject[] tracks;

	private Transform bulletSpawnTransform;
	private GameController gameController;
	private float movementTempo;
	private SpawnController spawnController;

	private int currentTrack = 1;

	void Start ()
	{
		bulletSpawnTransform = GameObject.Find ("BulletSpawnPoint").transform;

		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		var tempo = gameController.GetComponent<GameController>().tempo;
		movementTempo = tempo/3;

		spawnController = GameObject.Find ("SpawnController").GetComponent<SpawnController> ();
		tracks = spawnController.spawns;
	}

	public void Shoot ()
	{
		if (canShoot) {
			GameObject bulletInstance = (GameObject)Instantiate (bullet, bulletSpawnTransform.position, Quaternion.identity);
			bulletInstance.rigidbody2D.AddForce(Vector2.up * bulletForce, ForceMode2D.Force);
		
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
			if (currentTrack > 0) {
				currentTrack--;
				ApplyMove (currentTrack);
			}
		}	
		else if (Input.GetKeyDown("right")) {
			if (currentTrack < tracks.Length-1) {
				currentTrack++;
				ApplyMove (currentTrack);
			}
		}
#endif
	}

	public void ApplyMove (int track)
	{
		if (!canShoot) {
			canShoot = true;
		}

		if (canMove) {
			iTween.MoveTo (this.gameObject, iTween.Hash (
				"position", new Vector3 (tracks [track].transform.position.x, transform.position.y, transform.position.z)
				,"easetype", iTween.EaseType.spring
				,"time", movementTempo
				));
		}
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		if (collision.gameObject.CompareTag("asteroid")) {
			gameController.LoseHeart();
			if (crashSounds.Length > 0) {
				audio.PlayOneShot(crashSounds[Random.Range(0, crashSounds.Length)]);
			}
		}
		
		Destroy(collision.gameObject);
		GameObject explosionInstance = (GameObject)Instantiate (explosion, collision.contacts[0].point, Quaternion.identity);
		Destroy (explosionInstance, 3f);
	}

}
