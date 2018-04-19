using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;

public class Inventory : MonoBehaviour
{

	private List<Allie> dataBase = new List<Allie> ();
	private JsonData itemData;
	public List<Item> myItems = new List<Item> ();


	void Start ()
	{
		itemData = JsonMapper.ToObject (File.ReadAllText (Application.dataPath + "/Inventory/AllieDatabase.json"));
		ReadDataBase ();
	}

	private void ReadDataBase ()
	{

		for (int i = 0; i < itemData.Count; i++) {
	
			dataBase.Add (new Allie ((int)itemData [i] ["Movement"], (int)itemData [i] ["Level"], 
				(int)itemData [i] ["Experience"], (int)itemData [i] ["Stats"] ["MaxHp"], (int)itemData [i] ["Stats"] ["Strength"],
				(int)itemData [i] ["Stats"] ["Skill"], (int)itemData [i] ["Stats"] ["Speed"], (int)itemData [i] ["Stats"] ["Luck"],
				(int)itemData [i] ["Stats"] ["Defence"], (int)itemData [i] ["Stats"] ["Resistance"], (string)itemData [i] ["Mount"],
				(int)itemData [i] ["Complexity"], (int)itemData [i] ["Aid"], (string)itemData [i] ["FaceImage"]));
	
			for (int j = 0; j < itemData [i] ["Items"].Count; j++) {

				if (itemData [i] ["Items"] [j].Count > 0) {
					myItems.Add (new Item ((int)itemData [i] ["Items"] [j] ["ProcedenceID"], (int)itemData [i] ["Items"] [j] ["ID"], (string)itemData [i] ["Items"] [j] ["Image"], (string)itemData [i] ["Items"] [j] ["Type"], (string)itemData [i] ["Items"] [j] ["Name"],
						(string)itemData [i] ["Items"] [j] ["Description"], (int)itemData [i] ["Items"] [j] ["Quantity"], (int)itemData [i] ["Items"] [j] ["Attack"], (int)itemData [i] ["Items"] [j] ["AttackDistance"], (int)itemData [i] ["Items"] [j] ["Critical"],
						(int)itemData [i] ["Items"] [j] ["Accuracy"], (int)itemData [i] ["Items"] [j] ["Weight"]));
				}
			}
		}
	}

	public void RemoveItem (int AllieID, int ID)
	{

		int numItemsBefore = 0;

		for (int i = 0; i < AllieID; i++) {
			numItemsBefore += getAllieItems (i).Count;
		}

		myItems.RemoveAt (ID + numItemsBefore);



		List<Item> allieItems = getAllieItems (AllieID);

		itemData [AllieID] ["Items"].Clear ();
		UpdateDatabase ();
		for (int j = 0; j < allieItems.Count; j++) {
			itemData [AllieID] ["Items"].Add (itemData [itemData.Count - 1] ["Items"] [0]);
			UpdateDatabase ();
			itemData [AllieID] ["Items"] [j] ["ProcedenceID"] = allieItems [j].ProcedenceID;
			itemData [AllieID] ["Items"] [j] ["ID"] = allieItems [j].ID;
			itemData [AllieID] ["Items"] [j] ["Image"] = allieItems [j].Image;
			itemData [AllieID] ["Items"] [j] ["Type"] = allieItems [j].Type;
			itemData [AllieID] ["Items"] [j] ["Name"] = allieItems [j].Name;
			itemData [AllieID] ["Items"] [j] ["Description"] = allieItems [j].Description;
			itemData [AllieID] ["Items"] [j] ["Quantity"] = allieItems [j].Quantity;
			itemData [AllieID] ["Items"] [j] ["Attack"] = allieItems [j].Attack;
			itemData [AllieID] ["Items"] [j] ["AttackDistance"] = allieItems [j].AttackDistance;
			itemData [AllieID] ["Items"] [j] ["Critical"] = allieItems [j].Critical;
			itemData [AllieID] ["Items"] [j] ["Accuracy"] = allieItems [j].Accuracy;
			itemData [AllieID] ["Items"] [j] ["Weight"] = allieItems [j].Weight;	
		}
		UpdateDatabase ();
	}

