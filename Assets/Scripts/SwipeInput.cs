using UnityEngine;
using System.Collections;

public class SwipeInput : MonoBehaviour {

	private float minSwipeDistX;
	private Vector2 startPos;

	private Ship ship;

	void Start ()
	{
		ship = GameObject.FindGameObjectWithTag ("Player").GetComponent<Ship> ();
		minSwipeDistX = Screen.width * .2f;
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
					// Reset the startPos to make a new measurement
					startPos = touch.position;
				}
				break;
			}
		}
	}
}
