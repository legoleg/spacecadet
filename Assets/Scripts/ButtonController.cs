using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {

	public Rect btnRect;
	public GameObject btn;
	public Sprite btnUp;
	public Sprite btnDown;
	public AudioClip secretClip;
	public AudioClip btnSound;
	public AudioClip[] audioClips;
	public Object particle;

	public float blinkBackgroundRate = .1f;
	Color backgroundColor;
	Counter counter;

	public static bool showingAd = false;

	void Start ()
	{
		backgroundColor = Camera.main.backgroundColor;
		counter = GetComponent<Counter>();
		audio.clip = secretClip;
	}

	void PlaySoundWithButton (bool isRandom)
	{
		if (isRandom)
		{
			if (Random.Range(0, 99) < 3 && !audio.isPlaying)
			{
				audio.Play();
				StartCoroutine (BlinkBackgroundColor());
				Debug.Log("Secret clip was chosen...");
			}
			else
			{
				Camera.main.audio.clip = audioClips [Random.Range (0, audioClips.Length)];
				Debug.Log("Clip" + Camera.main.audio.clip.name + "was chosen...");
			}
			Camera.main.audio.Play ();
		}
		else
		{
			Camera.main.audio.PlayOneShot (btnSound);
		}
	}

	void ChangeButtonSpriteTo (Sprite sprite)
	{
		btn.GetComponent<SpriteRenderer>().sprite = sprite;
		Debug.Log("Sprite was set: " + sprite.name);
	}

	IEnumerator BlinkBackgroundColor ()
	{
		Camera.main.backgroundColor = new Color (Mathf.Round(Random.value*2)/2, Mathf.Round(Random.value*2)/2, Mathf.Round(Random.value*2)/2); //backgrounColors[Random.Range(0, backgrounColors.Length)];
		Debug.Log("Color was chosen: " + Camera.main.backgroundColor.ToString());
		yield return new WaitForSeconds (blinkBackgroundRate);
		StartCoroutine (BlinkBackgroundColor());
	}

	public void BtnDown (Vector2 position)
	{
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint(position);
		Vector2 touchPoint = new Vector2(worldPoint.x, worldPoint.y);
		if (collider2D != Physics2D.OverlapPoint(touchPoint))
		{
			ChangeButtonSpriteTo (btnDown);
			PlaySoundWithButton (false);
			StartCoroutine (BlinkBackgroundColor ());
		}
	}

	public void BtnUp (Vector2 position)
	{
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint(position);
		Vector2 touchPoint = new Vector2(worldPoint.x, worldPoint.y);
		if (collider2D != Physics2D.OverlapPoint(touchPoint))
		{
			ChangeButtonSpriteTo (btnUp);
			PlaySoundWithButton (true);
			StopAllCoroutines ();
			GameObject p = (GameObject)Instantiate (particle, transform.position, Quaternion.identity);
			p.particleSystem.startColor = Camera.main.backgroundColor;
			Destroy (p, 3f);
			Camera.main.backgroundColor = backgroundColor;
			counter.AddOne();
		}
	}

	void Update ()
	{
		if (showingAd) {
			return;
		}

		if (Input.touchCount > 0)
		{
			Touch t = Input.GetTouch(0);
			if (t.phase == TouchPhase.Began)
			{
				BtnDown (Input.GetTouch (0).position);
			}
			if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
			{
				BtnUp (Input.GetTouch (0).position);
			}
		}

		// debugging
		if (Input.GetButtonDown("Fire1"))
		{
			BtnDown(Input.mousePosition);
		}
		if (Input.GetButtonUp("Fire1"))
		{
			BtnUp(Input.mousePosition);
		}
	}
}
