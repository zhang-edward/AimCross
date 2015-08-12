using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

using GoogleMobileAds.Api;

public class GUIManager : MonoBehaviour {

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

	InterstitialAd interstitial;
	private const string INTERSTITIAL_ID = "ca-app-pub-1010781108315903/2216275874";

	void Awake()
	{
		canvas = GetComponent<Canvas>();
	}

	void Start()
	{
		// set the canvas to be in the UI layer (in front of everything else)
		canvas.sortingLayerName = "UI";

		RequestInterstitial();
	}

	void Update()
	{
		if (scoreTextIncrementer < GameManager.instance.score)
		{
			scoreTextIncrementer ++;

			scoreText.text = scoreTextIncrementer.ToString();
			scoreTextShadow.text = scoreText.text;

			highScoreText.text = "High Score: " + ScoreManager.instance.highScore.ToString();
			highScoreTextShadow.text = highScoreText.text;
		}
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

		// Every 6th game show an ad
		/*if (interstitial.IsLoaded() && 
		    ScoreManager.instance.gamesPlayed % 6 == 0)
		{
			interstitial.Show();
		}*/
	}

	private void RequestInterstitial()
	{
		#if UNITY_ANDROID
		string adUnitId = INTERSTITIAL_ID;
		#elif UNITY_IPHONE
		string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
		#else
		string adUnitId = "unexpected_platform";
		#endif
		
		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd(adUnitId);
		// Create an empty ad request.
		// ADD TEST DEVICE HERE
		AdRequest request = new AdRequest.Builder()
			.AddTestDevice("A4D94C6CCCC78F95136843C5B0579088")
			.Build();
		// Load the interstitial with the request.
		interstitial.LoadAd(request);
	}
	
	public void MainMenu()
	{
		Application.LoadLevel("MainMenu");
	}

	public void GPGAchievementsUI()
	{
		Social.ShowAchievementsUI();
	}

	public void GPGLeaderboardsUI()
	{
		Social.ShowLeaderboardUI();
	}
}
