using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	private Board board;					// Reference to the Board object

	public bool aiming;						// whether the game is waiting for the player to aim or not
	private Aimer[] aimers = new Aimer[2];	// the two (TODO: make it possible for potentially more) aimers
	private int aimerIndex = 0;				// index for which aimer to use

	// TODO: implement a score system
	public int score;

	void Awake()
	{
		board = GetComponentInChildren<Board>();
	}
	
	void Start()
	{
		aimers[0] = transform.FindChild("Aimer1").GetComponent<Aimer>();
		aimers[1] = transform.FindChild("Aimer2").GetComponent<Aimer>();

		StartCoroutine("Init");
	}

	IEnumerator Init()
	{
		// Initialize the board (place tiles)
		board.InitBoard();

		// wait for the board to be initialized
		while (!board.initialized)
			yield return null;

		// wait for user to acknowledge that the board has been init'd
		while (!Input.GetMouseButtonDown(0))
			yield return null;

		yield return new WaitForEndOfFrame();

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

			// shorthand for if aimerIndex is 0, set to 1, else, set to 0
			// (switch the aimer between blue and purple)
			aimerIndex = aimerIndex == 0 ? 1 : 0;
			
			aiming = true;

			StartCoroutine("Aim");
		}
		else
		{
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
}
