using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

	public float spawnRate = 2.0869564f;
	public GameObject[] objectsToSpawn;
	public Material hotMaterial;
	public Material[] backMaterials;
	public GameObject[] spawns;
	public GameObject[] bgObjectsToSpawn;
	public GameObject[] spawnsBG1;
	public GameObject[] spawnsBG2;

	
	void Start () {
		spawnRate = GameController.tempo * 2;

		if (objectsToSpawn.Length < 1 || spawns.Length < 1 || bgObjectsToSpawn.Length < 1 || spawnsBG1.Length < 1)
		{
			Debug.LogError("Assign GameObjects...");
		}
		else
		{
			StartCoroutine (DelaySpawnRoutine());
			StartCoroutine (SpawnBG1Routine());
			StartCoroutine (SpawnBG2Routine());
		}
	}

	IEnumerator DelaySpawnRoutine ()
	{
		yield return new WaitForSeconds (3f);
		StartCoroutine (SpawnRoutine());
	}

	// TODO use this for the background as well
	IEnumerator SpawnRoutine ()
	{
		// instantiate a random object at a random spawnpoint
		var original = objectsToSpawn [Random.Range (0, objectsToSpawn.Length)];
		var position = spawns [Random.Range (0, spawns.Length)].transform.position;
		GameObject obj = (GameObject)Instantiate (original, position, Quaternion.identity);
		obj.rigidbody2D.AddTorque (Random.value * 4);
		obj.renderer.material = hotMaterial;

		//wait and loop
		yield return new WaitForSeconds (spawnRate);
		StartCoroutine (SpawnRoutine());
	}

	// spawn en masse i baggrunden
	IEnumerator SpawnBG1Routine ()
	{
		// instantiate a random object at a random spawnpoint
		var original = bgObjectsToSpawn [Random.Range (0, bgObjectsToSpawn.Length)];
		var position = spawnsBG1 [Random.Range (0, spawnsBG1.Length)].transform.position;
		GameObject obj = (GameObject)Instantiate (original, position, Quaternion.identity);
		obj.rigidbody2D.AddTorque (Random.value * 4);
		obj.renderer.material = backMaterials[Random.Range(0,backMaterials.Length)];

		//wait and loop
		yield return new WaitForSeconds (spawnRate);
		StartCoroutine (SpawnBG1Routine());
	}

	// spawn en masse i baggrunden
	IEnumerator SpawnBG2Routine ()
	{
		// instantiate a random object at a random spawnpoint
		var original = bgObjectsToSpawn [Random.Range (0, bgObjectsToSpawn.Length)];
		var position = spawnsBG2 [Random.Range (0, spawnsBG2.Length)].transform.position;
		GameObject obj = (GameObject)Instantiate (original, position, Quaternion.identity);
		obj.rigidbody2D.AddTorque (Random.value * 4);
		obj.renderer.material = backMaterials[Random.Range(0,backMaterials.Length)];

		//wait and loop
		yield return new WaitForSeconds (spawnRate);
		StartCoroutine (SpawnBG2Routine());
	}

}
