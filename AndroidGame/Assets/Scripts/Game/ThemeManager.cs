using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThemeManager : MonoBehaviour {

	public static ThemeManager instance;

	public Theme[] themes;
	public int themeIndex;

	void Awake()
	{
		// Make this a singleton
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		
		DontDestroyOnLoad(gameObject);
	}
}
