using UnityEngine;
using System.Collections;

public class BoardPiece : MonoBehaviour {

	public bool isEnemy;

	public void Destroy()
	{
		gameObject.SetActive(false);
	}

}
