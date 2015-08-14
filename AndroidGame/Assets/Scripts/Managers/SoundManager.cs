using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioSource boardEfx;
	public AudioSource gameEfx;

	public static SoundManager instance = null;

	public AudioClip uiSound;

	public float lowPitchRange = 0.95f;
	public float highPitchRange = 1.05f;

	// Use this for initialization
	void Awake () {

		// Make this a singleton
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad(gameObject);
	}

	void Update()
	{
		if (PlayerPrefs.GetInt("Mute") == 1)
			boardEfx.mute = true;
		else
			boardEfx.mute = false;
	}

	public void RandomizeSfxBoard(AudioClip clip)
	{
		// randomize the pitch of each sound a little bit
		float randomPitch = Random.Range (lowPitchRange, highPitchRange);
		boardEfx.pitch = randomPitch;
		boardEfx.clip = clip;
		boardEfx.Play();
	}

	public void PlaySingleBoard(AudioClip clip)
	{
		boardEfx.clip = clip;
		boardEfx.Play();
	}

	public void RandomizeSfxGame(AudioClip clip)
	{
		// randomize the pitch of each sound a little bit
		float randomPitch = Random.Range (lowPitchRange, highPitchRange);
		gameEfx.pitch = randomPitch;
		gameEfx.clip = clip;
		gameEfx.Play();
	}
	
	public void PlaySingleGame(AudioClip clip)
	{
		gameEfx.clip = clip;
		gameEfx.Play();
	}

	public void UiSound()
	{
		gameEfx.clip = uiSound;
		gameEfx.Play ();
	}
}
