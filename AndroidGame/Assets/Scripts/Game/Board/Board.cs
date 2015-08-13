using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

	public const int boardSize = 6;

	public int[,] boardGen = new int[boardSize, boardSize];
	public Floor[,] floorTiles = new Floor[boardSize, boardSize];
	public BoardTile[,] board = new BoardTile[boardSize, boardSize];

	// Prefabs
	public GameObject floorPrefab;
	public GameObject enemyPrefab;
	public GameObject enemyAreaPrefab;

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

	public void PopulateBoardFromLayout(int[,] layout)
	{
		boardGen = layout;
		StartCoroutine("InitBoardAnim");
	}

	public void PopulateBoard()
	{
		// reset the boardGen
		for (int x = 0; x < boardSize; x ++)
		{
			for (int y = 0; y < boardSize; y ++)
			{
				boardGen[y, x] = 0;
			}
		}

		// Equation to calculate the number of enemies on te board
		int numEnemies = (int)(20.0f * (Mathf.Log10 (level + 1.0f)));

		Debug.Log (numEnemies);

		if (numEnemies > 18)
			numEnemies = 18;

		while (numEnemies > 0)
		{
			int randX = Random.Range (0, boardSize);
			int randY = Random.Range (0, boardSize);

			if (boardGen[randY, randX] == 0)
			{
				if (Random.value < 0.80)
					boardGen[randY, randX] = 1;
				else
					boardGen[randY, randX] = 2;

				numEnemies --;
			}
		}


		StartCoroutine("InitBoardAnim");
	}

	IEnumerator InitBoardAnim()
	{
		for (int x = 0; x < boardSize; x ++)
		{
			for (int y = 0; y < boardSize; y ++)
			{
				// if the boardGen is set to something that is not a floor tile
				if (boardGen[y, x] != 0)
				{
					// have a random delay before animating the tiles so they don't all animate at the same time
					float delay = Random.value;
					GameObject obj = null;

					// create the correct prefab
					if (boardGen[y, x] == 1)
						obj = CreateNewTile(enemyPrefab, x, y);
					else if (boardGen[y, x] == 2)
						obj = CreateNewTile(enemyAreaPrefab, x, y);

					obj.SetActive (false);

					StartCoroutine (AnimateTile(obj, delay, x, y));

				}
				// else make the floor tile at the coordinate visible
				else
					floorTiles[y, x].gameObject.SetActive (true);
			}
		}
		populated = true;

		yield return null;
	}

	IEnumerator AnimateTile(GameObject tile, float delay, int x, int y)
	{
		yield return new WaitForSeconds(delay);

		// make the floor tile invisible and the enemy tile visible
		floorTiles[y, x].gameObject.SetActive(false);
		tile.SetActive(true);

		// add the prefab to the board array and animate tile
		BoardTile enemy = tile.GetComponent<BoardTile>();
		board[y, x] = enemy;
		enemy.animate ();
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
