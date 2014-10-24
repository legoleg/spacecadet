using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
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

	void OnCollisionEnter2D (Collision2D collision)
	{
		if (collision.gameObject.tag == "asteroid") {
			gameController.AddPoints(1);
			Destroy(collision.gameObject);
			GameObject explosionInstance = (GameObject)Instantiate (explosion, collision.contacts[0].point, Quaternion.identity);
			Destroy (explosionInstance, 3f);
			Destroy (gameObject);
		}
	}
}
