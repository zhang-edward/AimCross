using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class TutorialUIManager : MonoBehaviour {

	private Canvas canvas;

	// this will continually increment until it reaches the score, giving the score that "scoreboard" feel
	public int scoreTextIncrementer;

	public Text scoreText;
	public Text scoreTextShadow;

	public AudioClip tutorialSound;
	public GameObject tutorialPanel;
	public Text tutorialText;
	public Text tutorialTextShadow;

	public GameObject pauseMenu;

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

		if (PlayerPrefs.HasKey ("TutorialComplete"))
			tutorialPanel.SetActive (false);
	}

	void Update()
	{
		if (scoreTextIncrementer < TutorialGameManager.instance.score)
			scoreTextIncrementer ++;
		else if (scoreTextIncrementer > TutorialGameManager.instance.score)
			scoreTextIncrementer --;

		scoreText.text = scoreTextIncrementer.ToString();
		scoreTextShadow.text = scoreText.text;

		soundToggle.isOn = !SoundManager.instance.GetComponent<AudioSource>().mute;
	}

	public void Pause()
	{
		TutorialGameManager.instance.paused = true;
		pauseMenu.gameObject.SetActive (true);
	}

	public void UnPause()
	{
		TutorialGameManager.instance.paused = false;
		pauseMenu.gameObject.SetActive (false);
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

	public void setTutorialText(int tutorialNumber, int textNumber)
	{
		// "Tap Screen to begin"
		if (tutorialNumber == 0)
		{
			if (textNumber == 0)
				tutorialText.text = "Tap the screen\nto begin";
			else if (textNumber == 1)
				tutorialText.text = "Try again!";
		}
		// TUTORIAL 1 //
		else if (tutorialNumber == 1)
		{
			if (textNumber == 0)
				tutorialText.text = "The goal of the\ngame is to\nmake the red\nbuttons green";
			else if (textNumber == 1)
				tutorialText.text = "Do this by\naiming the\ncrosshairs over\nthe red buttons";
		}
		
		// TUTORIAL 2 //
		else if (tutorialNumber == 2)
		{
			if (textNumber == 0)
				tutorialText.text = "Tap when the\nbar moves over\nthe red button";
			else if (textNumber == 1)
				tutorialText.text = "Do this with the\npurple crosshair\nas well";
			else if (textNumber == 2)
				tutorialText.text = "The two\ncrosshairs\ntake turns\naiming";
			else if (textNumber == 3)
				tutorialText.text = "Nice!";
		}

		else if (tutorialNumber == 3)
		{
			if (textNumber == 0)
				tutorialText.text = "When you aim,\nthe bars conceal\nparts of\nthe screen";
			else if (textNumber == 1)
				tutorialText.text = "Memorize the\npositions of\nthese two\nred buttons";
			else if (textNumber == 2)
				tutorialText.text = "Now aim the\npurple crosshair\nover the\nsecond button";
			else if (textNumber == 3)
				tutorialText.text = "Great!";
		}

		else if (tutorialNumber == 4)
		{
			if (textNumber == 0)
				tutorialText.text = "Darker red tiles\nhit all\nthe buttons\nadjacent to them";
			else if (textNumber == 1)
				tutorialText.text = "Awesome!";
		}

		else if (tutorialNumber == 5)
		{
			if (textNumber == 0)
				tutorialText.text = "You can even\nchain dark tiles\ntogether!";
			else if (textNumber == 1)
				tutorialText.text = "This concludes\nthe tutorial";
			else if (textNumber == 2)
				tutorialText.text = "Hope you enjoy\nAim Cross!";
		}

		tutorialTextShadow.text = tutorialText.text;
		
		tutorialPanel.GetComponent<Animation>().Play();
		SoundManager.instance.PlaySingleGame(tutorialSound);
	}
}
