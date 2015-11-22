using UnityEngine;
using System.Collections;

public class AimerHorizontal : AimerBar {

	// prefabs for left, middle, and right sprites
	public Sprite left;
	public Sprite mid;
	public Sprite right;
/*	public GameObject prefabLeft;
	public GameObject prefabMid;
	public GameObject prefabRight;*/

	// The y coordinate after this aimer has finished aiming
	public float targetY;



	// TODO: create an effect for when the bar appears in OnEnabled()

	void Awake()
	{
		// Create the sprites that make up the bar
		boardSize = Board.boardSize;

		CreateAimerPiece (aimerPrefab, -boardSize / 2, 0);
		CreateAimerPiece (aimerPrefab, boardSize / 2 - 1, 2);
		// the starting and stopping i values are +1 and -1 to exclude the left and right pieces
		for (int i = -boardSize / 2 + 1; i < boardSize / 2 - 1; i ++)
		{
			CreateAimerPiece(aimerPrefab, i, 1);
		}
	}

	// for param pos, 0 is left, 1 is mid, 2 is right
	void CreateAimerPiece(GameObject prefab, int xPos, int pos)
	{
		// set the World Position to this instance
		GameObject o = Instantiate (prefab, this.transform.position, Quaternion.identity) as GameObject;

		// set the appropriate sprite
		if (pos == 0)
			o.GetComponent<SpriteRenderer>().sprite = left;
		else if (pos == 1)
			o.GetComponent<SpriteRenderer>().sprite = mid;
		else if (pos == 2)
			o.GetComponent<SpriteRenderer>().sprite = right;
		o.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;

		// set the Local Position to the xPos specified
		Vector3 localPos = new Vector3(xPos, 0, 0);

		// set this to be the object's parent and set local position
		o.transform.SetParent(this.transform);
		o.transform.localPosition = localPos;
	}

	void Update()
	{
		// use Mathf.PingPong() to make the aimer back and forth along the board
		if (aiming && !paused)
		{
			counter += speed * Time.deltaTime;
			float yPos = Mathf.PingPong(counter, boardSize - 1);
			setPosition(new Vector3(transform.position.x, yPos));
		}
	}

	public void snap()
	{
		// round the counter so that Mathf.PingPong() will start at the correct position
		counter = Mathf.Round (counter);

		// round the yPos to get an integer value for the targetY, which is passed back to the Aimer
		float yPos = Mathf.Round(transform.position.y);
		targetY = yPos;

		StartCoroutine("smoothSnap");
	}

	// Use Mathf.SmoothDamp for smooth snapping to integer y position
	IEnumerator smoothSnap()
	{
		Vector3 destPos = new Vector3(Mathf.Round (transform.position.x),
		                              Mathf.Round (transform.position.y));
		Vector3 velocity = Vector3.zero;

		/*
		 * this counter prevents the player from tapping very fast and causing
		 * the aimer to start aiming before it finishes snapping, therefore causing
		 * this coroutine to try to snap the aimer back in place while it tries to 
		 * aim, causing erratic movements
		 * */
		float counter = 0.0f;
		while(Vector3.Distance (transform.position, destPos) > Mathf.Epsilon &&
		      counter <= 0.05f)
		{
			counter += Time.deltaTime;
			setPosition(Vector3.SmoothDamp(transform.position, destPos, ref velocity, 0.05f));
			yield return null;
		}
		setPosition(destPos);
		yield return null;
	}

	void setPosition(Vector3 pos)
	{
		transform.position = pos;
		// aimerC is updated whenever this transform position is updated
		aimerC.setY(this.transform.position.y);
	}
}
