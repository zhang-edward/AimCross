using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class MenuUIManager : MonoBehaviour {

	public GameObject settingsMenu;
	public GameObject GPGMenu;

	public Toggle soundToggle;

	public Text GPGButtonText;

	void Start()
	{
	}

	void Update()
	{
		soundToggle.isOn = !SoundManager.instance.GetComponent<AudioSource>().mute;

		if (PlayGamesPlatform.Instance.IsAuthenticated())
		{
			GPGButtonText.text = "Log Out";
		}
		else
		{
			GPGButtonText.text = "Log In";
		}
	}

	public void PlayGame()
	{
		Application.LoadLevel ("Game");
	}

	public void Mute(bool set)
	{
		if (set == true)
		{
			PlayerPrefs.SetInt ("Mute", 0);
		}
		else
		{
			PlayerPrefs.SetInt ("Mute", 1);
		}
	}

	public void GPGAchievementsUI()
	{
		//if (Social.localUser.authenticated)
			Social.ShowAchievementsUI();
		//else
		//	PlayGame ();
	}
	
	public void GPGLeaderboardsUI()
	{
		//if (Social.localUser.authenticated)
			Social.ShowLeaderboardUI();
		//else
		//	PlayGame ();
	}

	public void GPGAuthenticate()
	{
		Social.localUser.Authenticate((bool success) => {
		});
	}

	public void WriteReview()
	{
		// TODO: Replace URL with actual app url after package name is determined
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.ZedStudios.AndroidGame");
	}

	public void GPGLogOut()
	{
		PlayGamesPlatform.Instance.SignOut();
	}

	public void ClearData()
	{
		PlayerPrefs.DeleteAll();
	}

	public void UISound()
	{
		SoundManager.instance.UiSound();
	}
}
