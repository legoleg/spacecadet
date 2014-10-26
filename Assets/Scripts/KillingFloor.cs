using UnityEngine;
using System.Collections;

public class KillingFloor : MonoBehaviour {

	GameController gameController;
	public AudioClip[] punishSounds;

	void Start ()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		if (!audio) {
			gameObject.AddComponent<AudioSource>();
			audio.volume = .75f;
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("asteroid")) {
			gameController.AddPoints(-1);
			if (punishSounds.Length > 0) {
				audio.PlayOneShot(punishSounds[Random.Range(0, punishSounds.Length)]);
			}
		}
		Destroy(other.gameObject);
	}
}
