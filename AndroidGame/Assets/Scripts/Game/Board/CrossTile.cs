using UnityEngine;
using System.Collections;

public class CrossTile : BoardTile {
	
	public override void Hit()
	{
		// call the parent method first
		base.Hit ();
		
		int x = (int)transform.position.x;
		int y = (int)transform.position.y;
		
		// set the board at this tile's position to null so this tile no longer registers as enabled
		board.board[y, x] = null;
		
		StartCoroutine(Effect(x, y));
		
		board.floorTiles[y, x].gameObject.SetActive(true);
		board.floorTiles[y, x].animate ();
	}
	
	IEnumerator Effect(int x, int y)
	{
		this.GetComponent<SpriteRenderer>().enabled = false;
		// set the board to wait for this button to process
		board.Wait ();
		for (int offset = 1; offset <= Board.boardSize; offset ++)
		{
			hitTile(x + offset, y + offset);
			hitTile(x + offset, y - offset);
			hitTile(x - offset, y + offset);
			hitTile(x - offset, y - offset);

			yield return new WaitForSeconds(0.2f);
		}
		gameObject.SetActive (false);
		yield return null;
	}

	private void hitTile(int x, int y)
	{
		if (inBounds (x, y))
		{
			if (board.board[y, x] != null)
				board.board[y, x].Hit();
			else
				board.floorTiles[y, x].Hit ();
		}
	}

	private bool inBounds(int x, int y)
	{
		return 0 <= x && x < Board.boardSize && 0 <= y && y < Board.boardSize;
	}
}
