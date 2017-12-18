using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;

public class Inventory {

	private List<Ally> characterStats = new List<Ally> ();
	private JsonData itemData;
	public List<Item> myItems = new List<Item> ();


	void Start (){
		itemData = JsonMapper.ToObject (File.ReadAllText (Application.dataPath + "/Inventory/Items.json"));
		ReadDataBase ();
	}

	private void ReadDataBase (){

		for (int i = 0; i < itemData.Count; i++){
	
			dataBase.Add (new Ally ((int)itemData [i] ["Movement"], (int)itemData [i] ["AttackDistance"], (int)itemData [i] ["Level"], 
				(int)itemData [i] ["Experience"], (int)itemData [i] ["Stats"] ["Hp"], (int)itemData [i] ["Stats"] ["Strength"],
				(int)itemData [i] ["Stats"] ["Skill"], (int)itemData [i] ["Stats"] ["Speed"], (int)itemData [i] ["Stats"] ["Luck"],
				(int)itemData [i] ["Stats"] ["Defence"], (int)itemData [i] ["Stats"] ["Resistance"], (string)itemData [i] ["Mount"],
				(int)itemData [i] ["Complexity"], (int)itemData [i] ["Aid"]));
	
			for (int j = 0; j < itemData [i] ["Items"].Count; j++){

				if (itemData [i] ["Items"] [j].Count > 0){
					myItems.Add (new Item ((int)itemData [i] ["Items"] [j] ["ProcedenceID"], (string)itemData [i] ["Items"] [j] ["Image"], (string)itemData [i] ["Items"] [j] ["Type"], (string)itemData [i] ["Items"] [j] ["Name"],
						(string)itemData [i] ["Items"] [j] ["Description"], (int)itemData [i] ["Items"] [j] ["Quantity"], (int)itemData [i] ["Items"] [j] ["Attack"], (int)itemData [i] ["Items"] [j] ["Critical"],
						(int)itemData [i] ["Items"] [j] ["Accuracy"], (int)itemData [i] ["Items"] [j] ["Dodge"]));
				}
			}
		}
	}

	public void RemoveItem (int AllyID, int ID){

		int numItemsBefore = 0;

		for (int i = 0; i < AllyID; i++){
			numItemsBefore += getAllyItems (i).Count;
		}

		myItems.RemoveAt (ID + numItemsBefore);



		List<Item> AllyItems = getAllyItems (AllyID);
		for (int j = 0; j < AllyItems.Count; j++){
			itemData [AllyID] ["Items"] [j] ["ProcedenceID"] = AllyItems [j].ProcedenceID;
			itemData [AllyID] ["Items"] [j] ["Image"] = AllyItems [j].Image;
			itemData [AllyID] ["Items"] [j] ["Type"] = AllyItems [j].Type;
			itemData [AllyID] ["Items"] [j] ["Name"] = AllyItems [j].Name;
			itemData [AllyID] ["Items"] [j] ["Description"] = AllyItems [j].Description;
			itemData [AllyID] ["Items"] [j] ["Quantity"] = AllyItems [j].Quantity;
			itemData [AllyID] ["Items"] [j] ["Attack"] = AllyItems [j].Attack;
			itemData [AllyID] ["Items"] [j] ["Critical"] = AllyItems [j].Critical;
			itemData [AllyID] ["Items"] [j] ["Accuracy"] = AllyItems [j].Accuracy;
			itemData [AllyID] ["Items"] [j] ["Dodge"] = AllyItems [j].Dodge;
		}

		
		itemData [AllyID] ["Items"] [AllyItems.Count].Clear ();


		itemData = JsonMapper.ToJson (itemData); 
		File.WriteAllText (Application.dataPath + "/Inventory/Items.json", itemData.ToString ());
		dataBase.Clear ();
		myItems.Clear ();
		itemData = JsonMapper.ToObject (File.ReadAllText (Application.dataPath + "/Inventory/Items.json"));
		ReadDataBase ();
	}

