using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

	public GameManager gameManager;

	public Text scoreText;

	void Update()
	{
		scoreText.text = gameManager.score.ToString();
	}
}
