using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FitScreenWidth : MonoBehaviour {

	Rect textureRect;

	void Start ()
	{
		textureRect = new Rect (
			-Screen.width * 0.5f,
			guiTexture.pixelInset.y,
			Screen.width,
			guiTexture.pixelInset.height
			);

		guiTexture.pixelInset = textureRect;
	}
}
