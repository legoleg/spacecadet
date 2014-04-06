using UnityEngine;
using System.Collections;
//using Facebook;
using System.Collections.Generic;
//using Parse;
//using System.Threading.Tasks;

public class GameController : MonoBehaviour
{
	public Texture2D cameraTexture;
	// set this to match the BMP of the music: 0.5 = 120 bpm, 1 = 60 bpm, 2 = 30 bpm
	public static float tempo = 0.5217391f;
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

	SpawnController spawnController;
	Music music;
	Ship ship;	

	void Start ()
	{
		// find friends
		spawnController = GameObject.Find ("SpawnController").GetComponent<SpawnController> ();
		music = GameObject.Find ("Music").GetComponent<Music>();
		ship = GameObject.Find ("Ship").GetComponent<Ship>();

		foreach (GameObject heart in hearts) 
		{
			heart.guiTexture.enabled = false;
		}
		pointsTxt.guiText.enabled = false;
		
		points = 0;
		Time.timeScale = 1f;
		
		var fadeTime  = tempo * 2 - .1f;
		StartCoroutine(FadeIn(fadeTime));
		
		// fade in music 
		music.gameObject.audio.clip = inGameMusic;
		music.FadeIn (tempo * 4);
		music.audio.Play ();
		// TODO: change music when not playing
	}

	IEnumerator FadeIn (float fadeTime)
	{
		iTween.CameraFadeAdd(cameraTexture);
		iTween.CameraFadeFrom (1f, fadeTime);
		yield return new WaitForSeconds (fadeTime);
		iTween.CameraFadeDestroy();

		foreach (GameObject heart in hearts) 
		{
			yield return new WaitForSeconds (.1f);
			heart.guiTexture.enabled = true;
			iTween.MoveFrom(heart, iTween.Hash(
				"position", heart.transform.position + Vector3.down, 
				"time", fadeTime,
				"easetype", iTween.EaseType.easeInOutBack
				));
		}
		
		yield return new WaitForSeconds (.2f);
		pointsTxt.guiText.enabled = true;
		iTween.MoveFrom(pointsTxt, iTween.Hash(
			"position", pointsTxt.transform.position + Vector3.up, 
			"time", fadeTime,
			"easetype", iTween.EaseType.easeInOutBack
			));

		// TODO animate control buttons

		ship.canMove = true;
		ship.canShoot = true;
	}

	IEnumerator FadeOut ()
	{
		var fadeTime = tempo * 4 - .1f;

		iTween.CameraFadeAdd(cameraTexture);
				iTween.CameraFadeTo(iTween.Hash (
			"amount", 1f, 
			"time", fadeTime, 
			"ignoretimescale", true));
		yield return new WaitForSeconds (fadeTime);
		iTween.CameraFadeDestroy();
	}

	void Update ()
	{
		pointsTxt.guiText.text = points.ToString();

		// TODO move to where the points are subtracted
		for (int i = 0; i < hearts.Length; i++)
		{
			if (i >= lives) {
				iTween.MoveUpdate(hearts[i], iTween.Hash(
					 "position", hearts[i].transform.position + Vector3.up
					,"time", tempo * 6 - .1f
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
		ship.canShoot = false;
		ship.canMove = false;

		StartCoroutine (FadeOut());

		iTween.ValueTo (gameObject, iTween.Hash (
			"from", 1f,
			"to", 0f,
			"time", tempo * 2,
			"easetype", iTween.EaseType.easeInOutSine,
			"ignoretimescale", true,
			"onupdatetarget", gameObject, 
			"onupdate", "ScaleTime",
			"onCompleteTarget",gameObject,
			"oncomplete", "Restart"));
	}
	
	public void Restart ()
	{
		Application.LoadLevel(Application.loadedLevel);

//		iTween.CameraFadeDestroy ();
//		iTween.CameraFadeAdd ();
//		iTween.CameraFadeFrom (iTween.Hash (
//			"amount", 1f, 
//			"time", tempo - .1f, 
//			"ignoretimescale", true));
//
//		iTween.ValueTo (gameObject, iTween.Hash (
//			"from", 0f,
//			"to", 1f,
//			"time", tempo,
//			"easetype", iTween.EaseType.easeInOutSine,
//			"ignoretimescale", true,
//			"onupdatetarget", gameObject, 
//			"onupdate", "ScaleTime",
//			"onCompleteTarget",gameObject,
//			"oncomplete", "Init"));
	}
	
	public void TweenGameObject (GameObject obj)
	{
		iTween.PunchPosition (obj, iTween.Hash (
			"y", .1f, 
			"easetype", iTween.EaseType.easeInOutBack,
			"time", tempo));
	}
}
