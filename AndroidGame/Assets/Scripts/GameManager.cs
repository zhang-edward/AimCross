using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private Board board;

	public bool aiming;
	private Aimer[] aimers = new Aimer[2];
	private int aimerIndex = 0;


	public int score;

	void Awake()
	{
		board = GetComponent<Board>();
	}
	
	void Start()
	{
		aimers[0] = transform.FindChild("Aimer1").GetComponent<Aimer>();
		aimers[1] = transform.FindChild("Aimer2").GetComponent<Aimer>();

		board.InitBoard();

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

		// shorthand for if aimerIndex is 0, set to 1, else, set to 0
		aimerIndex = aimerIndex == 0 ? 1 : 0;

		//Debug.Log ("Exit: Aim");
		StartCoroutine(ProcessAim (targetX, targetY));
	}
	
	IEnumerator ProcessAim(int targetX, int targetY)
	{
		//Debug.Log ("Enter: ProcessAim");
		if (board.board[targetY, targetX] != null)
			board.board[targetY, targetX].Destroy();
		else
			GameOver();

		aiming = true;
		//Debug.Log ("Exit: ProcessAim");
		StartCoroutine("Aim");dq
		yield return null;
	}

	public void GameOver()
	{

	}
}
