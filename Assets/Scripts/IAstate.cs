using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAstate
{

	public float valor;
	public int weaponDistance;
	public Spot finalPos;
	public Allie target;
	public Item weapon;

	public float ataqueRealizado;
	public float probRealizado;
	public float critRealizado;
	public float ataqueRecibido;
	public float probRecibido;
	public float critRecibido;

	public IAstate ()
	{
		this.valor = -5000;
	}

	public IAstate (Spot finalPos, Allie target, int weaponDistance)
	{
		this.finalPos = finalPos;
		this.target = target;
		this.weaponDistance = weaponDistance;
		this.valor = -5000;
	}

	public void calculateValue (Enemy myEnemy, Inventory myInventory, Map myMap)
	{	// da una puntuacion segun como de bueno es el ataque.
		float optimo = float.MinValue;
		foreach (Item i in myEnemy.items) { 
			if ((i.AttackDistance == this.weaponDistance || weaponDistance == 12) && i.Type == "Weapon") {
				// la IA por ahora no tiene en cuenta que al atacar primero puede que no reciba el daño de vuelta si lo mata && ni los criticos

				ataqueRealizado = (myEnemy.Strength + i.Attack + calculateTriangleDmg (myEnemy, myInventory)) - (target.Defence + calculateTerrainDefense (target.transform.position, myMap));
				if (ataqueRealizado < 0) {
					ataqueRealizado = 0;
				}
				probRealizado = (((myEnemy.Skill * 2) + (myEnemy.Luck / 2) + calculateTriangleDmg (myEnemy, myInventory) * 10 + i.Accuracy) - (target.Speed * 2 + target.Luck + calculateTerrainEvasion (target.transform.position, myMap))) / 100f;
				critRealizado = (((myEnemy.Skill / 2) + i.Critical) - target.Luck) / 100;

				ataqueRecibido = (target.Strength + myInventory.getAllieWeapons (target.ID) [0].Attack + calculateTriangleDmg (myEnemy, myInventory)) - (myEnemy.Defence + calculateTerrainDefense (myEnemy.transform.position, myMap));
				if (ataqueRecibido < 0) {
					ataqueRecibido = 0;
				}
				if (myInventory.getAllieWeapons (target.ID) [0].AttackDistance == weaponDistance || myInventory.getAllieWeapons (target.ID) [0].AttackDistance == 12) {
					probRecibido = (((target.Skill * 2) + (target.Luck / 2) + calculateTriangleDmg (myEnemy, myInventory) * 10 + myInventory.getAllieWeapons (target.ID) [0].Accuracy) - (myEnemy.Speed * 2 + myEnemy.Luck + calculateTerrainEvasion (myEnemy.transform.position, myMap))) / 100f;
				} else {
					probRecibido = 0;
				}
				critRecibido = (((target.Skill / 2) + myInventory.getAllieWeapons (target.ID) [0].Critical) - myEnemy.Luck) / 100;

				if (probRealizado < 0) {
					probRealizado = 0;
				} else if (probRealizado > 1) {
					probRealizado = 1;
				}
				if (probRecibido < 0) {
					probRecibido = 0;
				} else if (probRecibido > 1) {
					probRecibido = 1;
				}

				valor = (ataqueRealizado * probRealizado) - (ataqueRecibido * probRecibido);
				if (ataqueRealizado > target.Hp) {
					valor += 9000;
				}
				if (valor > optimo) {
					optimo = valor;
					weapon = i;
				}
			}
		}
		this.valor = optimo;
	}





	private int calculateTriangleDmg (Enemy myEnemy, Inventory myInventory)
	{
		int ans = 0;
		string allieWeapon = myInventory.getAllieItems (target.ID) [0].Description;
		string enemyWeapon = myEnemy.items [0].Description; // posible bug porque el item no sea un arma.

		if ((allieWeapon == Types.Sword && enemyWeapon == Types.Axe) || (allieWeapon == Types.Spear && enemyWeapon == Types.Sword) || (allieWeapon == Types.Axe && enemyWeapon == Types.Spear)) {
			ans = 1;
		} else if ((allieWeapon == Types.Sword && enemyWeapon == Types.Spear) || (allieWeapon == Types.Spear && enemyWeapon == Types.Axe) || (allieWeapon == Types.Axe && enemyWeapon == Types.Sword)) {
			ans = -1;
		}

		return ans;
	}

	private int calculateTerrainEvasion (Vector3 position, Map myMap)
	{
		int ans = 0;

		if (myMap.GetTerrainValue (Mathf.FloorToInt (position.x), Mathf.FloorToInt (position.y)) == Types.Forest) {
			ans = 20;
		} else if (myMap.GetTerrainValue (Mathf.FloorToInt (position.x), Mathf.FloorToInt (position.y)) == Types.Mountain) {
			ans = 30;
		}
		return ans;
	}

	private int calculateTerrainDefense (Vector3 position, Map myMap)
	{
		int ans = 0;

		if (myMap.GetTerrainValue (Mathf.FloorToInt (position.x), Mathf.FloorToInt (position.y)) == Types.Forest) {
			ans = 1;
		} else if (myMap.GetTerrainValue (Mathf.FloorToInt (position.x), Mathf.FloorToInt (position.y)) == Types.Mountain) {
			ans = 1;
		}
		return ans;
	}
}
