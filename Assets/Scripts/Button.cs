using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
	public AudioSource introSoundEffect;
	private LevelManager lvl;
	// Use this for initialization
	void Start ()
	{
		lvl = FindObjectOfType<LevelManager> ();
		Invoke ("buttonStart", 2);
		gameObject.SetActive (false);
		DontDestroyOnLoad (introSoundEffect);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Return)) {
			introSoundEffect.Play ();
			lvl.Load ("First");
		}
	}

	private void buttonStart ()
	{
		gameObject.SetActive (true);
	}
}
