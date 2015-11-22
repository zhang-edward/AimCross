using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextScript : MonoBehaviour {

	public Text shadow;

	void Update()
	{
		shadow.text = this.GetComponent<Text>().text;
	}
}
