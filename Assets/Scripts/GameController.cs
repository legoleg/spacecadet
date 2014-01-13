
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

	public int points = 0;
	// display
	public GameObject pointsTxt;

	public AudioClip inGameMusic;
	public AudioClip pauseMusic;

	public float timeLimit = 120f;
	private float timeLeft;
//	public GameObject loadLvlBtn;

	Music music;	

	void Start ()
	{
		points = 0;
		playing = false;
		done = false;
		music = GameObject.Find ("Music").GetComponent<Music>();
		// fade in post-game music
		music.gameObject.audio.clip = inGameMusic;
		music.FadeIn(Ship.fireRate*4);
		music.audio.Play();
	}

	void Update ()
	{
		pointsTxt.guiText.text = points.ToString();
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
