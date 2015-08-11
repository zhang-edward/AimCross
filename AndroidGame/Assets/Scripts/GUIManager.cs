using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using GoogleMobileAds.Api;


public class GUIManager : MonoBehaviour {

	public GameManager gameManager;

	private Canvas canvas;

	public Text scoreText;
	public Text scoreTextShadow;

	public Text highScoreText;
	public Text highScoreTextShadow;

	public Text gameMenuScore;

	public GameObject pauseMenu;
	public GameObject gameMenu;

	private const string INTERSTITIAL_ID = "ca-app-pub-1010781108315903/2216275874";


	void Awake()
	{
		canvas = GetComponent<Canvas>();
	}

	void Start()
	{


		// set the canvas to be in the UI layer (in front of everything else)
		canvas.sortingLayerName = "UI";
	}

	void Update()
	{
		scoreText.text = gameManager.score.ToString();
		scoreTextShadow.text = scoreText.text;

		highScoreText.text = "High Score: " + ScoreManager.instance.highScore.ToString();
		highScoreTextShadow.text = highScoreText.text;
	}

	public void Pause()
	{
		gameManager.paused = true;
		pauseMenu.gameObject.SetActive (true);
	}

	public void UnPause()
	{
		gameManager.paused = false;
		pauseMenu.gameObject.SetActive (false);
	}

	public void GameMenu()
	{
		gameMenu.gameObject.SetActive(true);
		gameMenu.GetComponent<Animation>().Play();
		gameMenuScore.text = "Score:\n" + scoreText.text;

		RequestInterstitial();
	}

	private void RequestInterstitial()
	{
		// Initialize an InterstitialAd.
		InterstitialAd interstitial = new InterstitialAd(INTERSTITIAL_ID);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
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
