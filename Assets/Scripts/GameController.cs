using UnityEngine;
using System.Collections;
//using Facebook;
using System.Collections.Generic;
//using Parse;
//using System.Threading.Tasks;

public class GameController : MonoBehaviour
{
	public static bool playing = false;
	public static bool done = false;
	public int lives = 3;
	public GameObject[] hearts;
	public int points = 0;
	// display
	public GameObject pointsTxt;

	public AudioClip inGameMusic;
	public AudioClip pauseMusic;

	public float timeLimit = 120f;
	private float timeLeft;
//	public GameObject loadLvlBtn;

	Music music;
	SpawnController spawnController;
	

	void Start ()
	{
		points = 0;
		playing = false;
		done = false;
		spawnController = GameObject.Find ("SpawnController").GetComponent<SpawnController> ();
		music = GameObject.Find ("Music").GetComponent<Music>();
		// fade in post-game music
		music.gameObject.audio.clip = inGameMusic;
		music.FadeIn(Ship.fireRate*4);
		music.audio.Play();
	}

	void Update ()
	{
		pointsTxt.guiText.text = points.ToString();

		for (int i = 0; i < hearts.Length; i++)
		{
			if (i >= lives) {
				iTween.MoveUpdate(hearts[i], iTween.Hash(
					"position", hearts[i].transform.position + Vector3.down, 
					"time", 5f,
					"easetype", iTween.EaseType.easeInOutElastic
					));
			}
		}
	}

	void ScaleTime (float timeFactor)
	{
		Time.timeScale = timeFactor;
		music.audio.pitch = timeFactor;
	}
	
	public void Lose ()
	{
		iTween.ValueTo (gameObject, iTween.Hash (
			"from", 1f,
			"to", 0f,
			"time", spawnController.spawnRate * Mathf.PI,
			"easetype", iTween.EaseType.easeInQuart,
			"onupdatetarget", gameObject, 
			"onupdate", "ScaleTime",
			"ignoretimescale", true));
	}
	
	public void TweenGameObject (GameObject obj)
	{
		iTween.PunchPosition (obj, iTween.Hash ("y", -.05f, "time", .9f));
	}

	public IEnumerator GameTimer ()
	{
		// fade in in-game music
//		music.gameObject.audio.clip = inGameMusic;
//		music.FadeIn(.5f);

		playing = true;
		yield return new WaitForSeconds (timeLimit);

		playing = false;
		done = true;
		// move box back and fade music
		var go = GameObject.FindGameObjectWithTag ("Player");
		iTween.MoveTo (go, iTween.Hash (
			"position", new Vector3 (0, 0f, -7f), 
			"easetype", iTween.EaseType.easeInOutBack, 
			"time", 1f));
		music.FadeOut(.9f);
		yield return new WaitForSeconds (1f);

		// fade in post-game music
		music.gameObject.audio.clip = pauseMusic;
		music.FadeIn(.5f);

		yield return new WaitForSeconds (.5f);


		// show button
//		loadLvlBtn.GetComponent<LoadLevelButton>().Show(.1f);
		yield return new WaitForSeconds (pauseMusic.length + .5f);

		// load main menu
		Application.LoadLevel ("mainMenu");
	}

}
