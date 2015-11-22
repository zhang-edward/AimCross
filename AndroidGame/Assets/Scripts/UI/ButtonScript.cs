using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerClickHandler {

	public void OnPointerClick(PointerEventData data)
	{
		SoundManager.instance.UiSound();
	}
}
