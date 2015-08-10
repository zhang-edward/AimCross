using UnityEngine;
using System.Collections;

public class Aimer : MonoBehaviour {

	// Aimer Prefabs: Horizontal, Vertical, Center
	public AimerHorizontal aimerH;
	public AimerVertical aimerV;
	public AimerCenter aimerC;

	// time in seconds to wait before the aimer moves
	public float aimerSpeed = 8.0f;

	public bool aimingH = false;		// whether this aimer is currently aiming the horizontal aimer
	public bool aimingV = false;		// whether this aimer is currently aiming the vertical aimer
	public bool aimed = false;

	public int targetX;
	public int targetY;

	public bool paused;

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

	void Update()
	{
		if (paused)
		{
			aimerH.aiming = false;
			aimerV.aiming = false;
		}
		else
		{
			if (aimingH)
				aimerH.aiming = true;
			else if (aimingV)
				aimerV.aiming = true;
		}
	}

	IEnumerator AimH()
	{
		aimingH = true;

		// set aimerH active and to aiming mode
		aimerH.gameObject.SetActive (true);
		aimerH.aiming = true;
		// if the mouse button isn't pressed or if paused, do nothing
		while (!GameManager.getInput() || paused)
		{
			yield return null;
		}
		// wait for end of frame so the same input isn't registered twice
		yield return new WaitForSeconds(1.0f/60.0f);
		
		// stops aiming mode and snaps aimerH to an integer x and y position
		aimerH.aiming = false;
		aimerH.snap();

		aimingH = false;

		StartCoroutine("AimV");
	}

	IEnumerator AimV()
	{
		aimingV = true;

		aimerV.gameObject.SetActive (true);
		aimerC.GetComponent<SpriteRenderer>().enabled = true;

		aimerV.aiming = true;
		// if the mouse button isn't pressed or if paused, do nothing
		while (!GameManager.getInput() || paused)
		{
			yield return null;
		}
		// wait for end of frame so the same input isn't registered twice
		yield return new WaitForSeconds(1.0f/60.0f);

		aimerV.aiming = false;
		aimerV.snap();

		// set the target coordinates
		targetY = (int)aimerH.targetY;
		targetX = (int)aimerV.targetX;

		aimingV = false;
		// set this aimer to aimed mode
		aimed = true;
	}

	// controls the AimerCenter to animate correctly depending on the button hit
	public void hitTarget(bool hit)
	{
		aimerC.ShowIndicator(hit);
	}

	public void disableAimers()
	{
		aimerH.gameObject.SetActive(false);
		aimerV.gameObject.SetActive(false);
		aimerC.GetComponent<SpriteRenderer>().enabled = false;
	}
}
