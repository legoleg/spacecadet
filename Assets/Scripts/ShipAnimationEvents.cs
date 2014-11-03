using UnityEngine;
using System.Collections;

public class ShipAnimationEvents : MonoBehaviour {
	private Ship ship;

	void Start () {
		ship = GetComponentsInParent<Ship> ()[0];
	}

	public void Shoot () {
		if (ship.canShoot) {
			ship.Shoot();
		}
	}
}
