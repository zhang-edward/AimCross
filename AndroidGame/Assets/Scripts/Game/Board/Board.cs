using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

	public const int boardSize = 6;

	private int[,] boardGen = new int[boardSize, boardSize];
	public Floor[,] floorTiles = new Floor[boardSize, boardSize];
	public BoardTile[,] board = new BoardTile[boardSize, boardSize];

	// Prefabs
	public GameObject floorPrefab;
	public GameObject enemyPrefab;
	public GameObject enemyAreaPrefab;
	public GameObject invertTilePrefab;
	public GameObject crossTilePrefab;

	public bool populated = false;

	public int level;

	private bool waiting;		// makes GameManager wait until all buttons are finished pressing before aiming again
	public bool Waiting{
		get{return waiting;}
	}
	private float waitTimer;

	public void InitBoard()
	{
		// initialize board completely with floor tiles
		for (int x = 0; x < boardSize; x ++)
		{
			for (int y = 0; y < boardSize; y ++)
			{
				floorTiles[y, x] = CreateFloorTile(floorPrefab, x, y).GetComponent<BoardTile>() as Floor;
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

		// Equation to calculate the number of enemies on the board, max 15
		int numEnemies = (int)(10.0f * (Mathf.Log10 (level + 1.0f)) + 2);
		if (numEnemies > 15)
			numEnemies = 15;

		while (numEnemies > 0)
		{
			int randX = Random.Range (0, boardSize);
			int randY = Random.Range (0, boardSize);

			if (boardGen[randY, randX] == 0)
			{
				if (Random.value < 0.75f)
					boardGen[randY, randX] = 1;
				else
					boardGen[randY, randX] = 2;

				numEnemies --;
			}
		}
		StartCoroutine("InitBoardAnim");
	}

	private IEnumerator InitBoardAnim()
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
					// create the correct prefab
					if (boardGen[y, x] == 1)
						CreateEnemyTile(enemyPrefab, x, y, delay);
					else if (boardGen[y, x] == 2)
						CreateEnemyTile(enemyAreaPrefab, x, y, delay);
				}
				// else make the floor tile at the coordinate visible
				else
					floorTiles[y, x].gameObject.SetActive (true);
			}
		}
		populated = true;

		yield return null;
	}

	private IEnumerator AnimateTile(GameObject tile, float delay, int x, int y)
	{
		yield return new WaitForSeconds(delay);

		// make the floor tile invisible and the enemy tile visible
		floorTiles[y, x].gameObject.SetActive(false);
		tile.SetActive(true);

		// animate tile
		BoardTile enemy = tile.GetComponent<BoardTile>();
//		board[y, x] = enemy;
		enemy.animate ();
	}
	
	public GameObject CreateEnemyTile(GameObject prefab, int x, int y, float delay)
	{
		GameObject o = Instantiate (prefab, new Vector3(x, y), Quaternion.identity) as GameObject;
		o.transform.SetParent(this.transform);

		BoardTile enemy = o.GetComponent<BoardTile>();
		enemy.board = this;
		board[y, x] = enemy;

		o.SetActive(false);	// tile should not be active until animated in AnimateTile()
		StartCoroutine (AnimateTile(o, delay, x, y));

		return o;
	}

	public GameObject CreateFloorTile(GameObject prefab, int x, int y)
	{
		GameObject o = Instantiate (prefab, new Vector3(x, y), Quaternion.identity) as GameObject;
		o.transform.SetParent(this.transform);
		
		o.GetComponent<BoardTile>().board = this;
		return o;
	}

	// check if the board is empty (there are no more active enemy tiles)
	public bool CheckIfBoardClear()
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

	public void Wait()
	{
		// if waitTimer coroutine has been started already
		if (waitTimer > 0)
		{
			waitTimer = 1.0f;
		}
		else
		{
			waitTimer = 1.0f;
			StartCoroutine("WaitForButtonProcess");
		}
	}

	IEnumerator WaitForButtonProcess()
	{
		waiting = true;
		while (waitTimer > 0)
		{
			waitTimer -= Time.deltaTime;
			yield return null;
		}
		waiting = false;
		yield return null;
	}
}