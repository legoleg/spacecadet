/*
* Script by Devin Curry
* www.Devination.com
* www.youtube.com/user/curryboy001
* Please like and subscribe if you found my tutorials helpful :D
*/
using UnityEngine;
using System.Collections;

public class TouchLogic : MonoBehaviour 
{
	//so other scripts can know what touch is currently on screen
	public static int currTouch = 0;

	public void CheckTouches() {
		//is there a touch on screen?
		if(Input.touches.Length <= 0) {
			OnNoTouches();
		} else {
			// If there is a touch, loop through all the the touches on screen.
			foreach(Touch touch in Input.touches) {
				currTouch = touch.fingerId;
				//executes this code for current touch (i) on screen
				if(this.guiTexture != null && (this.guiTexture.HitTest(touch.position))) {
					//if current touch hits our guitexture, run this code
					switch(touch.phase) {
					case TouchPhase.Began:
						OnTouchBegan();
						break;
					case TouchPhase.Ended:
						OnTouchEnded();
						break;
					case TouchPhase.Moved:
						OnTouchMoved();
						break;
					case TouchPhase.Stationary:
						OnTouchStayed();
						break;
					}
				}
				
				//outside so it doesn't require the touch to be over the guitexture
				switch(touch.phase) {
				case TouchPhase.Began:
					OnTouchBeganAnywhere();
					break;
				case TouchPhase.Ended:
					OnTouchEndedAnywhere();
					break;
				case TouchPhase.Moved:
					OnTouchMovedAnywhere();
					break;
				case TouchPhase.Stationary:
					OnTouchStayedAnywhere();
					break;
				}
			}
		}
	}
	
	//the default functions, define what will happen if the child doesn't override these functions
	public virtual void OnNoTouches(){}
	public virtual void OnTouchBegan(){}
	public virtual void OnTouchEnded(){}
	public virtual void OnTouchMoved(){}
	public virtual void OnTouchStayed(){}
	public virtual void OnTouchBeganAnywhere(){}
	public virtual void OnTouchEndedAnywhere(){}
	public virtual void OnTouchMovedAnywhere(){}
	public virtual void OnTouchStayedAnywhere(){}
}