using UnityEngine;
using System.Collections;

public class ArrowButton : TouchLogic
{
	public enum Direction {left,right};
	public Direction direction;
	Ship ship;

	void Start ()
	{
		ship = GameObject.FindGameObjectWithTag("Player").GetComponent<Ship>();
	}
	
	void Update()
	{
		CheckTouches();
	}
	
	public override void OnTouchBegan()
	{
		Debug.Log("OnTouchBegan message received...");
		if (direction == Direction.left)
			ship.btnLeftDown = true;
		else if (direction == Direction.right)
			ship.btnRightDown = true;
	}

	public override void OnTouchEnded()
	{
		Debug.Log("OnTouchEnded message received...");
		if (direction == Direction.left)
			ship.btnLeftDown = false;
		else if (direction == Direction.right)
			ship.btnRightDown = false;
	}
}