using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class GooglePlayGamesController : MonoBehaviour {
	private const float FontSizeMult = 0.05f;
	private bool mWaitingForAuth = false;

	void Start () {
		// Select the Google Play Games platform as our social platform implementation
		GooglePlayGames.PlayGamesPlatform.Activate();
	}
	
	public void SignIn() {
		if (mWaitingForAuth) {
			return;
		}

		if (!Social.localUser.authenticated) {
			// Authenticate
			mWaitingForAuth = true;
			Social.localUser.Authenticate((bool success) => {
				mWaitingForAuth = false;
			});
			// TODO add exiting animation that shows sign in process

		} else {
			// Sign out!
			((GooglePlayGames.PlayGamesPlatform) Social.Active).SignOut();
		}
	}
}
