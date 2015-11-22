using UnityEngine;
using System.Collections;

public class TutorialGameManager : MonoBehaviour {
	
	public static TutorialGameManager instance;
	
	public Board board;					// Reference to the Board object
	
	public TutorialUI guiManager;
	
	public bool aiming;						// whether the game is waiting for the player to aim or not
	private Aimer[] aimers = new Aimer[2];	// the two (TODO: make it possible for potentially more) aimers
	private int aimerIndex = 0;				// index for which aimer to use
	
	public AudioClip hitGreen;
	public AudioClip hitRed;
	public AudioClip levelUp;
	
	public bool paused;
	public int score;

	public int tutorialLevel = 1;
	public int tutorialSequence = 0;

	// x and y coordinates to pause on in tutorial
	public int pauseCoordY;
	public int pauseCoordX;
	
	public int aimedMessage;
	public int finishMessage;

	public int[,] level1 = 
	   {{0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0},
		{0, 0, 0, 1, 0, 0},
		{0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0}};

	public int[,] level2 = 
	   {{0, 0, 0, 0, 0, 0},
		{0, 0, 1, 0, 0, 0},
		{0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 1, 0},
		{0, 1, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0}};

	public int[,] level3 = 
	   {{0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0},
		{0, 1, 0, 0, 1, 0},
		{0, 0, 0, 0, 0, 0}};

	public int[,] level4 = 
	   {{0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0},
		{0, 1, 1, 1, 0, 0},
		{0, 1, 2, 1, 0, 0},
		{0, 1, 1, 1, 0, 0},
		{0, 0, 0, 0, 0, 0}};

	public int[,] level5 =
	   {{0, 1, 2, 1, 0, 0},
		{0, 0, 1, 2, 1, 0},
		{0, 1, 1, 2, 0, 1},
		{0, 1, 2, 1, 1, 2},
		{0, 1, 1, 2, 2, 0},
		{0, 0, 0, 1, 2, 0}};
	
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
		guiManager.tutorialPanel.gameObject.SetActive (true);

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
		if (tutorialLevel == 1)
			board.PopulateBoardFromLayout(level1);
		else if (tutorialLevel == 2)
			board.PopulateBoardFromLayout(level2);
		else if (tutorialLevel == 3)
			board.PopulateBoardFromLayout(level3);
		else if (tutorialLevel == 4)
			board.PopulateBoardFromLayout(level4);
		else if (tutorialLevel == 5)
			board.PopulateBoardFromLayout(level5);

		// wait for the board to be initialized
		while (!board.populated)
			yield return null;

		// =========================== Tutorial ================================ //
		
		/*
		 * IMPORTANT WHEN DESIGNING TUTORIAL!
		 * To choose a message to display, make sure your tutorialLevel int is correct
		 * Look in the TutorialUIManager class in managers, then go to the method setTutorialText
		 * 
		 * 
		 * The tutorial sequence is the number aimer that we are on for the current level
		 * the first aimer (blue) would be 0, then the next one (purple) would be 1,
		 * then the next one (blue again) would be 2
		 * */

		// TUTORIAL 1 //
		if (tutorialLevel == 1)
		{
			aimedMessage = -1;
			finishMessage = -1;

			guiManager.setTutorialText(1, 0);

			// pause and wait for player input
			paused = true;
			while(!GameManager.getInput())
				yield return null;
			paused = false;
			// wait for end of frame so the same input isn't registered twice
			yield return new WaitForSeconds(1.0f/60.0f);

			guiManager.setTutorialText(1, 1);
		}
		// TUTORIAL 2 //
		else if (tutorialLevel == 2)
		{
			finishMessage = 3;
			guiManager.setTutorialText(2, 0);
		}
		// TUTORIAL 3 //
		else if (tutorialLevel == 3)
		{
			finishMessage = 3;

			guiManager.setTutorialText(3, 0);

			// pause and wait for player input
			paused = true;
			while(!GameManager.getInput())
				yield return null;
			paused = false;
			// wait for end of frame so the same input isn't registered twice
			yield return new WaitForSeconds(1.0f/60.0f);

			guiManager.setTutorialText(3, 1);
		}
		// TUTORIAL 4 //
		else if (tutorialLevel == 4)
		{
			finishMessage = 1;
			guiManager.setTutorialText(4, 0);
		}
		// TUTORIAL 5 //
		else if (tutorialLevel == 5)
		{
			finishMessage = 1;
			guiManager.setTutorialText(5, 0);
		}
		else
			guiManager.setTutorialText (0, 0);

