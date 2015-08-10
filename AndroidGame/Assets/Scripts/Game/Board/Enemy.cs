using UnityEngine;
using System.Collections;

public class Enemy : BoardTile {

	public override void Hit()
	{
		int x = (int)transform.position.x;
		int y = (int)transform.position.y;

		// set the board at this tile's position to null so this tile no longer registers as enabled
		board.board[y, x] = null;
		gameObject.SetActive(false);

		board.floorTiles[y, x].animate ();
	}
}
