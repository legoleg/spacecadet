using UnityEngine;
using System.Collections;

public class SwipeInput : MonoBehaviour {
	public float minSwipeDistY;
	public float minSwipeDistX;
	private Vector2 startPos;

	Ship ship;

	void Start ()
	{
		ship = GameObject.FindGameObjectWithTag ("Player").GetComponent<Ship> ();
	}
	
	void Update()
	{
		if (Input.touchCount > 0) {
			Touch touch = Input.touches[0];
			
			switch (touch.phase) {
			case TouchPhase.Began:
				startPos = touch.position;
				break;
			case TouchPhase.Moved:
				float swipeDistHorizontal = (new Vector3(touch.position.x,0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;
				if (swipeDistHorizontal > minSwipeDistX) {
					float swipeValue = Mathf.Sign(touch.position.x - startPos.x);
					//right swipe
					if (swipeValue > 0) {
						ship.MoveRight();
					}
					//left swipe
					else if (swipeValue < 0) {
						ship.MoveLeft();
					}
				}
				break;
			}
		}
	}
}
