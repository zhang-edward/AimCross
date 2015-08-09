using UnityEngine;
using System.Collections;

public class AimerVertical : MonoBehaviour {
	
	public Board board;
	int boardSize;
	
	public bool aiming;
	public bool movingUp;
	public float speed;
	
	public GameObject prefab;

	// The x coordinate after this aimer has stopped
	public float targetX;
	
	private float counter = 0.0f;
	
	void Start()
	{
		boardSize = Board.boardSize;
		for (int i = -boardSize / 2; i < boardSize / 2; i ++)
		{
			GameObject o = Instantiate (prefab, this.transform.position, Quaternion.identity) as GameObject;
			Vector3 localPos = new Vector3(0, i, 0);
			o.transform.SetParent(this.transform);
			o.transform.localPosition = localPos;
		}
	}
	
	void Update()
	{
		if (aiming)
		{
			counter += speed * Time.deltaTime;
			float xPos = Mathf.PingPong(counter, boardSize - 1);
			transform.position = new Vector3(xPos, transform.position.y);
		}
	}
	
	public void snapToX()
	{
		float xPos = Mathf.Round(transform.position.x);
		counter = Mathf.Round (counter);
		targetX = xPos;
		/*transform.position = new Vector3(xPos, transform.position.y);*/
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
	}
}
