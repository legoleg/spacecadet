using UnityEngine;
using System.Collections;

public class PlayButton : TouchLogic {

	void Awake () {
		Time.timeScale = 1f;
	}

	public void LoadGameScene () {
		Application.LoadLevel (1);
	}
}
