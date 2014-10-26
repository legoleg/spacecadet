using UnityEngine;
using System.Collections;

public class KillingFloor : MonoBehaviour {

	GameController gameController;

	void Start ()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("asteroid")) {
			gameController.AddPoints(-1);
		}
		Destroy(other.gameObject);
	}
}