	public void SwapItems (int AllyID, int ID1, int ID2){

		string pathImage = (string)itemData [AllyID] ["Items"] [ID1] ["Image"];
		Item aux = new Item ((int)itemData [AllyID] ["Items"] [ID1] ["ProcedenceID"], (string)itemData [AllyID] ["Items"] [ID1] ["Image"], (string)itemData [AllyID] ["Items"] [ID1] ["Type"], (string)itemData [AllyID] ["Items"] [ID1] ["Name"],
			           (string)itemData [AllyID] ["Items"] [ID1] ["Description"], (int)itemData [AllyID] ["Items"] [ID1] ["Quantity"], (int)itemData [AllyID] ["Items"] [ID1] ["Attack"], (int)itemData [AllyID] ["Items"] [ID1] ["Critical"],
			           (int)itemData [AllyID] ["Items"] [ID1] ["Accuracy"], (int)itemData [AllyID] ["Items"] [ID1] ["Dodge"]);
		

		itemData [AllyID] ["Items"] [ID1] ["ProcedenceID"] = (int)itemData [AllyID] ["Items"] [ID2] ["ProcedenceID"];
		itemData [AllyID] ["Items"] [ID1] ["Image"] = (string)itemData [AllyID] ["Items"] [ID2] ["Image"];
		itemData [AllyID] ["Items"] [ID1] ["Type"] = (string)itemData [AllyID] ["Items"] [ID2] ["Type"];
		itemData [AllyID] ["Items"] [ID1] ["Name"] = (string)itemData [AllyID] ["Items"] [ID2] ["Name"];
		itemData [AllyID] ["Items"] [ID1] ["Description"] = (string)itemData [AllyID] ["Items"] [ID2] ["Description"];
		itemData [AllyID] ["Items"] [ID1] ["Quantity"] = (int)itemData [AllyID] ["Items"] [ID2] ["Quantity"];
		itemData [AllyID] ["Items"] [ID1] ["Attack"] = (int)itemData [AllyID] ["Items"] [ID2] ["Attack"];
		itemData [AllyID] ["Items"] [ID1] ["Critical"] = (int)itemData [AllyID] ["Items"] [ID2] ["Critical"];
		itemData [AllyID] ["Items"] [ID1] ["Accuracy"] = (int)itemData [AllyID] ["Items"] [ID2] ["Accuracy"];
		itemData [AllyID] ["Items"] [ID1] ["Dodge"] = (int)itemData [AllyID] ["Items"] [ID2] ["Dodge"];


		itemData [AllyID] ["Items"] [ID2] ["ProcedenceID"] = aux.ProcedenceID;
		itemData [AllyID] ["Items"] [ID2] ["Image"] = pathImage;
		itemData [AllyID] ["Items"] [ID2] ["Type"] = aux.Type;
		itemData [AllyID] ["Items"] [ID2] ["Name"] = aux.Name;
		itemData [AllyID] ["Items"] [ID2] ["Description"] = aux.Description;
		itemData [AllyID] ["Items"] [ID2] ["Quantity"] = aux.Quantity;
		itemData [AllyID] ["Items"] [ID2] ["Attack"] = aux.Attack;
		itemData [AllyID] ["Items"] [ID2] ["Critical"] = aux.Critical;
		itemData [AllyID] ["Items"] [ID2] ["Accuracy"] = aux.Accuracy;
		itemData [AllyID] ["Items"] [ID2] ["Dodge"] = aux.Dodge;

		itemData = JsonMapper.ToJson (itemData); 
		File.WriteAllText (Application.dataPath + "/Inventory/Items.json", itemData.ToString ());
		dataBase.Clear ();
		myItems.Clear ();
		itemData = JsonMapper.ToObject (File.ReadAllText (Application.dataPath + "/Inventory/Items.json"));
		ReadDataBase ();
	}

	public void ChangeValues (int ID, string Field, int Value){

		itemData [ID] [Field] = Value; // change

		itemData = JsonMapper.ToJson (itemData);
		File.WriteAllText (Application.dataPath + "/Inventory/Items.json", itemData.ToString ());
		dataBase.Clear ();
		myItems.Clear ();
		itemData = JsonMapper.ToObject (File.ReadAllText (Application.dataPath + "/Inventory/Items.json"));
		ReadDataBase ();

	}

	public void ChangeValuesObject (int ID, int Value, int ID2, string Field2){

		itemData [ID] ["Items"] [ID2] [Field2] = Value; // change

		itemData = JsonMapper.ToJson (itemData); 

		File.WriteAllText (Application.dataPath + "/Inventory/Items.json", itemData.ToString ());
		dataBase.Clear ();
		myItems.Clear ();
		itemData = JsonMapper.ToObject (File.ReadAllText (Application.dataPath + "/Inventory/Items.json"));
		ReadDataBase ();

	}


	public Ally getAlly (int i){ // el Ally.ID concuerda con la posicion en la Array del .json
		return dataBase [i];
	}

	public List<Item> getAllyItems (int id){ // devuelve una lista de todos los items de un alliado
		List<Item> sol = new List<Item> ();

		for (int i = 0; i < myItems.Count; i++){
			if (myItems [i].ProcedenceID == id){
				sol.Add (myItems [i]);
			} else if (myItems [i].ProcedenceID > id){
				break;
			}	
		}
		return sol;
	}

}
