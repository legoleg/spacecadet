using UnityEngine;
using System.Collections;

public class KillingFloor : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D other)
	{
		Destroy(other.gameObject);
	}
}
