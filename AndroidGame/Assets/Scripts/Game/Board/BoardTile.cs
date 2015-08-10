using UnityEngine;
using System.Collections;

public abstract class BoardTile : MonoBehaviour {

	public Board board;
	protected Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void animate()
	{
		anim.SetTrigger ("Hit");
	}

	public abstract void Hit();
}