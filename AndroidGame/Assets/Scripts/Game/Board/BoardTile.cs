using UnityEngine;
using System.Collections;

public abstract class BoardTile : MonoBehaviour {

	public Board board;
	protected Animator anim;

	public AudioClip click;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void animate()
	{
		anim.SetTrigger ("Hit");
		SoundManager.instance.PlaySingle(click);
	}

	public abstract void Hit();
}