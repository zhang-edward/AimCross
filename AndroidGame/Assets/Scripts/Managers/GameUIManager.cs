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
	}

	void Update()
	{
		if (scoreTextIncrementer < GameManager.instance.score)
		{
			scoreTextIncrementer ++;
		}

		scoreText.text = scoreTextIncrementer.ToString();
		scoreTextShadow.text = scoreText.text;
		
		highScoreText.text = "High Score: " + ScoreManager.instance.highScore.ToString();
		highScoreTextShadow.text = highScoreText.text;

		soundToggle.isOn = !SoundManager.instance.GetComponent<AudioSource>().mute;
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

	public void GPGAchievementsUI()
	{
		Social.ShowAchievementsUI();
	}

	public void GPGLeaderboardsUI()
	{
		Social.ShowLeaderboardUI();
	}

	// Found on "http://forum.unity3d.com/threads/creating-a-share-button-intent-for-android-in-unity-that-forces-the-chooser.335751/"
	public void Share()
	{
		//execute the below lines if being run on a Android device
		#if UNITY_ANDROID

		string subject = "Come beat my score at Aim Cross!";
		string body = "I just scored " + scoreText.text + " in Aim Cross!\n" +
			"https://play.google.com/store/apps/details?id=com.ZedStudios.AimCross";

		//Reference of AndroidJavaClass class for intent
		AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");

		//Reference of AndroidJavaObject class for intent
		AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");

		//call setAction method of the Intent object created
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

		//set the type of sharing that is happening
		intentObject.Call<AndroidJavaObject>("setType", "text/plain");

		//add data to be passed to the other activity i.e., the data to be sent
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);

		//get the current activity
		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

		//start the activity by sending the intent data
		AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
		currentActivity.Call("startActivity", jChooser);
		#endif
	}

	public void UISound()
	{
		SoundManager.instance.UiSound();
	}
}
