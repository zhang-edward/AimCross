using UnityEngine;
using System.Collections;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;


/// <summary>
/// Manage Scorekeeping and all Google Play Services
/// </summary>
public class ScoreManager : MonoBehaviour {

	private int lastScore;
	public int highScore;

	public int gamesPlayed;
	public int buttonsPressed;

	// power-ups
	private const int COST_POINTNORMAL = 5;
	private const int COST_POINTAREA = 10;
	private const int COST_INVERT = 15;
	private const int COST_CROSSCLEAR = 15;

	private int points;
	public int Points {
		get{return points;}
	}
	private int pu_PointNormal;
	private int pu_PointArea;
	private int pu_Invert;
	private int pu_CrossClear;

	public int PU_PointNormal {
		get{return pu_PointNormal;}
		set{pu_PointNormal = value;}
	}
	public int PU_PointArea {
		get{return pu_PointArea;}
		set{pu_PointArea = value;}
	}
	public int PU_Invert {
		get{return pu_Invert;}
		set{pu_Invert = value;}
	}
	public int PU_CrossClear {
		get{return pu_CrossClear;}
		set{pu_CrossClear = value;}
	}

	public static ScoreManager instance;

	void Awake()
	{
		// Make this a singleton
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		
		DontDestroyOnLoad(gameObject);

/*		// DEBUG
		points = 9999;*/
	}

	void Start()
	{
		if (!PlayerPrefs.HasKey("TutorialComplete"))
			PlayerPrefs.SetInt ("TutorialLevel", 1);

		// get the high score from the playerPrefs
		highScore = PlayerPrefs.GetInt("High Score");
		gamesPlayed = PlayerPrefs.GetInt ("Games Played");

		// get points from playerPrefs
		points = PlayerPrefs.GetInt("Points");

		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			.Build();
		PlayGamesPlatform.InitializeInstance(config);
		// recommended for debugging:
		//PlayGamesPlatform.DebugLogEnabled = true;
		// Activate the Google Play Games platform

		PlayGamesPlatform.Activate();

		Social.localUser.Authenticate((bool success) => {
		});

		Application.targetFrameRate = 35;
	}

	public void UpdateScore(int score)
	{
		lastScore = score;
		if (lastScore > highScore)
			highScore = lastScore;

		// if this level is not the tutorial
		if (Application.loadedLevelName.Equals("Game"))
		{
			// store the high score locally
			PlayerPrefs.SetInt ("High Score", highScore);
			
			if (highScore >= 50)
			{
				GPGUnlockAchievement(
					"CgkItczL6uMHEAIQBQ");
			}
			if (highScore >= 100)
			{
				GPGUnlockAchievement(
					"CgkItczL6uMHEAIQCA");
			}
			if (highScore >= 175)
			{
				GPGUnlockAchievement(
					"CgkItczL6uMHEAIQCQ");
			}
		}
	}

	public void ReportScore()
	{
		Social.ReportScore(highScore, "CgkItczL6uMHEAIQBw", (bool success) => {
		});
		points += lastScore;
		PlayerPrefs.SetInt ("Points", points);
	}

	public void GPGIncrementButtonsPressed()
	{
		buttonsPressed ++;
		GPGIncrementAchievement(
			"CgkItczL6uMHEAIQAw", 1);
		GPGIncrementAchievement(
			"CgkItczL6uMHEAIQBA", 1);
		GPGIncrementAchievement(
			"CgkItczL6uMHEAIQAg", 1);
		GPGIncrementAchievement(
			"CgkItczL6uMHEAIQCg", 1);
	}

	public void GPGUnlockAchievement(string code)
	{
		Social.ReportProgress(code, 100.0f, (bool success) => {
		});
	}

	public void GPGIncrementAchievement(string code, int progress)
	{
		PlayGamesPlatform.Instance.IncrementAchievement(
			code, progress, (bool success) => {
		});
	}

	// Buying powerups
	public void BuyPointNormal()
	{
		if (points >= COST_POINTNORMAL)
		{
			points -= COST_POINTNORMAL;
			pu_PointNormal ++;
		}
	}

	public void BuyPointArea()
	{
		if (points >= COST_POINTAREA)
		{
			points -= COST_POINTAREA;
			pu_PointArea ++;
		}
	}
	public void BuyInvert()
	{
		if (points >= COST_INVERT)
		{
			points -= COST_INVERT;
			pu_Invert ++;
		}
	}
	public void BuyCrossClear()
	{
		if (points >= COST_CROSSCLEAR)
		{
			points -= COST_CROSSCLEAR;
			pu_CrossClear ++;
		}
	}
}
