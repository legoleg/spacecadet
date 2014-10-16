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
	public Image[] hearts;
	public int points = 0;
	// UI
	public Button buttonLeft;
	public Button buttonRight;
	public Text pointsTxt;
	public Text multiplierTxt;
	// Music
	public AudioClip inGameMusic;
	public AudioClip pauseMusic;
	Music music;

	float fadingTime;

	Ship ship;
	float hitTime;
		

	void Start ()
	{
		music = GameObject.Find ("Music").GetComponent<Music>();
		ship = GameObject.Find ("Ship").GetComponent<Ship>();

		points = 0;
		Time.timeScale = 1f;
		fadingTime = tempo * 3 - .1f;

		var fadeTime  = tempo * 2 - .1f;

		// disable UI elements that slide in on the screen
		pointsTxt.enabled = false;
		buttonLeft.image.enabled = false;
		buttonRight.image.enabled = false;

		foreach (Image heart in hearts) {
			heart.enabled = false;
		}
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
		foreach (Image heart in hearts) {
			yield return new WaitForSeconds (.1f);
			heart.enabled = true;
			iTween.MoveFrom(heart.gameObject, iTween.Hash(
				"y", -50f
				, "time", tempo
				, "easetype", iTween.EaseType.easeOutBack
				));
		}

		// Point text slides in
		yield return new WaitForSeconds (.1f);
		pointsTxt.enabled = true;
		iTween.MoveFrom(pointsTxt.gameObject, iTween.Hash(
			"x", Screen.width
			, "time", tempo
			, "easetype", iTween.EaseType.easeOutBack
			));

		// Left and Right Buttons slides in
		yield return new WaitForSeconds (.1f);

		buttonLeft.image.enabled = true;
		iTween.MoveFrom(buttonLeft.gameObject, iTween.Hash(
			"x", -96
			, "time", tempo
			, "easetype", iTween.EaseType.easeOutBack
			));

		buttonRight.image.enabled = true;
		iTween.MoveFrom(buttonRight.gameObject, iTween.Hash(
			"x", 96+Screen.width
			, "time", tempo
			, "easetype", iTween.EaseType.easeOutBack
			));

		ship.canMove = true;
		ship.canShoot = true;
	}

	public void AddPoints (int i) {
		StartCoroutine(PointRoutine (i));

	}

	IEnumerator PointRoutine (int i)
	{
		// Set the time that points was added and give an award two targets are hit at the same time
		if (Time.time - hitTime <= ship.fireRate) {
			Debug.Log ((Time.time - hitTime).ToString () + " <=" + ship.fireRate.ToString ());
			// Why 160? https://twitter.com/_legoleg/status/519207539603681280 @korumellis digs it on Mars One?
			points += 160;
			TweenGameObject (pointsTxt.gameObject, 50f, tempo * 2);
			iTween.MoveBy (multiplierTxt.gameObject, iTween.Hash ("easetype", iTween.EaseType.easeInOutExpo, "y", -350, "time", 4f));
			yield return new WaitForSeconds (.5f);
			iTween.MoveBy (multiplierTxt.gameObject, iTween.Hash ("easetype", iTween.EaseType.easeInOutExpo, "y", 350, "time", 4f));
		}
		else {
			points += i;
			TweenGameObject (pointsTxt.gameObject, 25f, tempo);
		}
		yield return new WaitForSeconds (.5f);
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

		StartCoroutine (MoveUIToLosePosition());
		StartCoroutine (FadeOut());

		// Send fading time scale to ScaleTime
		iTween.ValueTo (gameObject, iTween.Hash (
			"from", 1f
			,"to", 0f
			,"time", fadingTime
			,"easetype", iTween.EaseType.easeInOutSine
			,"ignoretimescale", true
			,"onupdatetarget", gameObject
			,"onupdate", "ScaleTime"
			,"onCompleteTarget",gameObject
			,"oncomplete", "Restart"
			));
	}

	IEnumerator MoveUIToLosePosition ()
	{
		// Left and Right Buttons slides in
		yield return new WaitForSeconds (.1f);
		
		iTween.MoveTo(buttonLeft.gameObject, iTween.Hash(
			"x", -96
			, "time", tempo
			, "easetype", iTween.EaseType.easeOutBack
			));
		buttonLeft.image.enabled = false;
		
		iTween.MoveTo(buttonRight.gameObject, iTween.Hash(
			"x", 96+Screen.width
			, "time", tempo
			, "easetype", iTween.EaseType.easeOutBack
			));
		buttonRight.image.enabled = false;

		// Point text slides in
		yield return new WaitForSeconds (.1f);
		iTween.MoveTo(pointsTxt.gameObject, iTween.Hash(
			"y", Screen.height/2
			, "time", tempo
			, "easetype", iTween.EaseType.easeOutBack
			));

	}
	
	IEnumerator FadeOut ()
	{
		iTween.CameraFadeAdd(cameraTexture);
		iTween.CameraFadeTo(iTween.Hash (
			"amount", 1f, 
			"time", fadingTime, 
			"ignoretimescale", true));
		yield return new WaitForSeconds (fadingTime);
		iTween.CameraFadeDestroy();
	}
	
	void ScaleTime (float timeFactor)
	{
		Time.timeScale = timeFactor;
		music.audio.pitch = timeFactor;
	}
	
	public void Restart ()
	{
		Time.timeScale = 1f;
		Application.LoadLevel("menu");
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