		// =========================== Tutorial ================================ //
		
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
		foreach(Aimer a in aimers)
			a.aimerSpeed = 5.0f;
		
		//Debug.Log("Enter: Aim");
		aimers[aimerIndex].Aim ();
		
		// init target values with -1
		int targetX = -1;
		int targetY = -1;

		// =========================== Tutorial - Set aimedMessage ================================ //
		if (tutorialLevel == 2)
		{
			if (tutorialSequence == 0)
				aimedMessage = 1;
			else if (tutorialSequence == 1)
				aimedMessage = 2;
			else
				aimedMessage = -1;
		}
		else if (tutorialLevel == 3)
		{
			aimedMessage = 2;
		}
		else
			aimedMessage = -1;
		// =========================== Tutorial ================================ //

		while (aiming)
		{
			// =========================== Tutorial ================================ //
			Aimer aimer = aimers[aimerIndex];
			float yPos = aimers[aimerIndex].aimerH.transform.position.y;
			float xPos = aimers[aimerIndex].aimerV.transform.position.x;

			if (tutorialLevel == 1)
			{
				aimers[aimerIndex].inputDisabled = true;

				if (Mathf.Abs (yPos - 3) < 0.3f)
				{
					aimer.aimerH.aiming = false;
				}
				if (Mathf.Abs (xPos - 3) < 0.3f)
				{
					aimer.aimerV.aiming = false;
				}
			}

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
		// =========================== Tutorial ================================ //
		
		//Debug.Log ("Exit: Aim");
		StartCoroutine(ProcessAim (targetX, targetY));
	}
	
	private IEnumerator ProcessAim(int targetX, int targetY)
	{
		if (tutorialLevel == 1)
			aimers[aimerIndex].inputDisabled = false;
		// ================ TUTORIAL ===================== //
		if (aimedMessage != -1)
		{
			guiManager.setTutorialText(tutorialLevel, aimedMessage);
		}
		tutorialSequence ++;
		// ================ TUTORIAL ===================== //

		
		// if there is an enemy at the target x and y positions
		if (board.board[targetY, targetX] != null)
		{
			// set the aimer's center to animates
			aimers[aimerIndex].hitTarget(true);
			
			BoardTile enemy = board.board[targetY, targetX];
			enemy.Hit();
			
			// if the board is waiting for the button to process, stop
			// the coroutine from continuing
			while (board.Waiting)
				yield return null;
			
			// check if all enemy tiles are cleared
			if (board.CheckIfBoardClear())
			{
				score = 0;

				// disabled the aimers when regenerating level
				foreach(Aimer a in aimers)
					a.disableAimers();

				// ================ TUTORIAL ===================== //
				if (finishMessage != -1)
					guiManager.setTutorialText(tutorialLevel, finishMessage);

				tutorialSequence = 0;
				tutorialLevel ++;
				// ================ TUTORIAL ===================== //

				SoundManager.instance.RandomizeSfxGame(levelUp);

				yield return new WaitForSeconds(2.0f);

				// ================ TUTORIAL ===================== //
				if (tutorialLevel > 5)
				{
					PlayerPrefs.SetInt("TutorialComplete", 0);
					ScoreManager.instance.GPGUnlockAchievement(
						"CgkItczL6uMHEAIQAQ");

					guiManager.setTutorialText(5, 2);

					// pause and wait for player input
					paused = true;
					while(!GameManager.getInput())
						yield return null;
					paused = false;
					// wait for end of frame so the same input isn't registered twice
					yield return new WaitForSeconds(1.0f/60.0f);

					guiManager.MainMenu();
				}
				// ================ TUTORIAL ===================== //
			
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
			
			// Report the score to the score manager
			ScoreManager.instance.UpdateScore(score);
		}
		// if did not hit an enemy
		else
		{
			guiManager.setTutorialText(0, 1);

			aiming = true;

			StartCoroutine("Aim");
		}
		//Debug.Log ("Exit: ProcessAim");
		yield return null;
	}
	
	public bool getInput()
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
