using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

	public const int boardSize = 6;

	public int[,] boardGen = new int[boardSize, boardSize];
	public Enemy[,] board = new Enemy[boardSize, boardSize];

	// Prefabs
	public GameObject floorPrefab;
	public GameObject enemyPrefab;

	public void InitBoard()
	{
		// initialize board completely with floor tiles
		for (int x = 0; x < boardSize; x ++)
		{
			for (int y = 0; y < boardSize; y ++)
			{
				CreateNewTile(floorPrefab, x, y);
			}
		}

		// put in random enemies
		for (int x = 0; x < boardSize; x ++)
		{
			for (int y = 0; y < boardSize; y ++)
			{
				if (Random.value < 0.50)
					boardGen[y, x] = 1;
			}
		}

		for (int x = 0; x < boardSize; x ++)
		{
			for (int y = 0; y < boardSize; y ++)
			{
				if (boardGen[y, x] == 1)
				{
					board[y, x] = CreateNewTile(enemyPrefab, x, y).GetComponent<Enemy>();
				}
			}
		}
	}
	
	GameObject CreateNewTile(GameObject prefab, int x, int y)
	{
		return Instantiate (prefab, new Vector3(x, y), Quaternion.identity) as GameObject;
	}
}