	public void SwapItems (int AllieID, int AllieID2, int ID1, int ID2)
	{
		
		if (ID2 > (getAllieItems (AllieID2).Count - 1)) { // el de la derecha vacio
			itemData [AllieID] ["Items"] [ID1] ["ProcedenceID"] = AllieID2;
			UpdateDatabase ();
			itemData [AllieID2] ["Items"].Add (itemData [AllieID] ["Items"] [ID1]); 
			UpdateDatabase ();
			RemoveItem (AllieID, ID1);
			UpdateDatabase ();

		} else if (ID1 > (getAllieItems (AllieID).Count - 1)) { 
			itemData [AllieID2] ["Items"] [ID2] ["ProcedenceID"] = AllieID;
			UpdateDatabase ();
			itemData [AllieID] ["Items"].Add (itemData [AllieID2] ["Items"] [ID2]); 
			UpdateDatabase ();
			RemoveItem (AllieID2, ID2);
			UpdateDatabase ();
		} else {
			Item aux = new Item ((int)itemData [AllieID] ["Items"] [ID1] ["ProcedenceID"], (int)itemData [AllieID] ["Items"] [ID1] ["ID"], (string)itemData [AllieID] ["Items"] [ID1] ["Image"], (string)itemData [AllieID] ["Items"] [ID1] ["Type"], (string)itemData [AllieID] ["Items"] [ID1] ["Name"],
				           (string)itemData [AllieID] ["Items"] [ID1] ["Description"], (int)itemData [AllieID] ["Items"] [ID1] ["Quantity"], (int)itemData [AllieID] ["Items"] [ID1] ["Attack"], (int)itemData [AllieID] ["Items"] [ID1] ["AttackDistance"], (int)itemData [AllieID] ["Items"] [ID1] ["Critical"],
				           (int)itemData [AllieID] ["Items"] [ID1] ["Accuracy"], (int)itemData [AllieID] ["Items"] [ID1] ["Weight"]);
	
			itemData [AllieID] ["Items"] [ID1] ["ID"] = (int)itemData [AllieID2] ["Items"] [ID2] ["ID"];
			itemData [AllieID] ["Items"] [ID1] ["Image"] = (string)itemData [AllieID2] ["Items"] [ID2] ["Image"];
			itemData [AllieID] ["Items"] [ID1] ["Type"] = (string)itemData [AllieID2] ["Items"] [ID2] ["Type"];
			itemData [AllieID] ["Items"] [ID1] ["Name"] = (string)itemData [AllieID2] ["Items"] [ID2] ["Name"];
			itemData [AllieID] ["Items"] [ID1] ["Description"] = (string)itemData [AllieID2] ["Items"] [ID2] ["Description"];
			itemData [AllieID] ["Items"] [ID1] ["Quantity"] = (int)itemData [AllieID2] ["Items"] [ID2] ["Quantity"];
			itemData [AllieID] ["Items"] [ID1] ["Attack"] = (int)itemData [AllieID2] ["Items"] [ID2] ["Attack"];
			itemData [AllieID] ["Items"] [ID1] ["AttackDistance"] = (int)itemData [AllieID2] ["Items"] [ID2] ["AttackDistance"];
			itemData [AllieID] ["Items"] [ID1] ["Critical"] = (int)itemData [AllieID2] ["Items"] [ID2] ["Critical"];
			itemData [AllieID] ["Items"] [ID1] ["Accuracy"] = (int)itemData [AllieID2] ["Items"] [ID2] ["Accuracy"];
			itemData [AllieID] ["Items"] [ID1] ["Weight"] = (int)itemData [AllieID2] ["Items"] [ID2] ["Weight"];

			itemData [AllieID2] ["Items"] [ID2] ["ID"] = aux.ID;
			itemData [AllieID2] ["Items"] [ID2] ["Image"] = aux.Image;
			itemData [AllieID2] ["Items"] [ID2] ["Type"] = aux.Type;
			itemData [AllieID2] ["Items"] [ID2] ["Name"] = aux.Name;
			itemData [AllieID2] ["Items"] [ID2] ["Description"] = aux.Description;
			itemData [AllieID2] ["Items"] [ID2] ["Quantity"] = aux.Quantity;
			itemData [AllieID2] ["Items"] [ID2] ["Attack"] = aux.Attack;
			itemData [AllieID2] ["Items"] [ID2] ["AttackDistance"] = aux.AttackDistance;
			itemData [AllieID2] ["Items"] [ID2] ["Critical"] = aux.Critical;
			itemData [AllieID2] ["Items"] [ID2] ["Accuracy"] = aux.Accuracy;
			itemData [AllieID2] ["Items"] [ID2] ["Weight"] = aux.Weight;
			UpdateDatabase ();
		}
	}

	public void ChangeValues (int ID, string Field, int Value)
	{
		itemData [ID] [Field] = Value; // change
		UpdateDatabase ();
	}

	public void ChangeValuesObject (int ID, int Value, int ID2, string Field2)
	{
		itemData [ID] ["Items"] [ID2] [Field2] = Value; // change
		UpdateDatabase ();
	}


	public Allie getAllie (int i)
	{ // el Allie.ID concuerda con la posicion en la Array del .json
		return dataBase [i];
	}

	public List<Item> getAllieItems (int id)
	{ // devuelve una lista de todos los items de un alliado
		List<Item> sol = new List<Item> ();

		for (int i = 0; i < myItems.Count; i++) {
			if (myItems [i].ProcedenceID == id) {
				sol.Add (myItems [i]);
			} else if (myItems [i].ProcedenceID > id) {
				break;
			}	
		}
		return sol;
	}

	public List<Item> getAllieWeapons (int id)
	{ // devuelve una lista de todos los items de un alliado
		List<Item> sol = new List<Item> ();

		for (int i = 0; i < myItems.Count; i++) {
			if (myItems [i].ProcedenceID == id && myItems [i].Type == "Weapon") {
				sol.Add (myItems [i]);
			} else if (myItems [i].ProcedenceID > id) {
				break;
			}	
		}
		return sol;
	}

	public void UpdateDatabase ()
	{
		itemData = JsonMapper.ToJson (itemData);
		File.WriteAllText (Application.dataPath + "/Inventory/AllieDatabase.json", itemData.ToString ());
		dataBase.Clear ();
		myItems.Clear ();
		itemData = JsonMapper.ToObject (File.ReadAllText (Application.dataPath + "/Inventory/AllieDatabase.json"));
		ReadDataBase ();
	}

}
