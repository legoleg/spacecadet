using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {

	void Start ()
	{
		Time.timeScale = 1f;
		// Your Publisher ID is: 73b1a9ddfd9ccc624275f5258d2ed465
//		HeyzapAds.start("73b1a9ddfd9ccc624275f5258d2ed465", HeyzapAds.FLAG_NO_OPTIONS);
	}

	public void LoadSceneByName (string scene)
	{
//		HZInterstitialAd.show();
		Application.LoadLevel(scene);
	}

	public void LoadNextScene ()
	{
//		HZInterstitialAd.show();
		Application.LoadLevel(Application.loadedLevel + 1);
	}
}
