// Divides the Screen.width in 3 equal parts corresponding to the tracks.
// When finger is moved, it checks which part the finger is in.
// If the part is different than in the last frame, inscruct the ship of the new track



using UnityEngine;
using System.Collections;

public class FollowFinger : MonoBehaviour {
	
	private Ship ship;
	
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
				break;
			case TouchPhase.Moved:
				break;
			}
		}
	}
}
