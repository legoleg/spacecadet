using UnityEngine;
using System.Collections;

public class PlayButton : TouchLogic {

	void Update()
	{
		CheckTouches();

#if UNITY_EDITOR
		// get input from keyboard
		if (Input.GetButton("Fire1"))
		{
			LoadGameScene (1);
		}	
#endif
	}
	
	public override void OnTouchBegan()
	{
		Debug.Log("OnTouchBegan message received...");
		LoadGameScene (1);
	}

	static void LoadGameScene (int sceneID)
	{
		Application.LoadLevel (sceneID);
	}
}
