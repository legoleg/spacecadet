using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour
{
	float spawnRate;
	public GameObject[] objectsToSpawn;
	public GameObject[] spawns;
	public static bool canSpawn = false;

	void Start ()
	{
		canSpawn = false;
		var tempo = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().tempo;
		spawnRate = tempo;

		if (objectsToSpawn.Length < 1 || spawns.Length < 1) {
			Debug.LogError ("Assign some GameObjects to be spawned.");
		} else {
			StartCoroutine (SpawnRoutine ());
		}
	}

	// TODO use this for the background as well
	// TODO Spawn using animation events
	IEnumerator SpawnRoutine ()
	{
		if (canSpawn) {
			// instantiate a random object at a random spawnpoint
			var original = objectsToSpawn [Random.Range (0, objectsToSpawn.Length)];
			var position = spawns [Random.Range (0, spawns.Length)].transform.position;
			Instantiate (original, position, Quaternion.identity);
		}
		//wait and loop
		yield return new WaitForSeconds (spawnRate);
		StartCoroutine (SpawnRoutine ());
	}
}
