using UnityEngine;
using System.Collections;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class MenuUIManager : MonoBehaviour {

	public GameObject settingsMenu;

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
		Social.localUser.Authenticate((bool success) => {
			if (success)
			{
				PlayerPrefs.SetInt("GPG", 1);
			}
		});
	}
}
