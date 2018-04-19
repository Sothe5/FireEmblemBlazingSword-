using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;

public class ItemInventory : MonoBehaviour{

	private JsonData itemData;
	public List<Item> itemList = new List<Item> ();

	void Start (){
		itemData = JsonMapper.ToObject (File.ReadAllText (Application.dataPath + "/Inventory/ItemDatabase.json"));
		readDataBase ();
	}

	private void readDataBase (){
		for (int i = 0; i < itemData.Count; i++){
			itemList.Add (new Item ((int)itemData [i] ["ProcedenceID"], (int)itemData [i] ["ID"], (string)itemData [i] ["Image"], (string)itemData [i] ["Type"],
				(string)itemData [i] ["Name"], (string)itemData [i] ["Description"], (int)itemData [i] ["Quantity"], (int)itemData [i] ["Attack"], (int)itemData [i] ["AttackDistance"],
				(int)itemData [i] ["Critical"], (int)itemData [i] ["Accuracy"], (int)itemData [i] ["Weight"]));
		}
	}

	public Item getItemByID (int itemID){
		
		for (int i = 0; i < itemList.Count; i++){

			if (itemList [i].ID == itemID){
				return itemList [i];
			}
		}
		Debug.Log("NO OBJECT FOUND");
		return null;
	}

}
