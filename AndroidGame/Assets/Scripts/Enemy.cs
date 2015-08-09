using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {


	public void Destroy()
	{
		gameObject.SetActive(false);
	}

}
