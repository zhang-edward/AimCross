using UnityEngine;
using System.Collections;

public class AimerHorizontal : MonoBehaviour {

	public Board board;
	int boardSize;

	public bool aiming;
	public bool movingUp;
	public float speed;

	public GameObject prefab;

	// The y coordinate after this aimer has stopped
	public float targetY;

	private float counter = 0.0f;

	void Awake()
	{
		// Create the sprites that make up the bar
		boardSize = Board.boardSize;
		for (int i = -boardSize / 2; i < boardSize / 2; i ++)
		{
			GameObject o = Instantiate (prefab, this.transform.position, Quaternion.identity) as GameObject;
			Vector3 localPos = new Vector3(i, 0, 0);
			o.transform.SetParent(this.transform);
			o.transform.localPosition = localPos;
		}
	}

	void Update()
	{
		// use Mathf.PingPong() to make the aimer back and forth along the board
		if (aiming)
		{
			counter += speed * Time.deltaTime;
			float yPos = Mathf.PingPong(counter, boardSize - 1);
			transform.position = new Vector3(transform.position.x, yPos);
		}
	}

	public void snapToY()
	{
		// round the counter so that Mathf.PingPong() will start at the correct position
		counter = Mathf.Round (counter);

		// round the yPos to get an integer value for the targetY, which is passed back to the Aimer
		float yPos = Mathf.Round(transform.position.y);
		targetY = yPos;

		StartCoroutine ("smoothSnap", yPos);
	}
	
	// Use Mathf.SmoothDamp for smooth snapping to integer y position
	IEnumerator smoothSnap(float yDest)
	{
		float currentY = this.transform.position.y;
		float velocity = 0.0f;
		while(Mathf.Abs (currentY - yDest) > 0.01)
		{
			currentY = this.transform.position.y;
			float yPos = Mathf.SmoothDamp(currentY, yDest, ref velocity, 0.05f);
			transform.position = new Vector3(transform.position.x, yPos);
			yield return null;
		}
		transform.position = new Vector3(transform.position.x, yDest);
	}
}
