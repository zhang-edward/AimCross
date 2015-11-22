using UnityEngine;
using System.Collections;

public class AimerCenter : MonoBehaviour {

	Animator anim;

	void Awake()
	{
		anim = this.transform.GetComponent<Animator>();
	}

	public void ShowIndicator(bool hit)
	{
		if (hit)
			anim.SetTrigger ("HitGreen");
		else
			anim.SetTrigger ("HitRed");
	}

	public void setX(float x)
	{
		transform.position = new Vector3(x, transform.position.y);
	}

	public void setY(float y)
	{
		transform.position = new Vector3(transform.position.x, y);
	}

	public void snap()
	{
		StartCoroutine("smoothSnap");
	}

	// Use Mathf.SmoothDamp for smooth snapping to integer y position
	IEnumerator smoothSnap()
	{
		Vector3 destPos = new Vector3(Mathf.Round (transform.position.x),
		                              Mathf.Round (transform.position.y));
		Vector3 velocity = Vector3.zero;
		while(Vector3.Distance (transform.position, destPos) > Mathf.Epsilon)
		{
			transform.position = Vector3.SmoothDamp(transform.position, destPos, ref velocity, 0.05f);
			yield return null;
		}
		transform.position = destPos;
	}
}
