using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class GameController : MonoBehaviour
{
	// This is for fading in
	public Texture2D cameraTexture;

	// set this to match the BMP of the music: 0.5 = 4/4 or 120 bpm, 2/4 or 1 = 60 bpm, 2 = 1/4 or 30 bpm, 0.66667 = 3/4
	public float tempo = 1f;
	private int lives = 3;
	public Image[] hearts;
	public static int points = 0;

	// UI
	public Text pointsTxt;
	public Text pointsTxtCentered;

	// Music
	public AudioClip inGameMusic;
	public AudioClip pauseMusic;
	private Music musicComponent;
	private Ship shipComponent;

	public GameObject music;
	public GameObject killZone;
	public GameObject ship;

	float fadeOutTime = 8f;
		

	void Start ()
	{
		// Initialise music
		musicComponent = music.GetComponent<Music>();
		// fade in music 
		musicComponent.gameObject.audio.clip = inGameMusic;
		musicComponent.FadeIn (tempo * 4);
		musicComponent.audio.Play ();

		// Initialise ship
		shipComponent = ship.GetComponent<Ship>();
		shipComponent.canMove = true;

		points = 0;

		Time.timeScale = 1f;
		fadeOutTime = fadeOutTime * tempo - .1f;
		var fadeInTime  = tempo;
		StartCoroutine(FadeIn(fadeInTime));

		// disable UI elements that slide in on the screen
//		pointsTxt.enabled = false;

		foreach (Image heart in hearts) {
			heart.enabled = false;
		}
	}

	void Update ()
	{
		// Displaying the points with the specified thousand-separator. Thanks to http://stackoverflow.com/a/752167/229507
		NumberFormatInfo numberFormatInfo = (NumberFormatInfo) CultureInfo.InvariantCulture.NumberFormat.Clone();
		numberFormatInfo.NumberGroupSeparator = " ";

		pointsTxt.text = System.Convert.ToInt32(points).ToString("N0", numberFormatInfo);
	}

	IEnumerator FadeIn (float fadeTime)
	{
		iTween.CameraFadeAdd(cameraTexture);
		iTween.CameraFadeFrom (1f, fadeTime);
		yield return new WaitForSeconds (fadeTime);
		iTween.CameraFadeDestroy();

		// Tween the hearts from invisible to visible positions
		foreach (Image heart in hearts) {
			heart.enabled = true;
			iTween.MoveFrom(heart.gameObject, iTween.Hash(
				"x", Screen.width + Screen.width * 0.2f
				, "time", tempo
				, "easetype", iTween.EaseType.easeOutBack
				));
			yield return new WaitForSeconds (.1f);
		}

		// Point text slides in
		yield return new WaitForSeconds (.1f);
		pointsTxt.enabled = true;
		iTween.MoveFrom(pointsTxt.gameObject, iTween.Hash(
			"x", -32
			, "time", tempo
			, "easetype", iTween.EaseType.easeOutBack
			));
	}

	public void AddPoints (int i)
	{
		StartCoroutine(PointRoutine (i));
	}

	IEnumerator PointRoutine (int i)
	{
		points += i;
		iTween.PunchPosition (pointsTxt.gameObject, iTween.Hash (
			"easetype", iTween.EaseType.easeInOutBack
			,"y", 25f
			,"time", tempo
			));
		yield return new WaitForSeconds (.5f);
	}

	public void LoseHeart ()
	{
		lives--;
		if (lives <= 0) {
			Lose ();
		}

		for (int i = 0; i < hearts.Length; i++) {
			if (i >= lives) {
				iTween.MoveTo(hearts[i].gameObject, iTween.Hash(
					"x", Screen.width + Screen.width * 0.2f
					, "time", tempo
					, "easetype", iTween.EaseType.easeInBack
					));
				StartCoroutine(DeactivateGameObjectAfterSeconds (hearts[i].gameObject, tempo));
			}
		}
	}

	IEnumerator DeactivateGameObjectAfterSeconds (GameObject gameObject, float seconds)
	{
		yield return new WaitForSeconds (seconds);
		gameObject.SetActive (false);
	}

	public void Lose ()
	{
		shipComponent.canShoot = false;
		shipComponent.canMove = false;
		shipComponent.GetComponentInChildren<Animator>().SetBool("Lost", true);
		ship.collider2D.enabled = false;
		ship.GetComponent<FollowFinger>().enabled = false;
		killZone.SetActive(false);

		StartCoroutine (MoveUIToLosePosition());
		StartCoroutine (FadeOut());

		// Send fading time scale to ScaleTime
		iTween.ValueTo (gameObject, iTween.Hash (
			"from", 1f
			,"to", 0f
			,"time", fadeOutTime
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
		// Point text slides in
		yield return new WaitForSeconds (.1f);
		pointsTxt.gameObject.SetActive(false);
		pointsTxtCentered.gameObject.SetActive(true);

		// Displaying the points with the specified thousand-separator. Thanks to http://stackoverflow.com/a/752167/229507
		NumberFormatInfo numberFormatInfo = (NumberFormatInfo) CultureInfo.InvariantCulture.NumberFormat.Clone();
		numberFormatInfo.NumberGroupSeparator = " ";
		pointsTxtCentered.text = System.Convert.ToInt32(points).ToString("N0", numberFormatInfo);

		iTween.MoveTo(pointsTxtCentered.gameObject, iTween.Hash(
			"y", Screen.height * .5f
			,"time", tempo
			,"easetype", iTween.EaseType.easeOutBack
			));
	}
	
	IEnumerator FadeOut ()
	{
		iTween.CameraFadeAdd(cameraTexture);
		iTween.CameraFadeTo(iTween.Hash (
			"amount", 1f, 
			"time", fadeOutTime, 
			"ignoretimescale", true));
		yield return new WaitForSeconds (fadeOutTime);
		iTween.CameraFadeDestroy();
	}
	
	void ScaleTime (float timeFactor)
	{
		Time.timeScale = timeFactor;
		musicComponent.audio.pitch = timeFactor;
	}
	
	void Restart ()
	{
		Time.timeScale = 1f;
		Application.LoadLevel("menu");
	}
}
