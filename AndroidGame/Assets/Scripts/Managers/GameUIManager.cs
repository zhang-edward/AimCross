using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GameUIManager : MonoBehaviour {

	private Canvas canvas;

	// this will continually increment until it reaches the score, giving the score that "scoreboard" feel
	public int scoreTextIncrementer;

	public Text scoreText;
	public Text scoreTextShadow;

	public Text highScoreText;
	public Text highScoreTextShadow;

	public AudioClip tutorialSound;
	public GameObject tutorialPanel;
	public Text tutorialText;

	public Text gameMenuScore;

	public GameObject pauseMenu;
	public GameObject gameMenu;

	public Toggle soundToggle;

	void Awake()
	{
		canvas = GetComponent<Canvas>();
	}

	void Start()
	{
		// set the canvas to be in the UI layer (in front of everything else)
		canvas.sortingLayerName = "UI";

		soundToggle.isOn = !SoundManager.instance.GetComponent<AudioSource>().mute;

		highScoreText.text = "High Score: " + ScoreManager.instance.highScore.ToString();

		if (PlayerPrefs.HasKey ("TutorialComplete"))
			tutorialPanel.SetActive (false);
	}

	void Update()
	{
		if (scoreTextIncrementer < GameManager.instance.score)
		{
			scoreTextIncrementer ++;

			scoreText.text = scoreTextIncrementer.ToString();
			scoreTextShadow.text = scoreText.text;
		}
		highScoreText.text = "High Score: " + ScoreManager.instance.highScore.ToString();
		highScoreTextShadow.text = highScoreText.text;
	}

	public void Pause()
	{
		GameManager.instance.paused = true;
		pauseMenu.gameObject.SetActive (true);
	}

	public void UnPause()
	{
		GameManager.instance.paused = false;
		pauseMenu.gameObject.SetActive (false);
	}

	public void GameMenu()
	{
		gameMenu.gameObject.SetActive(true);
		gameMenu.GetComponent<Animation>().Play();
		gameMenuScore.text = "Score:\n" + scoreText.text;
	}

	public void MainMenu()
	{
		Application.LoadLevel("MainMenu");
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

	public void setTutorialText(int textNumber)
	{
		if (textNumber == 1)
			tutorialText.text = "Tap the screen\nto begin";
		else if (textNumber == 2)
			tutorialText.text = "Tap when the bar\nmoves over a\nred button!";
		else if (textNumber == 3)
			tutorialText.text = "This row is\nconcealed from\nview now";
		else if (textNumber == 4)
			tutorialText.text = "Do you\nremember where\n the button was?";
		else if (textNumber == 5)
			tutorialText.text = "Tap again when\nthe bar moves\nover the\nred button!";
		else if (textNumber == 6)
			tutorialText.text = "Good Job!\nNow clear the board!";
		else if (textNumber == 7)
			tutorialText.text = "Oops!\nClick to try again";

		tutorialPanel.GetComponent<Animation>().Play();
		SoundManager.instance.PlaySingleGame(tutorialSound);
	}

	public void GPGAchievementsUI()
	{
		Social.ShowAchievementsUI();
	}

	public void GPGLeaderboardsUI()
	{
		Social.ShowLeaderboardUI();
	}

	public void UISound()
	{
		SoundManager.instance.UiSound();
	}
}
