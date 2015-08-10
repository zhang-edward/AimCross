using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

	public GameManager gameManager;

	private Canvas canvas;

	public Text scoreText;
	public Text scoreTextShadow;

	public GameObject pauseMenu;

	void Awake()
	{
		canvas = GetComponent<Canvas>();
	}

	void Start()
	{
		// set the canvas to be in the UI layer (in front of everything else)
		canvas.sortingLayerName = "UI";
	}

	void Update()
	{
		scoreText.text = gameManager.score.ToString();
		scoreTextShadow.text = scoreText.text;
	}

	public void Pause()
	{
		gameManager.paused = true;
		pauseMenu.gameObject.SetActive (true);
	}

	public void UnPause()
	{
		gameManager.paused = false;
		pauseMenu.gameObject.SetActive (false);
	}
}
