using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

	public const int boardSize = 6;

	public int[,] boardGen = new int[boardSize, boardSize];
	public Floor[,] floorTiles = new Floor[boardSize, boardSize];
	public Enemy[,] board = new Enemy[boardSize, boardSize];

	// Prefabs
	public GameObject floorPrefab;
	public GameObject enemyPrefab;

	public bool initialized = false;

	public void InitBoard()
	{
		// initialize board completely with floor tiles
		for (int x = 0; x < boardSize; x ++)
		{
			for (int y = 0; y < boardSize; y ++)
			{
				floorTiles[y, x] = CreateNewTile(floorPrefab, x, y).GetComponent<BoardTile>() as Floor;
			}
		}

		// put in random boardPieces
		for (int x = 0; x < boardSize; x ++)
		{
			for (int y = 0; y < boardSize; y ++)
			{
				if (Random.value < 0.40)
					boardGen[y, x] = 1;
			}
		}

		StartCoroutine("InitBoardAnim");
	}

	IEnumerator InitBoardAnim()
	{
		yield return new WaitForSeconds(0.5f);
		for (int x = 0; x < boardSize; x ++)
		{
			for (int y = 0; y < boardSize; y ++)
			{
				if (boardGen[y, x] == 1)
				{
					GameObject o = CreateNewTile(enemyPrefab, x, y);
					Enemy enemy = o.GetComponent<Enemy>();
					enemy.animate ();
					board[y, x] = enemy;
				}
			}
		}

		initialized = true;
	}
	
	GameObject CreateNewTile(GameObject prefab, int x, int y)
	{
		GameObject o = Instantiate (prefab, new Vector3(x, y), Quaternion.identity) as GameObject;
		o.transform.SetParent(this.transform);
		o.GetComponent<BoardTile>().board = this;
		return o;
	}
}
