using UnityEngine;
using System.Collections;

public class PlayButton : TouchLogic {

	void Update()
	{
		CheckTouches();
	}
	
	public override void OnTouchBegan()
	{
		Debug.Log("OnTouchBegan message received...");
		Application.LoadLevel(1);
	}
}
