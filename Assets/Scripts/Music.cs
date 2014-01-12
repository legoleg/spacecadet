using UnityEngine;
using System.Collections;


[RequireComponent(typeof (AudioSource))]
public class Music : MonoBehaviour {

	public static GameObject go;
	public float pitch;
	public float volume;

	void Start ()
	{
		go = gameObject;
		FadeIn (.5f);
	}

	public void FadeIn (float time)
	{
		iTween.AudioTo (go, volume, pitch, time);
		go.audio.Play ();
	}

	public void FadeOut (float time)
	{
		iTween.AudioTo (go, 0f, 0f, time);
	}
}
