using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Globalization;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

/// <summary>
/// Stats.
/// Save statistics for score, swipes, deaths, improves, 
/// </summary>
public class Stats : MonoBehaviour 
{
	private static Stats _instance;
	
	public static Stats instance {
		get
		{
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType<Stats>();
				//Tell unity not to destroy this object when loading a new scene!
				DontDestroyOnLoad(_instance.gameObject);
			}
			
			return _instance;
		}
	}

	public static int points = 0;
	private Text hiScoreNumber;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		if(_instance == null) {
			//If I am the first instance, make me the Singleton
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else {
			//If a Singleton already exists and you find another reference in scene, destroy it!
			if(this != _instance)
				Destroy(this.gameObject);
		}

		hiScoreNumber = GameObject.Find ("HiScoreNumber").GetComponent<Text> ();
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		SetScore(0);

		if (Debug.isDebugBuild == false) {
			Destroy(GameObject.Find("DebugButton"));
		}
	}

	/// <summary>
	/// Resets the local score.
	/// </summary>
	public void ResetLocalScore () {
		points = 0;
		PlayerPrefs.DeleteAll();
		SetScore(points);
	}

	/// <summary>
	/// Gets the score.
	/// </summary>
	public int GetScore(){
		if (PlayerPrefs.HasKey ("highscore")) {
//			if (Social.Active.localUser.authenticated) {
//				Social.LoadScores ("CgkIi-bbm7gMEAIQAQ", scores => {
//					if (scores.Length > 0) {
//						Debug.Log ("Got " + scores.Length + " scores");
//						string myScores = "Leaderboard:\n";
//						foreach (IScore score in scores)
//							myScores += "\t" + score.userID + " " + score.formattedValue + " " + score.date + "\n";
//						Debug.Log (myScores);
//					} else {
//					}
//					Debug.Log ("No scores loaded");
//				});
//			}
			return PlayerPrefs.GetInt("highscore");
		}
		else{
			return 0;
		}
	}

	/// <summary>
	/// Shows the leaderboard UI.
	/// </summary>
	public void ShowLeaderboardUI () {
//		if (Social.Active.localUser.authenticated) {
			Social.ShowLeaderboardUI();
//		}
	}

	/// <summary>
	/// Adds to points.
	/// </summary>
	/// <param name="pointsToAdd">Points to add.</param>
	public static void AddToPoints (int pointsToAdd) {
		points += pointsToAdd;
	}

	/// <summary>
	/// Sets the score.
	/// </summary>
	public void SetScore(int newHighscore) {
		if (PlayerPrefs.HasKey("highscore")) {
			int oldHighscore = PlayerPrefs.GetInt("highscore", 0);    
			if(newHighscore > oldHighscore) {
				if (Social.Active.localUser.authenticated) {
					Social.ReportScore(newHighscore, "CgkIi-bbm7gMEAIQAQ", (bool success) => {});
				}
				PlayerPrefs.SetInt("highscore", newHighscore);
			}
		}
		else {
			PlayerPrefs.SetInt("highscore", newHighscore);
		}
		PlayerPrefs.Save();

		// Display the highscore if the text object is loaded
		if (hiScoreNumber) {
			// Displaying the points with the specified thousand-separator. Thanks to http://stackoverflow.com/a/752167/229507
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo) CultureInfo.InvariantCulture.NumberFormat.Clone();
			numberFormatInfo.NumberGroupSeparator = " ";
			hiScoreNumber.text = System.Convert.ToInt32(GetScore()).ToString("N0", numberFormatInfo);
		}
	}
}
