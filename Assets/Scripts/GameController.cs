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
	public float tempo = .666f;
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

	void Update ()
	{
		pointsTxt.text = points.ToString();
	}
	
	IEnumerator FadeIn (float fadeTime)
	{
		iTween.CameraFadeAdd(cameraTexture);
		iTween.CameraFadeFrom (1f, fadeTime);
		yield return new WaitForSeconds (fadeTime);
		iTween.CameraFadeDestroy();

		// tween heart from invisible to visible positions
		foreach (RectTransform heart in hearts) {
			yield return new WaitForSeconds (.1f);
			iTween.MoveTo(heart.gameObject, iTween.Hash(
				"y", 50f
				, "time", tempo
				, "easetype", iTween.EaseType.easeOutBack
				));
		}
		
		yield return new WaitForSeconds (.2f);
		pointsTxt.enabled = true;
		iTween.MoveFrom(pointsTxt.gameObject, iTween.Hash(
			"position", pointsTxt.transform.position + Vector3.up
			, "time", fadeTime
			, "easetype", iTween.EaseType.easeInOutBack
			));

		// TODO animate control buttons

		ship.canMove = true;
		ship.canShoot = true;
	}

	public void AddPoints (int i) {
		// Set the time that points was added and give an award two targets are hit at the same time
		if (Time.time - hitTime <= ship.fireRate) {
			Debug.Log((Time.time-hitTime).ToString() + " <=" + ship.fireRate.ToString());

			// Why 160? https://twitter.com/_legoleg/status/519207539603681280 @korumellis digs it on Mars One?
			points += 160;
			TweenGameObject(pointsTxt.gameObject, 50f, tempo * 2);
			TweenGameObject(multiplierTxt.gameObject, -350f, 5f);
		} else {
			points += i;
			TweenGameObject(pointsTxt.gameObject, 25f, tempo);
		}

		hitTime = Time.time;
	}

	public void LoseHeart () {
		lives--;

		for (int i = 0; i < hearts.Length; i++) {
			if (i >= lives) {
				iTween.MoveTo(hearts[i].gameObject, iTween.Hash(
					"y", -50
					, "time", tempo
					, "easetype", iTween.EaseType.easeInBack
					));
			}
		}

		if (lives <= 0) {
			Lose ();
		}
	}

	void AnimateMultilpierText (float timeFactor) {
		multiplierTxt.rectTransform.anchoredPosition = new Vector2(multiplierTxt.rectTransform.anchoredPosition.x, timeFactor);
	}

	public void Lose () {
		ship.canShoot = false;
		ship.canMove = false;

		StartCoroutine (FadeOut());

		// Send fading time scale to ScaleTime
		iTween.ValueTo (gameObject, iTween.Hash (
			"from", 1f
			,"to", 0f
			,"time", tempo * 3
			,"easetype", iTween.EaseType.easeInOutSine
			,"ignoretimescale", true
			,"onupdatetarget", gameObject
			,"onupdate", "ScaleTime"
			,"onCompleteTarget",gameObject
			,"oncomplete", "Restart"
			));
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
	
	void ScaleTime (float timeFactor)
	{
		Time.timeScale = timeFactor;
		music.audio.pitch = timeFactor;
	}
	
	public void Restart ()
	{
		Application.LoadLevel(0);
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
