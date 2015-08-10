using UnityEngine;
using System.Collections;

public class Aimer : MonoBehaviour {

	// Aimer Prefabs: Horizontal, Vertical, Center
	public AimerHorizontal aimerH;
	public AimerVertical aimerV;
	public AimerCenter aimerC;

	// time in seconds to wait before the aimer moves
	public float aimerSpeed = 8.0f;
	
	public bool aimed = false;

	public int targetX;
	public int targetY;

	// Use this for initialization
	void Start () {
		aimerH.speed = aimerSpeed;
		aimerV.speed = aimerSpeed;

		// give aimerH and aimerV references to aimerC to set its position
		aimerH.aimerC = this.aimerC;
		aimerV.aimerC = this.aimerC;
	}

	public void Aim()
	{
		StartCoroutine("AimH");
	}

	IEnumerator AimH()
	{
		// set aimerH active and to aiming mode
		aimerH.gameObject.SetActive (true);
		aimerH.aiming = true;
		// if the mouse button isn't pressed, do nothing
		while (!Input.GetMouseButtonDown(0) &&
		       !getTouchInput())
		{
			yield return null;
		}
		// stops aiming mode and snaps aimerH to an integer x and y position
		aimerH.aiming = false;
		aimerH.snap();

		// wait for end of frame so the same input isn't registered twice
		yield return new WaitForEndOfFrame();

		StartCoroutine("AimV");
	}

	IEnumerator AimV()
	{
		aimerV.gameObject.SetActive (true);
		aimerC.GetComponent<SpriteRenderer>().enabled = true;

		aimerV.aiming = true;
		// if the mouse button isn't pressed, do nothing
		while (!Input.GetMouseButtonDown(0) &&
		       !getTouchInput())
		{
			aimerC.setX (aimerV.transform.position.x);
			yield return null;
		}
		aimerV.aiming = false;
		aimerV.snap();

		// set the target coordinates
		targetY = (int)aimerH.targetY;
		targetX = (int)aimerV.targetX;

		// set this aimer to aimed mode
		aimed = true;
	}

	bool getTouchInput()
	{
		return Input.touchCount > 0 && 
			Input.GetTouch(0).phase == TouchPhase.Began;
	}

	// controls the AimerCenter to animate correctly depending on the button hit
	public void hitTarget(bool hit)
	{
		aimerC.ShowIndicator(hit);
	}
}
