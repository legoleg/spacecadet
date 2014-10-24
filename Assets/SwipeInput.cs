using UnityEngine;
using System.Collections;

public class SwipeInput : MonoBehaviour {
	public float minSwipeDistY;
	public float minSwipeDistX;
	private Vector2 startPos;
	
	void Update() {
		if (Input.touchCount > 0) {
			Touch touch = Input.touches[0];
			
			switch (touch.phase) {
			case TouchPhase.Began:
				startPos = touch.position;
				break;
			case TouchPhase.Ended:
				float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
				if (swipeDistVertical > minSwipeDistY) {
					float swipeValue = Mathf.Sign(touch.position.y - startPos.y);
					//up swipe
					if (swipeValue > 0) {
						//Jump ();
					} 
					//down swipe
					else if (swipeValue < 0) {
						//Shrink ();
					}
				}
				float swipeDistHorizontal = (new Vector3(touch.position.x,0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;
				if (swipeDistHorizontal > minSwipeDistX) {
					float swipeValue = Mathf.Sign(touch.position.x - startPos.x);
					//right swipe
					if (swipeValue > 0) {
						//MoveRight ();
					}
					//left swipe
					else if (swipeValue < 0) {
						//MoveLeft ();
					}
				}
				break;
			}
		}
	}
}
