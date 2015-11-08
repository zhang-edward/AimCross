using UnityEngine;
using System.Collections;

public class Floor : BoardTile {

	public override void Hit ()
	{
		animate ();
	}
}
