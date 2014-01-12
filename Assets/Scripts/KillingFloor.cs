using UnityEngine;
using System.Collections;

public class KillingFloor : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		Destroy(other.gameObject);
	}
}
