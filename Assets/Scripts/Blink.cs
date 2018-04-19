using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
	void Start ()
	{
		gameObject.SetActive (false);
		InvokeRepeating ("blink", 2, 0.7f);
	}

	private void blink ()
	{
		if (gameObject.activeSelf) {
			gameObject.SetActive (false);
		} else {
			gameObject.SetActive (true);
		}
	}
}
