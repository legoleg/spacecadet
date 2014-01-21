using UnityEngine;
using System.Collections;

public class FBController : MonoBehaviour {

	public GameObject myAvatar;


	void Start ()
	{
		CallFBInit();
	}

	#region FB.Init() example
	
	bool isInit = false;
	
	private void CallFBInit()
	{
		FB.Init(OnInitComplete, OnHideUnity);
	}
	
	private void OnInitComplete()
	{
		if (FB.IsLoggedIn)
		{
			Debug.Log("FB.Init completed: User is logged in!");
			FB.API("/me?fields=picture.width(128)", Facebook.HttpMethod.GET, MyPictureCallback);
		}
		else 
		{
			Debug.Log("FB.Init completed: User is NOT logged in!");
			//login to facebook
			CallFBLogin();
		}
		isInit = true;
	}
	
	private void OnHideUnity(bool isGameShown)
	{
		Debug.Log("Is game showing? " + isGameShown);
	}
	
	#endregion FB.Init() example

	#region FB.Login() example
	
	private void CallFBLogin()
	{
		FB.Login("email,publish_actions", LoginCallback);
	}

	private string lastResponse = "";

	void LoginCallback(FBResult result)
	{
		if (result.Error != null)
		{
			lastResponse = "Error Response:\n" + result.Error;
		}
		else if (!FB.IsLoggedIn)
		{
			lastResponse = "Login cancelled by Player";
		}
		else
		{
			// Reqest player info and profile picture
//			FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);
		}
		FB.API("/me?fields=picture.width(128)", Facebook.HttpMethod.GET, MyPictureCallback);
	}

	void MyPictureCallback (FBResult result)
	{
		myAvatar.guiTexture.texture = result.Texture;
	}
	
	private void CallFBLogout()
	{
		FB.Logout();
	}
	#endregion FB.Login() example
}
