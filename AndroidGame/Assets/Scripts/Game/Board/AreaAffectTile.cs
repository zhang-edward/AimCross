using UnityEngine;
using System.Collections;

public class AreaAffectTile : BoardTile {

	void Start()
	{
		anim.clip = ThemeManager.instance.themes[ThemeManager.instance.themeIndex].enemyPressed;
	}

	public override void Hit()
	{
		ScoreManager.instance.IncrementButtonsPressed();
		if (Application.loadedLevelName.Equals("Tutorial"))
			TutorialGameManager.instance.score += pointValue;
		else
			GameManager.instance.score += pointValue;

		int x = (int)transform.position.x;
		int y = (int)transform.position.y;
		
		// set the board at this tile's position to null so this tile no longer registers as enabled
		board.board[y, x] = null;

		StartCoroutine(AreaEffect(x, y));
		
		board.floorTiles[y, x].gameObject.SetActive(true);
		board.floorTiles[y, x].animate ();
	}

	IEnumerator AreaEffect(int x, int y)
	{
		this.GetComponent<SpriteRenderer>().enabled = false;
		// set the board to wait for this button to process
		board.Wait ();
		for (int xOffset = -1; xOffset <= 1; xOffset ++)
		{
			for (int yOffset = -1; yOffset <= 1; yOffset ++)
			{
				int xPos = x + xOffset;
				int yPos = y + yOffset;
				
				// if the x and y positions are in bounds
				if (0 <= xPos && xPos < Board.boardSize &&
				    0 <= yPos && yPos < Board.boardSize)
				{
						if (board.board[yPos, xPos] != null)
							board.board[yPos, xPos].Hit();
						else
							board.floorTiles[yPos, xPos].Hit ();
						yield return new WaitForSeconds(0.1f);
				}
			}
		}
		gameObject.SetActive (false);
		yield return null;
	}
}
