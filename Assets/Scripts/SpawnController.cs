using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

	public float spawnRate = 2.0869564f;
	public GameObject[] objectsToSpawn;
	public GameObject[] spawns;

	
	void Start () {
		spawnRate = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().tempo;

		if (objectsToSpawn.Length < 1 || spawns.Length < 1) {
			Debug.LogError("Assign GameObjects...");
		} else {
			StartCoroutine (DelaySpawnRoutine());
		}
	}

	IEnumerator DelaySpawnRoutine () {
		yield return new WaitForSeconds (3f);
		StartCoroutine (SpawnRoutine());
	}

	// TODO use this for the background as well
	IEnumerator SpawnRoutine () {
		// instantiate a random object at a random spawnpoint
		var original = objectsToSpawn [Random.Range (0, objectsToSpawn.Length)];
		var position = spawns [Random.Range (0, spawns.Length)].transform.position;
		Instantiate (original, position, Quaternion.identity);

		//wait and loop
		yield return new WaitForSeconds (spawnRate);
		StartCoroutine (SpawnRoutine());
	}
}
