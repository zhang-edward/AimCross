using UnityEngine;
using System.Collections;

public abstract class BoardTile : MonoBehaviour {

	public Board board;
	protected Animation anim;

	// how many points this tile is worth (for the enemy tiles)
	public int pointValue;

	public AudioClip click;

	void Awake()
	{
		anim = GetComponent<Animation>();
	}


	public void animate()
	{
		anim.Play();
		SoundManager.instance.RandomizeSfxBoard(click);
	}

	public abstract void Hit();
}