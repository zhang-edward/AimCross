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
	public Text GPGButtonShadow;

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
		GPGButtonShadow.text = GPGButtonText.text;
	}

	public void PlayGame()
	{
		if (PlayerPrefs.HasKey ("TutorialComplete"))
			Application.LoadLevel ("Game");
		else
			Application.LoadLevel ("Tutorial");
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
		if (Social.localUser.authenticated)
			Social.ShowAchievementsUI();
		else
			GPGAuthenticate();
	}
	
	public void GPGLeaderboardsUI()
	{
		if (Social.localUser.authenticated)
			Social.ShowLeaderboardUI();
		else
			GPGAuthenticate();
	}

	public void GPGAuthenticate()
	{
		if (Social.localUser.authenticated)
		{
			GPGMenu.SetActive(true);
		}
		else
		{
			Social.localUser.Authenticate((bool success) => {
			});
		}
	}

	public void WriteReview()
	{
		// TODO: Replace URL with actual app url after package name is determined
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.ZedStudios.AimCross");
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
