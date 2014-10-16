using UnityEngine;
using System.Collections;

public class HeyZapAdsController : MonoBehaviour {

	void Start ()
	{
		// Your Publisher ID is: 73b1a9ddfd9ccc624275f5258d2ed465
		HeyzapAds.start("73b1a9ddfd9ccc624275f5258d2ed465", HeyzapAds.FLAG_NO_OPTIONS);
		HZInterstitialAd.show();
	}

	public static IEnumerator AdRoutine ()
	{
		// Disable game input
		ButtonController.showingAd = true;
		// Dim the game
		iTween.CameraFadeAdd();
		iTween.CameraFadeTo (1f, 1f);
		// wait a while
		yield return new WaitForSeconds (1f);
		// Show the ad
		HZInterstitialAd.show();
		// wait a while
		yield return new WaitForSeconds (3f);
		// Hide the ad
		HZInterstitialAd.hide();
		// Undim the game
		iTween.CameraFadeTo (0f, .5f);
		yield return new WaitForSeconds (.5f);
		iTween.CameraFadeDestroy();
		// Enable input
		ButtonController.showingAd = false;
	}
}
