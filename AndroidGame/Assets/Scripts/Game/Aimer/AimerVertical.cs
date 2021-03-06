using UnityEngine;
using System.Collections;

public class AimerVertical : AimerBar {

	public Sprite top;
	public Sprite mid;
	public Sprite bottom;
	public GameObject aimerPiece;
/*	public GameObject prefabTop;
	public GameObject prefabMid;
	public GameObject prefabBottom;*/

	// The x coordinate after this aimer has stopped
	public float targetX;
	
	void Awake()
	{
		// Create the sprites that make up the bar
		boardSize = Board.boardSize;
		
		CreateAimerPiece (aimerPiece, -boardSize / 2, 2);
		CreateAimerPiece (aimerPiece, boardSize / 2 - 1, 0);
		// the starting and stopping i values are +1 and -1 to exclude the left and right pieces
		for (int i = -boardSize / 2 + 1; i < boardSize / 2 - 1; i ++)
		{
			CreateAimerPiece(aimerPiece, i, 1);
		}
	}

	// for param pos, 0 is top, 1 is mid, 2 is bottom
	void CreateAimerPiece(GameObject prefab, int yPos, int pos)
	{
		// set the World Position to this instance
		GameObject o = Instantiate (prefab, this.transform.position, Quaternion.identity) as GameObject;

		// set the appropriate sprite
		if (pos == 0)
			o.GetComponent<SpriteRenderer>().sprite = top;
		else if (pos == 1)
			o.GetComponent<SpriteRenderer>().sprite = mid;
		else if (pos == 2)
			o.GetComponent<SpriteRenderer>().sprite = bottom;
		o.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;

		// set the Local Position to the xPos specified
		Vector3 localPos = new Vector3(0, yPos, 0);
		
		// set this to be the object's parent and set local position
		o.transform.SetParent(this.transform);
		o.transform.localPosition = localPos;
	}
	
	void Update()
	{
		if (aiming && !paused)
		{
			counter += speed * Time.deltaTime;
			float xPos = Mathf.PingPong(counter, boardSize - 1);
			setPosition(new Vector3(xPos, transform.position.y));
		}
	}
	
	/*public void snapToX()
	{
		float xPos = Mathf.Round(transform.position.x);
		counter = Mathf.Round (counter);
		targetX = xPos;
		StartCoroutine ("smoothSnap", xPos);
	}

	IEnumerator smoothSnap(float xDest)
	{
		float currentX = this.transform.position.x;
		float velocity = 0.0f;
		while(Mathf.Abs (currentX - xDest) > 0.01)
		{
			currentX = this.transform.position.x;
			float xPos = Mathf.SmoothDamp(currentX, xDest, ref velocity, 0.05f);
			transform.position = new Vector3(xPos, transform.position.y);
			yield return null;
		}
		transform.position = new Vector3(xDest, transform.position.y);
	}*/

	public void snap()
	{
		counter = Mathf.Round (counter);

		float xPos = Mathf.Round(transform.position.x);
		targetX = xPos;

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
		aimerC.setX(this.transform.position.x);
	}
}
