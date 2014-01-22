using UnityEngine;
using System.Collections;

[RequireComponent(typeof (AudioSource))]
public class Music : MonoBehaviour {

	public float pitch;
	public float volume;

	public void FadeIn (float time)
	{
		iTween.AudioTo (gameObject, volume, pitch, time);
	}

	public void FadeOut (float time)
	{
		iTween.AudioTo (gameObject, 0f, 0f, time);
	}
}
