using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public GameObject explosion;
	public float bulletLifeTime = 2f;


	GameController gameController;

	void Start ()
	{
		Destroy (gameObject, bulletLifeTime);
		ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
		foreach (var particleSystem in particleSystems) {
			particleSystem.startLifetime = bulletLifeTime;
		}
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController>();
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.CompareTag("asteroid"))
		{
			gameController.points++;
			// TODO Check for combo and multiply
			gameController.TweenGameObject(gameController.pointsTxt);
		}

		Destroy(collision.gameObject);
		GameObject explosionInstance = (GameObject)Instantiate (explosion, collision.contacts[0].point, Quaternion.identity);
		Destroy (explosionInstance, 3f);
		Destroy (gameObject);
	}
}
