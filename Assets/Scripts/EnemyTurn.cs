using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyTurn : MonoBehaviour {

	private Map myMap;
	private Inventory myInventory;
	private bool working;
	private bool isattackingAllie;
	private bool isAccion;
	private Cursor cursor;

	public GameObject panelCambioDeTurno;
	public GameObject panelCambioDeTurnoRe;

	public void Start () {
		myMap = FindObjectOfType<Map> ();
		myInventory = FindObjectOfType<Inventory> ();
		cursor = FindObjectOfType<Cursor> ();
		working = false;
		isattackingAllie = false;
	}

	public void Development () {
		StartCoroutine (ConcurrentDevelopment ());
	}

	public IEnumerator ConcurrentDevelopment () { // asegurarse que para cada enemigo se le vuelve a setear a OCUPIED al final
		cursor.EnemyBackgroundSound.Play ();
		cursor.AllyBackgroundSound.Stop ();
		panelCambioDeTurno.SetActive (true);
		yield return new WaitForSecondsRealtime (2f);
		panelCambioDeTurno.SetActive (false);
		yield return new WaitForSecondsRealtime (1f);

		foreach (Enemy e in FindObjectsOfType<Enemy>()) {
			myMap.matrixScenary [Mathf.FloorToInt (e.transform.position.x)] [Mathf.FloorToInt (e.transform.position.y)].setType (Types.Free);
		}

		foreach (Allie a in FindObjectsOfType<Allie>()) {
			myMap.matrixScenary [Mathf.FloorToInt (a.transform.position.x)] [Mathf.FloorToInt (a.transform.position.y)].setType (Types.Ocupaid);
		}

		foreach (Enemy e in FindObjectsOfType<Enemy>()) { 
			while (working) {
				yield return null;
			}
			working = true;

			StartCoroutine (accion (e));
			while (isAccion) {
				yield return null;
			}

			yield return new WaitForSecondsRealtime (0.5f); // sino cambialo a 1 esta porque el destroy del allie al reciveDmg no se actualiza rapido
			if (e.AmIAlive ()) {
				e.setDisponibility (false);
			}

			if (cursor.AllieWin ()) {
				cursor.PanelVictoriaDerrota.SetActive (true);
				cursor.VictoriaDerrota.text = "¡VICTORIA!";
				cursor.AllyBackgroundSound.Stop ();
				cursor.EnemyBackgroundSound.Stop ();
				cursor.VictorySound.Play ();
				cursor.setState (-1);
				Invoke ("BackToTittle", 9);
				break;
			} else if (cursor.EnemyWin ()) {
				cursor.PanelVictoriaDerrota.SetActive (true);
				cursor.VictoriaDerrota.text = "¡DERROTA!";
				cursor.AllyBackgroundSound.Stop ();
				cursor.EnemyBackgroundSound.Stop ();
				cursor.DefeatSound.Play ();
				cursor.setState (-1);
				Invoke ("BackToTittle", 10);
				break;
			}

		}

		while (working) {
			yield return null;
		}
		if (!cursor.AllieWin () && !cursor.EnemyWin ()) {
			foreach (Enemy e in FindObjectsOfType<Enemy>()) {
				myMap.matrixScenary [Mathf.FloorToInt (e.transform.position.x)] [Mathf.FloorToInt (e.transform.position.y)].setType (Types.Ocupaid);
			}

			foreach (Allie a in FindObjectsOfType<Allie>()) {
				myMap.matrixScenary [Mathf.FloorToInt (a.transform.position.x)] [Mathf.FloorToInt (a.transform.position.y)].setType (Types.Free);
			}
			cursor.AllyBackgroundSound.Play ();
			cursor.EnemyBackgroundSound.Stop ();
			panelCambioDeTurnoRe.SetActive (true);
			yield return new WaitForSecondsRealtime (2f);
			panelCambioDeTurnoRe.SetActive (false);
			yield return new WaitForSecondsRealtime (0.25f);
			cursor.panelTerrainInfo.SetActive (true);
			cursor.panelPersonajeInfo.SetActive (true);
			cursor.UpdatePersonajeInfo ();
			cursor.panelObjective.SetActive (true);
			cursor.GetComponent<SpriteRenderer> ().sortingOrder = 4;
			myMap.EndTurnEnemy ();

			cursor.setState (0);
		}

	}


	public IEnumerator accion (Enemy myEnemy) {
		isAccion = true;
		List<IAstate> estados = new List<IAstate> ();
		List<IAstate> aux = new List<IAstate> ();
		List<IAstate> bloqueado = new List<IAstate> ();

		int attackDistance = 0;
		foreach (Item i in myEnemy.items) {
			attackDistance += i.AttackDistance;
		}

		if (attackDistance == myEnemy.items.Count) {
			attackDistance = 1;
		} else if (attackDistance == 2 * myEnemy.items.Count) {
			attackDistance = 2;
		} else {
			attackDistance = 12;
		}



		foreach (Allie a in FindObjectsOfType<Allie>()) {
			if (attackDistance == 1 || attackDistance == 12) {
				foreach (Spot s in myMap.matrixScenary [Mathf.FloorToInt (a.transform.position.x)] [Mathf.FloorToInt (a.transform.position.y)].getNeighbors (myMap.matrixScenary,myMap.Right,myMap.Up)) {
					estados.Add (new IAstate (s, a, attackDistance));
				}
			}
			if (attackDistance == 2 || attackDistance == 12) {
				foreach (Spot s in myMap.matrixScenary [Mathf.FloorToInt (a.transform.position.x)] [Mathf.FloorToInt (a.transform.position.y)].getFarNeighbors (myMap.matrixScenary,myMap.Right,myMap.Up)) {
					estados.Add (new IAstate (s, a, attackDistance));
				}
			}
		}

		foreach (IAstate state in estados) {
			List<Spot> path = new List<Spot> ();
			if (!myMap.isPathFinding (Mathf.FloorToInt (myEnemy.transform.position.x), Mathf.FloorToInt (myEnemy.transform.position.y), state.finalPos.x, state.finalPos.y, myEnemy.movement, path)
			    || myMap.GetValue (state.finalPos.x, state.finalPos.y) == Types.Allie
			    || (myMap.GetValue (state.finalPos.x, state.finalPos.y) == Types.Enemy
			    && (Mathf.FloorToInt (myEnemy.transform.position.x) != state.finalPos.x || Mathf.FloorToInt (myEnemy.transform.position.y) != state.finalPos.y))) {
				aux.Add (state);
			}
			if (myMap.isPathFinding (Mathf.FloorToInt (myEnemy.transform.position.x), Mathf.FloorToInt (myEnemy.transform.position.y), state.finalPos.x, state.finalPos.y, myEnemy.movement, path)
			    && (myMap.GetValue (state.finalPos.x, state.finalPos.y) == Types.Enemy
			    && (Mathf.FloorToInt (myEnemy.transform.position.x) != state.finalPos.x || Mathf.FloorToInt (myEnemy.transform.position.y) != state.finalPos.y))) {
				bloqueado.Add (state);
			}
		}
		foreach (IAstate state in aux) {
			estados.Remove (state);
		}


		if (estados.Count == 0) { 
			if (bloqueado.Count == 0) {
				MoveToClosestAllie (myEnemy);
			} else {
				MoveToClosestAllieBeingInRange (myEnemy);
			}
		} else { 
			IAstate optimo = new IAstate ();
			foreach (IAstate state in estados) {
				state.calculateValue (myEnemy, myInventory, myMap);
				if (state.valor > optimo.valor) {
					optimo = state;
				}
			}
			StartCoroutine (AttackAnAllie (myEnemy, optimo));
			while (isattackingAllie) {
				yield return null;
			}
		}
		yield return new WaitForSecondsRealtime (0.5f);
		isAccion = false;
	}


	private void MoveToClosestAllieBeingInRange (Enemy myEnemy) {
		int menorDistancia = int.MaxValue;
		Allie target = null;
		int myEnemyX = Mathf.FloorToInt (myEnemy.transform.position.x);
		int myEnemyY = Mathf.FloorToInt (myEnemy.transform.position.y);

		foreach (Allie a in FindObjectsOfType<Allie>()) {
			List<Spot> path1 = new List<Spot> ();
			myMap.matrixScenary [Mathf.FloorToInt (a.transform.position.x)] [Mathf.FloorToInt (a.transform.position.y)].setType (Types.Free);
			myMap.isPathFinding (myEnemyX, myEnemyY, Mathf.FloorToInt (a.transform.position.x), Mathf.FloorToInt (a.transform.position.y), myEnemy.movement, path1);
			myMap.matrixScenary [Mathf.FloorToInt (a.transform.position.x)] [Mathf.FloorToInt (a.transform.position.y)].setType (Types.Ocupaid);
			if (path1.Count != 0) {
				if (path1 [0].g < menorDistancia) {
					target = a;
					menorDistancia = path1 [0].g;
				}
			}
		}

		List<Spot> path = new List<Spot> ();

		myMap.matrixScenary [Mathf.FloorToInt (target.transform.position.x)] [Mathf.FloorToInt (target.transform.position.y)].setType (Types.Free);
		myMap.isPathFinding (myEnemyX, myEnemyY, Mathf.FloorToInt (target.transform.position.x), Mathf.FloorToInt (target.transform.position.y), myEnemy.movement, path);
		myMap.matrixScenary [Mathf.FloorToInt (target.transform.position.x)] [Mathf.FloorToInt (target.transform.position.y)].setType (Types.Ocupaid);
		if (path.Count > 1) {
			int i = 1;
			List<Spot> aux = new List<Spot> ();
			aux.Add (path [0]);
			while (myMap.GetValue (path [i].x, path [i].y) == Types.Enemy) {
				aux.Add (path [i]);
				i++;
			}
			foreach (Spot s in aux) {
				path.Remove (s);
			}

			StartCoroutine (AnimationMovement (path, myEnemy));
		}
	}




	private void MoveToClosestAllie (Enemy myEnemy) {
		int menorDistancia = int.MaxValue;
		Allie target = null;
		int myEnemyX = Mathf.FloorToInt (myEnemy.transform.position.x);
		int myEnemyY = Mathf.FloorToInt (myEnemy.transform.position.y);

		foreach (Allie a in FindObjectsOfType<Allie>()) {
			List<Spot> path1 = new List<Spot> ();
			myMap.matrixScenary [Mathf.FloorToInt (a.transform.position.x)] [Mathf.FloorToInt (a.transform.position.y)].setType (Types.Free);
			myMap.isPathFinding (myEnemyX, myEnemyY, Mathf.FloorToInt (a.transform.position.x), Mathf.FloorToInt (a.transform.position.y), myEnemy.movement, path1);
			myMap.matrixScenary [Mathf.FloorToInt (a.transform.position.x)] [Mathf.FloorToInt (a.transform.position.y)].setType (Types.Ocupaid);
			if (path1.Count != 0) {
				if (path1 [0].g < menorDistancia) {
					target = a;
					menorDistancia = path1 [0].g;
				}
			}
		}

		List<Spot> path = new List<Spot> ();

		myMap.matrixScenary [Mathf.FloorToInt (target.transform.position.x)] [Mathf.FloorToInt (target.transform.position.y)].setType (Types.Free);
		myMap.isPathFinding (myEnemyX, myEnemyY, Mathf.FloorToInt (target.transform.position.x), Mathf.FloorToInt (target.transform.position.y), myEnemy.movement, path);
		myMap.matrixScenary [Mathf.FloorToInt (target.transform.position.x)] [Mathf.FloorToInt (target.transform.position.y)].setType (Types.Ocupaid);
		if (path.Count != 0) {
			int i = 0;
			while (path [i].g > myEnemy.movement) {
				i++;
			}
			path.RemoveRange (0, i);
			StartCoroutine (AnimationMovement (path, myEnemy));
		}
	}


	private IEnumerator AttackAnAllie (Enemy myEnemy, IAstate optimo) {
		isattackingAllie = true;
		List<Spot> path = new List<Spot> ();
		myMap.isPathFinding (Mathf.FloorToInt (myEnemy.transform.position.x), Mathf.FloorToInt (myEnemy.transform.position.y), optimo.finalPos.x, optimo.finalPos.y, myEnemy.movement, path);

		StartCoroutine (AnimationMovement (path, myEnemy));
		while (working) {
			yield return null;
		}
		cursor.GetComponent<SpriteRenderer> ().sortingOrder = 4;
		cursor.transform.position = optimo.target.transform.position;
		int AllieX = Mathf.FloorToInt (optimo.target.transform.position.x);
		int AllieY = Mathf.FloorToInt (optimo.target.transform.position.y);
		int EnemyX = Mathf.FloorToInt (myEnemy.transform.position.x);
		int EnemyY = Mathf.FloorToInt (myEnemy.transform.position.y);

		if (AllieX == EnemyX - 1) {
			myEnemy.GetComponent<Animator> ().SetTrigger ("A");
		} else if (AllieX == EnemyX + 1) {
			myEnemy.GetComponent<Animator> ().SetTrigger ("D");
		} else if (AllieY == EnemyY - 1) {
			myEnemy.GetComponent<Animator> ().SetTrigger ("S");
		} else if (AllieY == EnemyY + 1) {
			myEnemy.GetComponent<Animator> ().SetTrigger ("W");
		}

		yield return new WaitForSecondsRealtime (0.8f);
		cursor.GetComponent<SpriteRenderer> ().sortingOrder = -3;
		myMap.gameObject.GetComponent<SpriteRenderer> ().sortingOrder = 5;
		cursor.setEnemy (myEnemy);
		cursor.setAllie (optimo.target);
		cursor.MarcoBatalla.SetActive (true);
		cursor.boneAllie.GetComponent<Animator> ().SetTrigger ("Idle" + optimo.target.Name);
		cursor.nombreAliado.text = optimo.target.Name;
		cursor.nombreEnemigo.text = myEnemy.Name;
		cursor.nombreArmaAliado.text = myInventory.getAllieWeapons (optimo.target.ID) [0].Name;
		cursor.nombreArmaEnemigo.text = myEnemy.items [0].Name;
		cursor.vidaAliado.text = optimo.target.Hp.ToString ();
		cursor.vidaEnemigo.text = myEnemy.Hp.ToString ();
		cursor.sliderVidaAliadoFiller.fillAmount = optimo.target.Hp / (float)optimo.target.MaxHp;
		cursor.sliderVidaEnemigoFiller.fillAmount = myEnemy.Hp / (float)myEnemy.MaxHp;
		cursor.imagenArmaAliado.sprite = Resources.Load<Sprite> (myInventory.getAllieWeapons (optimo.target.ID) [0].Image);
		cursor.imagenArmaEnemigo.sprite = Resources.Load<Sprite> (myEnemy.items [0].Image);
		cursor.vGolAliado.text = (optimo.probRecibido * 100).ToString ();
		cursor.vGolEnemigo.text = (optimo.probRealizado * 100).ToString ();
		cursor.vDanAliado.text = optimo.ataqueRecibido.ToString ();
		cursor.vDanEnemigo.text = optimo.ataqueRealizado.ToString ();
		cursor.vCriAliado.text = (optimo.critRecibido * 100).ToString ();
		cursor.vCriEnemigo.text = (optimo.critRealizado * 100).ToString ();
		cursor.platformAllie.sprite = myMap.matrixScenary [Mathf.FloorToInt (optimo.target.transform.position.x)] [Mathf.FloorToInt (optimo.target.transform.position.y)].platformSprite;
		cursor.platformEnemy.sprite = myMap.matrixScenary [Mathf.FloorToInt (myEnemy.transform.position.x)] [Mathf.FloorToInt (myEnemy.transform.position.y)].platformSprite;
		cursor.EnemyBackgroundSound.Pause ();
		cursor.BattleSound.Play ();

		bool hasDodge = false;
		// First Attack
		int criticalMultiplicator = 1;
		if (Random.value < optimo.critRealizado) {
			criticalMultiplicator = 3;
		}

		if (criticalMultiplicator == 1) {
			cursor.boneEnemy.GetComponent<Animator> ().SetTrigger (myEnemy.ID + "Attack");
		} else {
			cursor.boneEnemy.GetComponent<Animator> ().SetTrigger (myEnemy.ID + "Crit");
		}

		if (Random.value < optimo.probRealizado || criticalMultiplicator > 1) {
			optimo.target.reciveDamage (Mathf.FloorToInt (optimo.ataqueRealizado * criticalMultiplicator));
			if (criticalMultiplicator == 1) {
				cursor.hitEnemy.GetComponent<Animator> ().SetTrigger ("Hit");
			} else {
				cursor.hitEnemy.GetComponent<Animator> ().SetTrigger ("CritHit");
			}
		} else {
			cursor.boneAllie.GetComponent<Animator> ().SetTrigger (optimo.target.Name + "Dodge");
			hasDodge = true;
		}

		while (cursor.boneEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			yield return null;
		}
		if (hasDodge) {
			while (cursor.boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle" + optimo.target.Name) || cursor.boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				yield return null;
			}
		}
		hasDodge = false;
		while (!cursor.boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle" + optimo.target.Name)) {
			yield return null;
		}


		while (!cursor.boneEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			yield return null;
		}

		// Response
		if (optimo.target.AmIAlive () && optimo.probRecibido > 0) {
			criticalMultiplicator = 1;
			if (Random.value < optimo.critRecibido) {
				criticalMultiplicator = 3;
			}

			if (criticalMultiplicator == 1) {
				cursor.boneAllie.GetComponent<Animator> ().SetTrigger (optimo.target.Name + "Attack");
			} else {
				cursor.boneAllie.GetComponent<Animator> ().SetTrigger (optimo.target.Name + "Crit");
			}

			if (Random.value < optimo.probRecibido || criticalMultiplicator > 1) {
				myEnemy.reciveDamage (Mathf.FloorToInt (optimo.ataqueRecibido * criticalMultiplicator));
				if (criticalMultiplicator == 1) {
					cursor.hitAllie.GetComponent<Animator> ().SetTrigger ("Hit");
				} else {
					cursor.hitAllie.GetComponent<Animator> ().SetTrigger ("CritHit");
				}
			} else {
				cursor.boneEnemy.GetComponent<Animator> ().SetTrigger (myEnemy.ID + "Dodge");
				hasDodge = true;
			}
		
			while (cursor.boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle" + optimo.target.Name) || cursor.boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				yield return null;
			}

			if (hasDodge) {
				while (cursor.boneEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
					yield return null;
				}
			}
			hasDodge = false;
			while (!cursor.boneEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				yield return null;
			}

			while (!cursor.boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle" + optimo.target.Name)) {
				yield return null;
			}
		
		}
		// Second Attack
		int allieVelocity = optimo.target.Speed - myInventory.getAllieWeapons (optimo.target.ID) [0].Weight;
		int enemyVelocity = myEnemy.Speed - optimo.weapon.Weight;


		if (enemyVelocity - allieVelocity >= 4 && myEnemy.AmIAlive () && optimo.target.AmIAlive ()) {
			criticalMultiplicator = 1;
			if (Random.value < optimo.critRealizado) {
				criticalMultiplicator = 3;
			}

			if (criticalMultiplicator == 1) {
				cursor.boneEnemy.GetComponent<Animator> ().SetTrigger (myEnemy.ID + "Attack");
			} else {
				cursor.boneEnemy.GetComponent<Animator> ().SetTrigger (myEnemy.ID + "Crit");
			}

			if (Random.value < optimo.probRealizado || criticalMultiplicator > 1) {
				optimo.target.reciveDamage (Mathf.FloorToInt (optimo.ataqueRealizado * criticalMultiplicator));
				if (criticalMultiplicator == 1) {
					cursor.hitEnemy.GetComponent<Animator> ().SetTrigger ("Hit");
				} else {
					cursor.hitEnemy.GetComponent<Animator> ().SetTrigger ("CritHit");
				}
			} else {
				cursor.boneAllie.GetComponent<Animator> ().SetTrigger (optimo.target.Name + "Dodge");
				hasDodge = true;
			}

			while (cursor.boneEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				yield return null;
			}

			if (hasDodge) {
				while (cursor.boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle" + optimo.target.Name) || cursor.boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
					yield return null;
				}
			}
			hasDodge = false;

			while (!cursor.boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle" + optimo.target.Name)) {
				yield return null;
			}

			while (!cursor.boneEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				yield return null;
			}
		}

		// Second Response
		if (enemyVelocity - allieVelocity <= -4 && myEnemy.AmIAlive () && optimo.target.AmIAlive () && optimo.probRecibido > 0) {
			criticalMultiplicator = 1;
			if (Random.value < optimo.critRecibido) {
				criticalMultiplicator = 3;
			}

			if (criticalMultiplicator == 1) {
				cursor.boneAllie.GetComponent<Animator> ().SetTrigger (optimo.target.Name + "Attack");
			} else {
				cursor.boneAllie.GetComponent<Animator> ().SetTrigger (optimo.target.Name + "Crit");
			}

			if (Random.value < optimo.probRecibido || criticalMultiplicator > 1) {
				myEnemy.reciveDamage (Mathf.FloorToInt (optimo.ataqueRecibido * criticalMultiplicator));
				if (criticalMultiplicator == 1) {
					cursor.hitAllie.GetComponent<Animator> ().SetTrigger ("Hit");
				} else {
					cursor.hitAllie.GetComponent<Animator> ().SetTrigger ("CritHit");
				}
			} else {
				cursor.boneEnemy.GetComponent<Animator> ().SetTrigger (myEnemy.ID + "Dodge");
				hasDodge = true;
			}

			while (cursor.boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle" + optimo.target.Name) || cursor.boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				yield return null;
			}

			if (hasDodge) {
				while (cursor.boneEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
					yield return null;
				}
			}
			hasDodge = false;

			while (!cursor.boneEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				yield return null;
			}

			while (!cursor.boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle" + optimo.target.Name)) {
				yield return null;
			}
		}
		isattackingAllie = false;
		if (myEnemy.AmIAlive ()) {
			myEnemy.GetComponent<Animator> ().SetTrigger ("BackToIdle");
		}
		yield return new WaitForSecondsRealtime (0.8f);
		cursor.boneAllie.GetComponent<Animator> ().SetTrigger ("BackToIdle");
		myMap.gameObject.GetComponent<SpriteRenderer> ().sortingOrder = 0;
		cursor.MarcoBatalla.SetActive (false);
		cursor.EnemyBackgroundSound.UnPause ();
		cursor.BattleSound.Stop ();
	}


	private IEnumerator AnimationMovement (List<Spot> path, Enemy myEnemy) {
		myMap.SetValue (Mathf.FloorToInt (myEnemy.transform.position.x), Mathf.FloorToInt (myEnemy.transform.position.y), null);
		float counter;
		float x = myEnemy.transform.position.x;
		float y = myEnemy.transform.position.y;

		path.Reverse ();

		for (int i = 1; i < path.Count; i++) {
			counter = 0;
			if (path [i].x > path [i - 1].x) {
				x += 1;

				if (!myEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("MovementD")) {
					myEnemy.GetComponent<Animator> ().SetTrigger ("D");
				}

				while (counter < 1) {
					myEnemy.transform.position += new Vector3 (0.1f, 0, 0);
					counter += 0.1f;
					yield return new WaitForSecondsRealtime (0.015f);
				}
			} else if (path [i].x < path [i - 1].x) {
				x -= 1;

				if (!myEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("MovementA")) {
					myEnemy.GetComponent<Animator> ().SetTrigger ("A");
				}


				while (counter < 1) {
					myEnemy.transform.position += new Vector3 (-0.1f, 0, 0);
					counter += 0.1f;
					yield return new WaitForSecondsRealtime (0.015f);
				}
			} else if (path [i].y > path [i - 1].y) {
				y += 1;

				if (!myEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("MovementW")) {
					myEnemy.GetComponent<Animator> ().SetTrigger ("W");
				}

				while (counter < 1) {
					myEnemy.transform.position += new Vector3 (0, 0.1f, 0);
					counter += 0.1f;
					yield return new WaitForSecondsRealtime (0.015f);
				}
			} else if (path [i].y < path [i - 1].y) {
				y -= 1;

				if (!myEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("MovementS")) {
					myEnemy.GetComponent<Animator> ().SetTrigger ("S");
				}

				while (counter < 1) {
					myEnemy.transform.position += new Vector3 (0, -0.1f, 0);
					counter += 0.1f;
					yield return new WaitForSecondsRealtime (0.015f);
				}
			}
		}
		myEnemy.GetComponent<Animator> ().SetTrigger ("BackToIdle");
		myEnemy.transform.position = new Vector3 (Mathf.FloorToInt (x) + 0.5f, Mathf.FloorToInt (y) + 0.5f, myEnemy.transform.position.z);
		myMap.SetValue (Mathf.FloorToInt (myEnemy.transform.position.x), Mathf.FloorToInt (myEnemy.transform.position.y), Types.Enemy);
		working = false;
	}

	private void BackToTittle () {
		SceneManager.LoadScene ("Titulo");
	}
}