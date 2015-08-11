using UnityEngine;
using System.Collections;

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
}
