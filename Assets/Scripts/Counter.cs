using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Counter : MonoBehaviour {

	private int count = 0;
	public Text countText;
//	private int adFrequency = 128;
	private bool isCounting = false;

	public void Start()
	{
		countText.text = count.ToString ();
	}

	public void AddOne()
	{
		if (isCounting) {
			return;
		}
		count++;
		StartCoroutine(LimitCount());
		countText.text = count.ToString();

//		if (count % adFrequency == 0)
//		{
//			StartCoroutine(HeyZapAdsController.AdRoutine());
//		}
	}

	IEnumerator LimitCount() {
		isCounting = true;
		yield return new WaitForEndOfFrame();
		isCounting = false;
	}
}
