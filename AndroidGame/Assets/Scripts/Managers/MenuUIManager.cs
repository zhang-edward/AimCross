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


	void Start()
	{
		soundToggle.isOn = !SoundManager.instance.GetComponent<AudioSource>().mute;

		Social.localUser.Authenticate((bool success) => {
			if (success)
			{
				PlayerPrefs.SetInt("GPG", 1);
			}
		});
	}

	public void PlayGame()
	{
		Application.LoadLevel ("Game");
	}

	public void openSettingsMenu()
	{
		settingsMenu.gameObject.SetActive(true);
	}

	public void closeSettingsMenu()
	{
		settingsMenu.gameObject.SetActive(false);
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
		if (PlayerPrefs.HasKey("GPG") && PlayerPrefs.GetInt("GPG") == 1)
			Social.ShowAchievementsUI();
		else
			GPGAuthenticate();
	}
	
	public void GPGLeaderboardsUI()
	{
		if (PlayerPrefs.HasKey("GPG") && PlayerPrefs.GetInt("GPG") == 1)
			Social.ShowLeaderboardUI();
		else
			GPGAuthenticate();
	}

	public void GPGAuthenticate()
	{
		if (PlayerPrefs.HasKey("GPG") && PlayerPrefs.GetInt("GPG") == 1)
		{
			GPGMenu.gameObject.SetActive(true);
		}
		else
		{
			Social.localUser.Authenticate((bool success) => {
				if (success)
				{
					PlayerPrefs.SetInt("GPG", 1);
				}
			});
		}
	}

	public void GPGLogOut()
	{
		PlayGamesPlatform.Instance.SignOut();
	}
}
