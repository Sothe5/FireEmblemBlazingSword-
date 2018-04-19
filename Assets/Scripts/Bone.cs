using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour {

	private Cursor myCursor;
	// Use this for initialization
	void Start () {	
		myCursor = FindObjectOfType<Cursor> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ActualizarVidaAllie(){
		myCursor.UpdateAllieHp ();
	}

	public void ActualizarVidaEnemy(){
		myCursor.UpdateEnemyHp ();
	}
}
