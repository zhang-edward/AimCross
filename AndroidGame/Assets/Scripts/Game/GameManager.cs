using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	private Board board;					// Reference to the Board object

	public bool aiming;						// whether the game is waiting for the player to aim or not
	private Aimer[] aimers = new Aimer[2];	// the two (TODO: make it possible for potentially more) aimers
	private int aimerIndex = 0;				// index for which aimer to use

	public AudioClip hitGreen;
	public AudioClip hitRed;
	public AudioClip levelUp;

	public bool paused;
	public int score;

	void Awake()
	{
		board = GetComponentInChildren<Board>();
	}
	
	void Start()
	{
		aimers[0] = transform.FindChild("Aimer1").GetComponent<Aimer>();
		aimers[1] = transform.FindChild("Aimer2").GetComponent<Aimer>();

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

	IEnumerator Init()
	{
		// Initialize the board (place tiles)
		board.PopulateBoard();

		// wait for the board to be initialized
		while (!board.populated)
			yield return null;

		// wait for user to acknowledge that the board has been init'd
		while (!getInput () || paused)
			yield return null;

		yield return new WaitForSeconds(1.0f/60.0f);

		// initialize aiming coroutine
		aiming = true;
		StartCoroutine("Aim");
	}

	IEnumerator Aim()
	{
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
	
	IEnumerator ProcessAim(int targetX, int targetY)
	{
		//Debug.Log ("Enter: ProcessAim");
		if (board.board[targetY, targetX] != null)
		{
			// set the aimer's center to animate
			aimers[aimerIndex].hitTarget(true);

			board.board[targetY, targetX].Hit();

			// check if all enemy tiles are cleared
			if (board.checkIfBoardClear())
			{
				score += 5;
				SoundManager.instance.PlaySingle(levelUp);

				// disabled the aimers when regenerating level
				yield return new WaitForSeconds(1.0f);
				foreach(Aimer a in aimers)
					a.disableAimers();
				
				board.populated = false;
				
				StartCoroutine("Init");
			}
			// normal hit
			else
			{
				score += 1;
				SoundManager.instance.PlaySingle(hitGreen);

				// shorthand for if aimerIndex is 0, set to 1, else, set to 0
				// (switch the aimer between blue and purple)
				aimerIndex = aimerIndex == 0 ? 1 : 0;
				
				aiming = true;
				
				StartCoroutine("Aim");
			}
		}
		else
		{
			SoundManager.instance.PlaySingle(hitRed);
			// set the aimer's center to animate
			aimers[aimerIndex].hitTarget (false);

			StartCoroutine("GameOver");
		}
		//Debug.Log ("Exit: ProcessAim");
		yield return null;
	}

	IEnumerator GameOver()
	{
		//Debug.Log ("You lose");
		yield return new WaitForSeconds(1.0f);

		Application.LoadLevel(Application.loadedLevel);
	}

	public void Restart()
	{
		Application.LoadLevel (Application.loadedLevel);
	}

	public static bool getInput()
	{
		float yBorder = (Camera.main.orthographicSize * 2) * (10.0f / 12.0f) - 3.5f;
//#if UNITY_EDITOR
		return Input.GetMouseButtonDown(0) && 
			Camera.main.ScreenToWorldPoint(Input.mousePosition).y < yBorder;

//#elif UNITY_ANDROID
		/*return Input.touchCount > 0 && 
			Input.GetTouch (0).phase == TouchPhase.Began &&
			Input.GetTouch(0).position.y < yBorder;*/

//#endif
	}
}
