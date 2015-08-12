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

	public bool populated = false;

	public int level;

	// TODO: spawn a set number of enemy tiles per level instead of just random
	void Start()
	{

	}

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
	}

	public void PopulateBoard()
	{
		// Equation to calculate the number of enemies on te board
		int numEnemies = (int)(20.0f * (Mathf.Log10 (level + 1.0f)));

		if (numEnemies > 18)
			numEnemies = 18;

		while (numEnemies > 0)
		{
			int randX = Random.Range (0, boardSize);
			int randY = Random.Range (0, boardSize);

			if (boardGen[randY, randX] == 0)
			{
				boardGen[randY, randX] = 1;
				numEnemies --;
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

					floorTiles[y, x].gameObject.SetActive(false);
				}
				else
				{
					floorTiles[y, x].gameObject.SetActive (true);
				}
			}
		}
		populated = true;
	}
	
	public GameObject CreateNewTile(GameObject prefab, int x, int y)
	{
		GameObject o = Instantiate (prefab, new Vector3(x, y), Quaternion.identity) as GameObject;
		o.transform.SetParent(this.transform);
		o.GetComponent<BoardTile>().board = this;
		return o;
	}

	// check if the board is empty (there are no more active enemy tiles)
	public bool checkIfBoardClear()
	{
		for (int x = 0; x < boardSize; x ++)
		{
			for (int y = 0; y < boardSize; y ++)
			{
				if (board[y, x] != null)
					return false;
			}
		}
		return true;
	}
}
