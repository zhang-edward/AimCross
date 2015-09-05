using UnityEngine;
using System.Collections;

public class Floor : BoardTile {

	void Start()
	{
		anim.clip = ThemeManager.instance.themes[ThemeManager.instance.themeIndex].floorPressed;
	}

	public override void Hit ()
	{
		animate ();
	}
}
