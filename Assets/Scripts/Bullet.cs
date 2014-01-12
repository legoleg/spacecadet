using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public GameObject explosion;

	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.CompareTag("asteroid"))
		{
			GameController.points++;
		}

		Destroy(collision.gameObject);
		GameObject explosionInstance = (GameObject)Instantiate (explosion, collision.contacts[0].point, Quaternion.identity);
		Destroy (explosionInstance, 3f);
		Destroy (gameObject);
	}
}
