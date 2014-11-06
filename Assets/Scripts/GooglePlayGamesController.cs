using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class GooglePlayGamesController : MonoBehaviour {
	private const float FontSizeMult = 0.05f;
	private bool mWaitingForAuth = false;
	public Sprite signedInSprite, waitingSprite, signedOutSprite;

	void Start () {
		// Select the Google Play Games platform as our social platform implementation
		GooglePlayGames.PlayGamesPlatform.Activate();
		if (signedOutSprite) {
			GetComponent<Button>().image.sprite = signedOutSprite;
		}
	}

	void Update () {
		SignIn();
	}
	
	public void SignIn() {
		if (mWaitingForAuth) {
			return;
		}

		if (!Social.localUser.authenticated) {
			// Authenticate
			mWaitingForAuth = true;
			Debug.Log("Signing in to Google Play Games...");
			if (waitingSprite) {
				// TODO add exiting animation that shows sign in process
				GetComponent<Button>().image.sprite = waitingSprite;
			}
			Social.localUser.Authenticate((bool success) => {
				Debug.Log("Signed in to Google Play Games.");
				mWaitingForAuth = false;
				if (signedInSprite) {
					GetComponent<Button>().image.sprite = signedInSprite;
				}
			});
		} 
		else {
//			// Sign out!
//			((GooglePlayGames.PlayGamesPlatform) Social.Active).SignOut();
//			GetComponent<Button>().image.sprite = signedOut;
		}
	}
}
