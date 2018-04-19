using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Allie : MonoBehaviour {

	private bool isAble;
	private Inventory myInventory;
	private Map myMap;
	private SpriteRenderer mySpriteRenderer;
	private Color myColor;

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
//		public string Dexterity;


	public Allie (int movement,int Level,int Experience,int MaxHp,int Strength,int Skill,int Speed, 
		int Luck, int Defence,int Resistance,string Mount,int Complexity, int Aid,string FaceImage){

		this.movement = movement;
		this.Level = Level;
		this.Experience = Experience;
		this.MaxHp = MaxHp;
		this.Strength = Strength;
		this.Skill = Skill;
		this.Speed = Speed;
		this.Luck = Luck;
		this.Defence = Defence;
		this.Resistance = Resistance;
		this.Mount = Mount;
		this.Complexity = Complexity;
		this.Aid = Aid;
		this.FaceImage = Resources.Load<Sprite>(FaceImage);
	}

	void Start(){
		myInventory = FindObjectOfType<Inventory>();
		myMap = FindObjectOfType<Map> ();
		isAble = true;
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		myColor = mySpriteRenderer.color;
	
		// primero cambias y despues lees el valor que quieras una vez ya modificado una vez se cambie de nivel se 

//		myInventory.ChangeValuesObject(0,"Items",3,0,"Dodge");

		movement = myInventory.getAllie(ID).movement;
		Level = myInventory.getAllie(ID).Level;
		Experience = myInventory.getAllie(ID).Experience;
		MaxHp = myInventory.getAllie(ID).MaxHp;
		Strength = myInventory.getAllie(ID).Strength;
		Skill = myInventory.getAllie(ID).Skill;
		Speed = myInventory.getAllie(ID).Speed;
		Luck = myInventory.getAllie(ID).Luck;
		Defence = myInventory.getAllie(ID).Defence;
		Resistance = myInventory.getAllie(ID).Resistance;
		Mount = myInventory.getAllie(ID).Mount;
		Complexity = myInventory.getAllie(ID).Complexity;
		Aid = myInventory.getAllie(ID).Aid;
		FaceImage = myInventory.getAllie(ID).FaceImage;
		Hp = MaxHp;
	}

	public void reciveDamage(int amount){
		Hp -= amount;
		if(Hp < 1){
			myMap.SetValue (Mathf.FloorToInt (transform.position.x), Mathf.FloorToInt (transform.position.y), null);
			myMap.matrixScenary [Mathf.FloorToInt (transform.position.x)] [Mathf.FloorToInt (transform.position.y)].setType (Types.Free);
			Destroy(gameObject);
		}
	}

	public bool AmIAlive(){
		return Hp > 0;
	}

	public bool getDisponibility(){
		return isAble;
	}

	public void setDisponibility(bool a){
		isAble = a;
		if(!a){
			myColor.r = 0.25f;
			myColor.g = 0.25f;
			myColor.b = 0.25f;
			mySpriteRenderer.color = myColor;
		}else{
			myColor.r = 1;
			myColor.g = 1;
			myColor.b = 1;
			mySpriteRenderer.color = myColor;
		}

	}

}

public class Item {
	
	public int ProcedenceID;
	public int ID;
	public string Image;
	public string Type;
	public string Name;
	public string Description;
	public int Quantity;
	public int Attack;
	public int AttackDistance;
	public int Critical;
	public int Accuracy;
	public int Weight;

	public Item (int ProcedenceID,int ID,string Image,string Type,string Name,string Description,int Quantity,int Attack,int AttackDistance,int Critical,int Accuracy,int Weight){
		this.ProcedenceID = ProcedenceID;
		this.ID = ID;
		this.Image = Image;
		this.Type = Type;
		this.Name = Name;
		this.Description = Description;
		this.Quantity = Quantity;
		this.Attack = Attack;
		this.AttackDistance = AttackDistance;
		this.Critical = Critical;
		this.Accuracy = Accuracy;
		this.Weight = Weight;
	}
}
