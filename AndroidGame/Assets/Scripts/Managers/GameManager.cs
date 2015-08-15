using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public Board board;					// Reference to the Board object

	public GameUIManager guiManager;

	public bool aiming;						// whether the game is waiting for the player to aim or not
	private Aimer[] aimers = new Aimer[2];	// the two (TODO: make it possible for potentially more) aimers
	private int aimerIndex = 0;				// index for which aimer to use

	public AudioClip hitGreen;
	public AudioClip hitRed;
	public AudioClip levelUp;

	public bool paused;
	public int score;

	public bool showTutorial;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		board = GetComponentInChildren<Board>();

		aimers[0] = transform.FindChild("Aimer1").GetComponent<Aimer>();
		aimers[1] = transform.FindChild("Aimer2").GetComponent<Aimer>();
	}
	
	void Start()
	{
		//DEBUG! DELETE WHEN BUILDING!!!!!!!!!
		//PlayerPrefs.DeleteAll();
		//DEBUG! DELETE WHEN BUILDING!!!!!!!!!

		if (!PlayerPrefs.HasKey("TutorialComplete"))
		{
			showTutorial = true;
			guiManager.highScoreText.gameObject.SetActive (false);
			guiManager.tutorialPanel.gameObject.SetActive (true);
		}

		else
		{
			showTutorial = false;
			guiManager.highScoreText.gameObject.SetActive (true);
			guiManager.tutorialPanel.gameObject.SetActive (false);
		}


		board.InitBoard();
		StartCoroutine("Init");
	}

	void Update()
	{
		if (paused)
		{
			foreach(Aimer a in aimers)
				a.paused = true;
		}
		else
		{
			foreach(Aimer a in aimers)
				a.paused = false;
		}
	}

	private IEnumerator Init()
	{
		// Request Interstitial ad 3 games before it is displayed
		if (ScoreManager.instance.gamesPlayed % 6 - 3 == 0)
			AdManager.instance.RequestInterstitial();

		// Initialize the board (place tiles)
		board.PopulateBoard();

		// wait for the board to be initialized
		while (!board.populated)
			yield return null;

		// Tutorial ===============================================//

		if (PlayerPrefs.HasKey("TutorialComplete"))
		{
			showTutorial = false;
			guiManager.highScoreText.gameObject.SetActive (true);
			guiManager.tutorialPanel.gameObject.SetActive (false);
		}

		if (showTutorial)
			guiManager.setTutorialText(1);
		// Tutorial ===============================================//

		// wait for user to acknowledge that the board has been init'd
		while (!getInput () || paused)
			yield return null;

		yield return new WaitForSeconds(1.0f/60.0f);

		// initialize aiming coroutine
		aiming = true;
		StartCoroutine("Aim");
	}

	private IEnumerator Aim()
	{
		// Speed of aimer increases logarithmically as level increases
		// I found this equation just thru a graphing calc and tweaking
		float speed = (3.0f * Mathf.Log10(board.level + 1.0f) + 4.0f);
		foreach(Aimer a in aimers)
			a.aimerSpeed = speed;

		//Debug.Log("Enter: Aim");
		aimers[aimerIndex].Aim ();

		// init target values with -1
		int targetX = -1;
		int targetY = -1;

		while (aiming)
		{
			// if aimer is finished aiming, get target coords and start next coroutine
			if (aimers[aimerIndex].aimed)
			{
				aiming = false;
				targetX = aimers[aimerIndex].targetX;
				targetY = aimers[aimerIndex].targetY;
			}
			yield return null;
		}
		// reset aimer
		aimers[aimerIndex].aimed = false;

		//Debug.Log ("Exit: Aim");
		StartCoroutine(ProcessAim (targetX, targetY));
	}
	
	private IEnumerator ProcessAim(int targetX, int targetY)
	{
		//Debug.Log ("Enter: ProcessAim");

		// if there is an enemy at the target x and y positions
		if (board.board[targetY, targetX] != null)
		{
			// set the aimer's center to animates
			aimers[aimerIndex].hitTarget(true);

			BoardTile enemy = board.board[targetY, targetX];
			enemy.Hit();

			// if the board is waiting for the button to process, stop
			// the coroutine from continuing
			while (board.waiting)
				yield return null;

			// check if all enemy tiles are cleared
			if (board.checkIfBoardClear())
			{
				score += 5;
				SoundManager.instance.RandomizeSfxGame(levelUp);

				foreach(Aimer a in aimers)
					a.disableAimers();

				// disabled the aimers when regenerating level
				yield return new WaitForSeconds(1.0f);

				board.level ++;
				board.populated = false;
				
				StartCoroutine("Init");
			}
			// normal hit
			else
			{
				SoundManager.instance.RandomizeSfxGame(hitGreen);

				// shorthand for if aimerIndex is 0, set to 1, else, set to 0
				// (switch the aimer between blue and purple)
				aimerIndex = aimerIndex == 0 ? 1 : 0;
				
				aiming = true;
				
				StartCoroutine("Aim");
			}

			// Tutorial ===============================================//
			if (showTutorial)
			{
				guiManager.setTutorialText(6);
				PlayerPrefs.SetInt("TutorialComplete", 0);
				showTutorial = false;

				ScoreManager.instance.GPGUnlockAchievement(
					"CgkItczL6uMHEAIQAQ");
			}
			// Tutorial ===============================================//

			// Report the score to the score manager
			ScoreManager.instance.UpdateScore(score);
		}
		else
		{
			SoundManager.instance.RandomizeSfxGame(hitRed);
			// set the aimer's center to animate
			aimers[aimerIndex].hitTarget (false);

			StartCoroutine("GameOver");
		}
		//Debug.Log ("Exit: ProcessAim");
		yield return null;
	}

	public IEnumerator GameOver()
	{
		yield return new WaitForSeconds(0.5f);
		// show the player the board
		foreach(Aimer a in aimers)
		{
			a.aimerH.snap();
			a.aimerV.snap();
			a.disableAimers();
		}
		yield return new WaitForSeconds(0.5f);

		if (ScoreManager.instance.gamesPlayed % 6 == 0 &&
		    ScoreManager.instance.gamesPlayed != 0)
			AdManager.instance.ShowInterstitial();

		ScoreManager.instance.GPGReportScore();
		ScoreManager.instance.gamesPlayed ++;

		guiManager.GameMenu();
	}

	public void Restart()
	{
		Application.LoadLevel (Application.loadedLevel);
	}

	public static bool getInput()
	{
		float yBorder = (Camera.main.orthographicSize * 2) * (3.0f / 4.0f) - 3.5f;
//#if UNITY_EDITOR
		return (Input.GetMouseButtonDown(0) && 
			Camera.main.ScreenToWorldPoint(Input.mousePosition).y < yBorder);

//#elif UNITY_ANDROID
		/*return Input.touchCount > 0 && 
			Input.GetTouch (0).phase == TouchPhase.Began &&
			Input.GetTouch(0).position.y < yBorder;*/

//#endif
	}
}
