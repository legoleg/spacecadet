using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class GooglePlayGamesController : MonoBehaviour {
	private const float FontSizeMult = 0.05f;
	private bool mWaitingForAuth = false;
	public Sprite signedIn, waiting, signedOut;

	void Start () {
		// Select the Google Play Games platform as our social platform implementation
		GooglePlayGames.PlayGamesPlatform.Activate();
		GetComponent<Button>().image.sprite = signedOut;
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
			// TODO add exiting animation that shows sign in process
			GetComponent<Button>().image.sprite = waiting;
			Social.localUser.Authenticate((bool success) => {
				mWaitingForAuth = false;
				GetComponent<Button>().image.sprite = signedIn;
			});
		} 
//		else {
//			// Sign out!
//			((GooglePlayGames.PlayGamesPlatform) Social.Active).SignOut();
//			GetComponent<Button>().image.sprite = signedOut;
//		}
	}
}
