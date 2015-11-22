using UnityEngine;
using System.Collections;

public abstract class AimerBar : MonoBehaviour {

	// References to Board object properties
	public Board board;
	protected int boardSize;
	
	// Reference to the AimerCenter object to set its position
	public AimerCenter aimerC;
	
	public bool aiming;		// whether this aimer bar is aiming
	protected bool paused;
	public bool Paused{
		set{paused = value;}
	}
	
	protected float speed;		// speed of the aimer (higher = faster)
	public float Speed{
		set{speed = value;}
	}

	public GameObject aimerPrefab;
	public int sortingOrder;

	/* Custom timer for Mathf.PingPong when the aimer is aiming
	 * Counter will stop when the aimer stops, thus conserving the
	 * position of the aimer so when it calls Mathf.PingPong again,
	 * it will continue from its current position
	 * 
	 * This is needed because Mathf.PingPong is a simple function
	 * */
	protected float counter = 0.0f;
}
