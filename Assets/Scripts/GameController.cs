using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using Facebook;
using System.Collections.Generic;
//using Parse;
//using System.Threading.Tasks;

public class GameController : MonoBehaviour
{
	public Texture2D cameraTexture;
	// set this to match the BMP of the music: 0.5 = 120 bpm, 1 = 60 bpm, 2 = 30 bpm
	public static float tempo = .5f;
	public int lives = 3;
	public RectTransform[] hearts;
	public int points = 0;
	// UI
	public Text pointsTxt;
	public Text multiplierTxt;
	// Music
	public AudioClip inGameMusic;
	public AudioClip pauseMusic;
	Music music;

	Ship ship;
	float hitTime;

	

	void Start ()
	{
		music = GameObject.Find ("Music").GetComponent<Music>();
		ship = GameObject.Find ("Ship").GetComponent<Ship>();

		points = 0;
		Time.timeScale = 1f;
		
		var fadeTime  = tempo * 2 - .1f;
		StartCoroutine(FadeIn(fadeTime));
		
		// fade in music 
		music.gameObject.audio.clip = inGameMusic;
		music.FadeIn (tempo * 4);
		music.audio.Play ();
	}

	IEnumerator FadeIn (float fadeTime)
	{
		iTween.CameraFadeAdd(cameraTexture);
		iTween.CameraFadeFrom (1f, fadeTime);
		yield return new WaitForSeconds (fadeTime);
		iTween.CameraFadeDestroy();

		// TODO animate heart from invisible to visible states
		foreach (RectTransform heart in hearts) {
			yield return new WaitForSeconds (.1f);
			heart.GetComponent<Animator>().SetBool("visible", true);
		}
		
		yield return new WaitForSeconds (.2f);
		pointsTxt.enabled = true;
		iTween.MoveFrom(pointsTxt.gameObject, iTween.Hash(
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
		pointsTxt.text = points.ToString();
	}

	public void AddPoints (int i)
	{
		//set time point was added and double points if time since last point was added == Ship.fireRate
		if (Time.time-hitTime <= ship.fireRate) {
//			Debug.Log((Time.time-hitTime).ToString() + " <=" + ship.fireRate.ToString());
			//double the points
			points *= 2;
			//TODO visualize multiplier
			TweenGameObject(pointsTxt.gameObject, -.5f, tempo * 4);
			TweenGameObject(multiplierTxt.gameObject, -.7f, tempo * 8);
		}
		else 
		{
			points += i;
			TweenGameObject(pointsTxt.gameObject, .1f, tempo);
		}

		hitTime = Time.time;
	}

	void ScaleTime (float timeFactor)
	{
		Time.timeScale = timeFactor;
		music.audio.pitch = timeFactor;
	}

	public void HideNextHeart () {
		hearts[lives].GetComponent<Animator>().SetBool("visible", false);
	}
	
	public void Lose () {
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
		Application.LoadLevel(0);

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
	
	public void TweenGameObject (GameObject obj, float amount, float time)
	{
		iTween.PunchPosition (obj, iTween.Hash (
			"easetype", iTween.EaseType.easeInOutBack
			,"y", amount
			,"time", time
			));
	}
}
