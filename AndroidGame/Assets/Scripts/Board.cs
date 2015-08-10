﻿using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

	public const int boardSize = 6;

	public int[,] boardGen = new int[boardSize, boardSize];
	public BoardPiece[,] board = new BoardPiece[boardSize, boardSize];

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
				boardGen[y, x] = 0;
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

		for (int x = 0; x < boardSize; x ++)
		{
			for (int y = 0; y < boardSize; y ++)
			{
				if (boardGen[y, x] == 0)
				{
					board[y, x] = CreateNewTile(floorPrefab, x, y).GetComponent<BoardPiece>();
				}

				if (boardGen[y, x] == 1)
				{
					board[y, x] = CreateNewTile(enemyPrefab, x, y).GetComponent<BoardPiece>();
				}
			}
		}
	}
	
	GameObject CreateNewTile(GameObject prefab, int x, int y)
	{
		return Instantiate (prefab, new Vector3(x, y), Quaternion.identity) as GameObject;
	}
}
