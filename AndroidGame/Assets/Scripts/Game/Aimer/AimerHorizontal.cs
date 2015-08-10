using UnityEngine;
using System.Collections;

public class AimerHorizontal : MonoBehaviour {

	// References to Board object properties
	public Board board;
	int boardSize;

	// Reference to the AimerCenter object to set its position
	public AimerCenter aimerC;
	
	public bool aiming;		// whether this aimer bar is aiming
	public float speed;		// speed of the aimer (higher = faster)

	// prefabs for left, middle, and right sprites
	public GameObject prefabLeft;
	public GameObject prefabMid;
	public GameObject prefabRight;

	// The y coordinate after this aimer has finished aiming
	public float targetY;

	/* Custom timer for Mathf.PingPong when the aimer is aiming
	 * Counter will stop when the aimer stops, thus conserving the
	 * position of the aimer so when it calls Mathf.PingPong again,
	 * it will continue from its current position
	 * 
	 * This is needed because Mathf.PingPong is a simple function
	 * */
	private float counter = 0.0f;

	// TODO: create an effect for when the bar appears in OnEnabled()

	void Awake()
	{
		// Create the sprites that make up the bar
		boardSize = Board.boardSize;

		CreateAimerPiece (prefabLeft, -boardSize / 2);
		CreateAimerPiece (prefabRight, boardSize / 2 - 1);
		// the starting and stopping i values are +1 and -1 to exclude the left and right pieces
		for (int i = -boardSize / 2 + 1; i < boardSize / 2 - 1; i ++)
		{
			CreateAimerPiece(prefabMid, i);
		}
	}

	void CreateAimerPiece(GameObject prefab, int xPos)
	{
		// set the World Position to this instance
		GameObject o = Instantiate (prefab, this.transform.position, Quaternion.identity) as GameObject;

		// set the Local Position to the xPos specified
		Vector3 localPos = new Vector3(xPos, 0, 0);

		// set this to be the object's parent and set local position
		o.transform.SetParent(this.transform);
		o.transform.localPosition = localPos;
	}

	void Update()
	{
		// use Mathf.PingPong() to make the aimer back and forth along the board
		if (aiming)
		{
			counter += speed * Time.deltaTime;
			float yPos = Mathf.PingPong(counter, boardSize - 1);
			setPosition(new Vector3(transform.position.x, yPos));
		}
	}

/*	public void snapToY()
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
	}*/

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
