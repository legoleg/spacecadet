// Divides the Screen.width in 3 equal parts corresponding to the tracks.
// When finger is moved, it checks which part the finger is in.
// If the part is different than in the last frame, inscruct the ship of the new track



using UnityEngine;
using System.Collections;

public class FollowFinger : MonoBehaviour {
	
	private Ship ship;
	private int lastSection;
	private int currentSection = 1;
	private int x1,x2;

	
	void Start ()
	{
		ship = GameObject.FindGameObjectWithTag ("Player").GetComponent<Ship> ();

		// Divide screen in 3 sections
		// If Screen.width is 480
		// 0 = -inf,160 or <= Screen.width/3
		// 1 = 161,320 or >Screen.width/3 && <(2*(Screen.width/3))
		// 2 = 321,inf or >=(2*(Screen.width/3))
		x1 = Screen.width / 3;
		x2 = x1 * 2;
	}
	
	void Update()
	{
		if (Input.touchCount > 0) {
			Touch touch = Input.touches[0];
			
			switch (touch.phase) {
			case TouchPhase.Began:
				MoveIfNewPosition(touch);
				break;
			case TouchPhase.Moved:
				MoveIfNewPosition(touch);
				break;
			}
		}
	}

	void MoveIfNewPosition (Touch touch)
	{
		lastSection = currentSection;
		
		if (touch.position.x < x1) {
			currentSection = 0;
		} else if (touch.position.x > x2) {
			currentSection = 2;
		} else {
			currentSection = 1;
		}
		
		if (lastSection != currentSection) {
			ship.ApplyMove (currentSection);
		}
	}
}
