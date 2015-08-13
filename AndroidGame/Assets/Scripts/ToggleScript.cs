using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleScript : MonoBehaviour {

	public Toggle toggle;
	public Image toggleImage;
	
	public Sprite onImage;
	public Sprite offImage;

	// Update is called once per frame
	void Update () {
		if (toggle.isOn)
			toggleImage.sprite = onImage;
		else
			toggleImage.sprite = offImage;
	}
}
