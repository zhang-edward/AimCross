using UnityEngine;
using System.Collections;

public class Aimer : MonoBehaviour {
	
	public int aimerNumber; 	// 1 or 0

	// Aimer Prefabs: Horizontal, Vertical, Center
	public AimerHorizontal aimerH;
	public AimerVertical aimerV;
	public AimerCenter aimerC;

	// time in seconds to wait before the aimer moves
	public float aimerSpeed = 8.0f;

	public bool aimed = false;

	public int targetX;
	public int targetY;

	public bool paused;

	public bool inputDisabled = false;	// used only in TutorialGameManager
	
	// Use this for initialization
	void Start () {
		Theme th = ThemeManager.instance.themes[ThemeManager.instance.themeIndex];
		if (aimerNumber == 0)
		{
			aimerH.prefabLeft.GetComponent<SpriteRenderer>().sprite = th.aimerH[0];
			aimerH.prefabMid.GetComponent<SpriteRenderer>().sprite = th.aimerH[1];
			aimerH.prefabRight.GetComponent<SpriteRenderer>().sprite = th.aimerH[2];
			aimerV.prefabTop.GetComponent<SpriteRenderer>().sprite = th.aimerV[0];
			aimerV.prefabMid.GetComponent<SpriteRenderer>().sprite = th.aimerV[1];
			aimerV.prefabBottom.GetComponent<SpriteRenderer>().sprite = th.aimerV[2];
		}
		else
		{
			aimerH.prefabLeft.GetComponent<SpriteRenderer>().sprite = th.aimerH[3];
			aimerH.prefabMid.GetComponent<SpriteRenderer>().sprite = th.aimerH[4];
			aimerH.prefabRight.GetComponent<SpriteRenderer>().sprite = th.aimerH[5];
			aimerV.prefabTop.GetComponent<SpriteRenderer>().sprite = th.aimerV[3];
			aimerV.prefabMid.GetComponent<SpriteRenderer>().sprite = th.aimerV[4];
			aimerV.prefabBottom.GetComponent<SpriteRenderer>().sprite = th.aimerV[5];
		}




		// give aimerH and aimerV references to aimerC to set its position
		aimerH.aimerC = this.aimerC;
		aimerV.aimerC = this.aimerC;
	}

	public void Aim()
	{
		StartCoroutine("AimH");
	}

	void Update()
	{
		if (paused)
		{
			aimerH.paused = true;
			aimerV.paused = true;
		}
		else
		{
			aimerH.paused = false;
			aimerV.paused = false;
		}
	}

	IEnumerator AimH()
	{
		aimerH.speed = aimerSpeed;
		aimerV.speed = aimerSpeed;

		// set aimerH active and to aiming mode
		aimerH.gameObject.SetActive (true);
		aimerH.aiming = true;
		// if the mouse button isn't pressed or if paused, do nothing
		while (aimerH.aiming || paused)
		{
			if (GameManager.getInput() && !paused && !inputDisabled)
			{
				// stops aiming mode
				aimerH.aiming = false;
			}
			yield return null;
		}
		// wait for end of frame so the same input isn't registered twice
		yield return new WaitForSeconds(1.0f/60.0f);
		
		// snaps aimerH to an integer x and y position
		aimerH.snap();

		/*// Tutorial ===============================================//
		if (GameManager.instance.showTutorial)
		{
			Board board = GameManager.instance.board;
			bool enemyFound = false;

			// Check if the player selected a row with an enemy tile in it
			int rowToCheck = (int)aimerH.targetY;
			int i = 0;
			while(i < Board.boardSize && !enemyFound)
			{
				if (board.board[rowToCheck, i] != null)
					enemyFound = true;
				i ++;
			}

			// If no enemy was found, restart the level
			if (!enemyFound)
			{
				TutorialGameManager.instance.StopCoroutine("Aim");
				TutorialGameManager.instance.guiManager.setTutorialText();

				// pause and wait for player input
				paused = true;
				while(!TutorialGameManager.getInput())
					yield return null;
				paused = false;
				// wait for end of frame so the same input isn't registered twice
				yield return new WaitForSeconds(1.0f/60.0f);
				
				TutorialGameManager.instance.StartCoroutine("Restart");
			}
		}
		// Tutorial ===============================================//*/

		StartCoroutine("AimV");
	}

	IEnumerator AimV()
	{
		/*// Tutorial ===============================================//
		if (GameManager.instance.showTutorial)
		{
			GameManager.instance.guiManager.setTutorialText(3);

			// pause and wait for player input
			paused = true;
			while(!GameManager.getInput())
				yield return null;
			paused = false;
			// wait for end of frame so the same input isn't registered twice
			yield return new WaitForSeconds(1.0f/60.0f);

			GameManager.instance.guiManager.setTutorialText(4);

			// pause and wait for player input
			paused = true;
			while(!GameManager.getInput())
				yield return null;
			paused = false;
			// wait for end of frame so the same input isn't registered twice
			yield return new WaitForSeconds(1.0f/60.0f);

			GameManager.instance.guiManager.setTutorialText (8);

			// pause and wait for player input
			paused = true;
			while(!GameManager.getInput())
				yield return null;
			paused = false;
			// wait for end of frame so the same input isn't registered twice
			yield return new WaitForSeconds(1.0f/60.0f);

			GameManager.instance.guiManager.setTutorialText(5);
		}
		// Tutorial ===============================================//
*/

		aimerV.gameObject.SetActive (true);
		aimerC.GetComponent<SpriteRenderer>().enabled = true;

		aimerV.aiming = true;
		// if the mouse button isn't pressed or if paused, do nothing
		while (aimerV.aiming || paused)
		{
			if (GameManager.getInput() && !paused && !inputDisabled)
			{
				aimerV.aiming = false;
			}
			yield return null;
		}
		// wait for end of frame so the same input isn't registered twice
		yield return new WaitForSeconds(1.0f/60.0f);

		aimerV.snap();

		// set the target coordinates
		targetY = (int)aimerH.targetY;
		targetX = (int)aimerV.targetX;


		// set this aimer to aimed mode
		aimed = true;
	}

	// controls the AimerCenter to animate correctly depending on the button hit
	public void hitTarget(bool hit)
	{
		aimerC.ShowIndicator(hit);
	}

	public void disableAimers()
	{
		aimerH.gameObject.SetActive(false);
		aimerV.gameObject.SetActive(false);
		aimerC.GetComponent<SpriteRenderer>().enabled = false;
	}
}
