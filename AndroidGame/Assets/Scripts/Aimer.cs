using UnityEngine;
using System.Collections;

public class Aimer : MonoBehaviour {

	// Aimer Prefabs: Horizontal, Vertical, Center
	public AimerHorizontal aimerH;
	public AimerVertical aimerV;
	public GameObject aimerCPrefab;
	
	// time in seconds to wait before the aimer moves
	public float aimerSpeed = 8.0f;
	
	public bool aimed = false;

	public int targetX;
	public int targetY;

	// Use this for initialization
	void Start () {
		aimerH.speed = aimerSpeed;
		aimerV.speed = aimerSpeed;
	}

	public void Aim()
	{
		StartCoroutine("AimH");
	}

	IEnumerator AimH()
	{
		aimerH.gameObject.SetActive (true);
		aimerH.aiming = true;
		// if the mouse button isn't pressed, do nothing
		while (!Input.GetMouseButtonDown(0) &&
		       !getTouchInput())
		{
			yield return null;
		}
		aimerH.aiming = false;
		aimerH.snapToY();

		// wait for end of frame so the same input isn't registered twice
		yield return new WaitForEndOfFrame();

		StartCoroutine("AimV");
	}

	IEnumerator AimV()
	{
		aimerV.gameObject.SetActive (true);
		aimerV.aiming = true;
		// if the mouse button isn't pressed, do nothing
		while (!Input.GetMouseButtonDown(0) &&
		       !getTouchInput())
		{
			yield return null;
		}
		aimerV.aiming = false;
		aimerV.snapToX();

		targetY = (int)aimerH.targetY;
		targetX = (int)aimerV.targetX;
		aimed = true;
	}

	bool getTouchInput()
	{
		return Input.touchCount > 0 && 
			Input.GetTouch(0).phase == TouchPhase.Began;
	}
}
