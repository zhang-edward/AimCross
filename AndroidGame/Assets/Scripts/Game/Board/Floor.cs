using UnityEngine;
using System.Collections;

public class Floor : BoardTile {

	public override void Hit ()
	{
		Debug.LogError ("Floor should not call Hit method!");
	}
}
