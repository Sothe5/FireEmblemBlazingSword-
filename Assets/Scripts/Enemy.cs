using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	private bool isAble;
	private SpriteRenderer mySpriteRenderer;
	private Color myColor;
	private ItemInventory myItemInventory;
	private Map myMap;


	public int ID;
	public string Name;
	public int movement;
	public int Level;
	public int Experience;
	public int MaxHp;
	public int Hp;
	public int Strength;
	public int Skill;
	public int Speed;
	public int Luck;
	public int Defence;
	public int Resistance;
	public string Mount;
	public int Complexity;
	public int Aid;
	public Sprite FaceImage;
	public List<Item> items = new List<Item> ();



		
	//		public string Dexterity;


	//	Tiene que haber una lista de IDs para los distintos enemigos, otra para los objetos y otra para los aliados.

	void Start ()
	{
		myMap = FindObjectOfType<Map> ();
		isAble = true;
		mySpriteRenderer = GetComponent<SpriteRenderer> ();
		myColor = mySpriteRenderer.color;
		myItemInventory = FindObjectOfType<ItemInventory> ();
		getItems ();
	}

	public void getItems ()
	{ 
		if (ID == 0) {
			items.Add (myItemInventory.getItemByID (0));
			items.Add (myItemInventory.getItemByID (1));
		} else if (ID == 1) {
			items.Add (myItemInventory.getItemByID (6));
		}
	}

	public void reciveDamage (int amount)
	{
		Hp -= amount;
		if (Hp < 1) {
			myMap.SetValue (Mathf.FloorToInt (transform.position.x), Mathf.FloorToInt (transform.position.y), null);
			myMap.matrixScenary [Mathf.FloorToInt (transform.position.x)] [Mathf.FloorToInt (transform.position.y)].setType (Types.Free);
			Destroy (gameObject);
		}
	}

	public bool AmIAlive ()
	{
		return Hp > 0;
	}

	public bool getDisponibility ()
	{
		return isAble;
	}

	public void setDisponibility (bool a)
	{
		isAble = a;
		if (!a) {
			myColor.r = 0.25f;
			myColor.g = 0.25f;
			myColor.b = 0.25f;
			mySpriteRenderer.color = myColor;
		} else {
			myColor.r = 1;
			myColor.g = 1;
			myColor.b = 1;
			mySpriteRenderer.color = myColor;
		}

	}

}
