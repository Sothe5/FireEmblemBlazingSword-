using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour{

	private Map myMap;
	private int cordX;
	private int cordY;
	private Ally myAlly;
	private Inventory myInventory;
	private Vector3 lastPos;
	private Quaternion lastQuat;
	private int counter;
	private bool isRectifying;
	// mira si estas rectificando o no.
	private bool isCountPlus1;
	// mira si el contador es == al movimiento
	private bool isOutOfArea;
	// mira si estas fuera del area.
	private Vector3 myAllyLastPos;
	// mira la ultima posicion del aliado
	private int state;
	private bool isCellOcupaid;
	private bool wasRecentlyOut;
	private List<Item> Objects;
	private List<Item> Objects2;
	private List <Spot> usefull = new List <Spot> ();
	 // IDEA: para el movimiento del personaje al return == 1. 		private List <Spot> currentPath = new List <Spot> ();


	public GameObject panelOptions;
	public Slider SliderOptions;
	public GameObject panelOptionsPanel;

	public GameObject panelObjects;
	public Slider SliderObjects;
	public GameObject panelObjectPanel;
	public Image Image1;
	public Text Name1;
	public Text Quantity1;
	public Image Image2;
	public Text Name2;
	public Text Quantity2;
	public Image Image3;
	public Text Name3;
	public Text Quantity3;
	public Image Image4;
	public Text Name4;
	public Text Quantity4;
	public Image Image5;
	public Text Name5;
	public Text Quantity5;

	public GameObject panelObjects2;
	public Slider SliderObjects2;
	public GameObject panelObjectPanel2;
	public Image Image12;
	public Text Name12;
	public Text Quantity12;
	public Image Image22;
	public Text Name22;
	public Text Quantity22;
	public Image Image32;
	public Text Name32;
	public Text Quantity32;
	public Image Image42;
	public Text Name42;
	public Text Quantity42;
	public Image Image52;
	public Text Name52;
	public Text Quantity52;


	public GameObject panelValuesObject;
	public GameObject panelValuesObjectWeapons;
	public Text QuantityAttack;
	public Text QuantityCritical;
	public Text QuantityDodge;
	public Text QuantityHit;
	public GameObject panelValuesObjectUsable;
	public Text Description;

	public GameObject panelEqUs;
	public GameObject Equipar;
	public GameObject Usar;
	public Text Tirar;
	public Slider SliderEqUs;


	public GameObject BlueTransparency;
	public GameObject RedTransparency;
	public GameObject Line;
	public GameObject EndLine;
	public GameObject EndLineArr;
	public GameObject DerArr;
	public GameObject DerAba;
	public GameObject IzAba;
	public GameObject IzArr;
	public GameObject tester;
	public GameObject auxCursor;

	// state 0 = free movement
	// state 1 = a character has been selected
	// state 2 = return has been press while a character is selected and options menu spawn.
	// state 3 = Object menu is open
	// state 4 = panelEqUs is open
	// state 5 = choosing adjacent Ally/Enemy;

	// codigo para terminar el turnoß
	//	myMap.EndTurn();
	//			BorraTransLinea();
	//			panelAttack.SetActive(false);
	//			state = 0;
	//			Time.timeScale = 1;
	//			counter = 0;




	void Start (){
		myInventory = FindObjectOfType<Inventory> ();
		state = 0;
		myMap = FindObjectOfType<Map> ();
		InvokeRepeating ("JustASec", 0, 0.15f);
		counter = 0;
		isCellOcupaid = false;


	}

	void Update (){

		if (Input.GetKeyDown (KeyCode.N)){
			myMap.EndTurn ();
			BorraTransLinea ();
			panelOptions.SetActive (false);
			state = 0;
			Time.timeScale = 1;
			counter = 0;
		}

		// Realiza los movimientos basicos
		if (CheckTwoAtaTime ()){
		} else{
			if (Input.GetKeyDown (KeyCode.A) && Time.timeScale > 0){
				transform.position += new Vector3 (-1, 0, 0);
				EnsureMap ();
			}

			if (Input.GetKeyDown (KeyCode.W) && Time.timeScale > 0){
				transform.position += new Vector3 (0, 1, 0);
				EnsureMap ();
			}

			if (Input.GetKeyDown (KeyCode.D) && Time.timeScale > 0){
				transform.position += new Vector3 (1, 0, 0);
				EnsureMap ();
			}

			if (Input.GetKeyDown (KeyCode.S) && Time.timeScale > 0){
				transform.position += new Vector3 (0, -1, 0);
				EnsureMap ();
			}
		}

		// Genera las coordenadas en Integers.
		cordX = Mathf.FloorToInt (transform.position.x); // la da por debajo siempre
		cordY = Mathf.FloorToInt (transform.position.y);

		// Generacion de Lineas de Movimiento
		if (state == 1 && myMap.GetValueScenary (cordX, cordY) != Types.Ocupaid && (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.S))){
			// indica si el cursor esta fuera del area.
			Spot[] path = new Spot[(myMap.Right + 1) * (myMap.Up + 1)];
			Spot posInPath = gameObject.AddComponent<Spot> ();
			posInPath.x = 0;
		
			if (!myMap.isPathFinding (Mathf.FloorToInt (myAlly.transform.position.x), Mathf.FloorToInt (myAlly.transform.position.y), cordX, cordY, myAlly, path, posInPath)){
				isOutOfArea = true;
				wasRecentlyOut = true;
			} else{
				isOutOfArea = false;
			}
			Destroy (posInPath);

			if (CheckTwoAtaTime () || isCellOcupaid || isOutOfArea){
				AutogeneraciondeCaminos (cordX, cordY);
			} else{
				GeneracionDeLineas ();
			}
			isCellOcupaid = false;
		} else if (myMap.GetValueScenary (cordX, cordY) == Types.Ocupaid){
			isCellOcupaid = true;
			isOutOfArea = true;
			wasRecentlyOut = true;
		}

		if (state == 5 && (Input.GetKeyDown (KeyCode.A))){
			for (int i = 0; i < usefull.Count; i++){
				if (myAlly.transform.position.x - 1 == usefull [i].x + 0.5f){

					transform.position = new Vector3 (usefull [i].x + 0.5f, usefull [i].y + 0.5f);
					break;
				}
			}
		} else if (state == 5 && (Input.GetKeyDown (KeyCode.W))){
			for (int i = 0; i < usefull.Count; i++){
				if (myAlly.transform.position.y + 1 == usefull [i].y + 0.5f){
					transform.position = new Vector3 (usefull [i].x + 0.5f, usefull [i].y + 0.5f);
					break;
				}
			}
		} else if (state == 5 && (Input.GetKeyDown (KeyCode.D))){
			for (int i = 0; i < usefull.Count; i++){
				if (myAlly.transform.position.x + 1 == usefull [i].x + 0.5f){

					transform.position = new Vector3 (usefull [i].x + 0.5f, usefull [i].y + 0.5f);
					break;
				}
			}
		} else if (state == 5 && (Input.GetKeyDown (KeyCode.S))){
			for (int i = 0; i < usefull.Count; i++){
				if (myAlly.transform.position.y - 1 == usefull [i].y + 0.5f){
					transform.position = new Vector3 (usefull [i].x + 0.5f, usefull [i].y + 0.5f);
					break;
				}
			}
		}

		// Mira si en la casilla hay algo cuando se pulsa en el estado 0.
		if (Input.GetKeyDown (KeyCode.Return) && state == 0){
			if (myMap.GetValue (cordX, cordY) == null){	// se abra otro menu

			} else if (myMap.GetValue (cordX, cordY) == Types.Enemy){
			
			} else if (myMap.GetValue (cordX, cordY) == Types.Ally){ 

				Vector3 position = new Vector3 (cordX, cordY, 0);
				Object[] objs = Ally.FindSceneObjectsOfType (typeof(Ally));
				myAlly = null;
   			
				foreach (Ally go in objs){
					if (Mathf.FloorToInt (go.transform.position.x) == position.x && Mathf.FloorToInt (go.transform.position.y) == position.y){
						myAlly = go;
						break;
					}
				}

				
				if (myAlly.getDisponibility ()){
					CampoDeVision (myAlly);
					lastPos = transform.position;
					lastQuat = Quaternion.Euler (2, 2, 2);
				} 
			
			}

			// Se va a la posicion indicada y abre el menu cuando se pulsa en el estado 1.
		} else if (Input.GetKeyDown (KeyCode.Return) && !isOutOfArea && state == 1){
			bool posHasAlly = false;
			Object[] objs = Ally.FindSceneObjectsOfType (typeof(Ally));
			foreach (Ally go in objs){
				if (transform.position == go.transform.position && go.GetComponent<Ally> () && go.ID != myAlly.ID){
					posHasAlly = true;
					break;
				}
			}
			if (!posHasAlly){ 
				myAllyLastPos = myAlly.transform.position;


				myAlly.transform.position = transform.position;


				BorraTransLinea ();
				panelOptions.SetActive (true);
				SliderOptions.value = 0;
				state = 2;
				Time.timeScale = 0;
				if (transform.position.x < ((myMap.Right + 1) / 2)){
					panelOptions.transform.position = new Vector3 (myMap.Right - (0.25f * myMap.Right) + 0.5f, myMap.Up - (0.3f * myMap.Up) + 0.5f, 0);

				} else{
					panelOptions.transform.position = new Vector3 (0.01f * myMap.Right + 0.5f, myMap.Up - (0.3f * myMap.Up) + 0.5f, 0);
				}

			}
			// Ejecuta una accion cuando se pulsa en el estado 2.
		} else if (Input.GetKeyDown (KeyCode.Return) && state == 2){
			usefull.Clear();
			Spot[] neighbors = myMap.matrixScenary [cordX] [cordY].neighbors;

			if (SliderOptions.value == 0){ // ATTACK
				Debug.Log ("ATTACK");	
			}
			if (SliderOptions.value == 1){ // OBJECTS
				if (myInventory.getAllyItems (myAlly.ID).Count > 0){
					state = 3;
					Objects = myInventory.getAllyItems (myAlly.ID);
					panelOptions.SetActive (false);
					panelObjects.SetActive (true);
					panelValuesObject.SetActive (true);
					SliderObjects.value = 0;
					ShowObjectValues ();
					ShowObjects ();
				} else{
					Debug.Log ("No tienes objetos");
				}

			}
			if (SliderOptions.value == 2){ // INTERC
				// elegir entre 4

				foreach (Spot a in neighbors){
					if (myMap.matrixScenary [a.x] [a.y].personType == Types.Ally){
						usefull.Add (a);
					}
				}
				neighbors = null;
				// ya tenemos la lista usefull con los posibles spot. pasar a state 5
				if (usefull.Count > 0){
					state = 5;
					panelOptions.SetActive(false);
					transform.position = new Vector3 (usefull [0].x + 0.5f, usefull [0].y + 0.5f);
					// tendria que hacer las 4 direcciones de movimiento y el shift derecho.
				}
			}
			if (SliderOptions.value == 3){ // ESPERAR
				state = 0;
				BorraTransLinea ();
				panelOptions.SetActive (false);
				Time.timeScale = 1;
				myAlly.setDisponibility (false);
				myMap.SetValue (Mathf.FloorToInt (myAllyLastPos.x), Mathf.FloorToInt (myAllyLastPos.y), null);
				myMap.SetValue (cordX, cordY, Types.Ally);
			}
		} else if (Input.GetKeyDown (KeyCode.Return) && state == 3){
			panelEqUs.SetActive (true);
			state = 4;
			SliderEqUs.value = 0;
			int index = Mathf.FloorToInt (SliderObjects.value);
			if (Objects [index].Type == "Weapon"){
				Equipar.SetActive (true);
				Usar.SetActive (false);
			} else{
				Equipar.SetActive (false);
				Usar.SetActive (true);
			}
		} else if (Input.GetKeyDown (KeyCode.Return) && state == 4){

			if (SliderEqUs.value == 0 && Equipar.activeSelf){ // equipar
				myInventory.SwapItems (myAlly.ID, Mathf.FloorToInt (SliderObjects.value), 0);
				Objects = myInventory.getAllyItems (myAlly.ID);

				Image1.sprite = Resources.Load<Sprite> (Objects [0].Image);
				Name1.text = Objects [0].Name.ToString ();
				Quantity1.text = Objects [0].Quantity.ToString ();

				if (SliderObjects.value == 1){
					Image2.sprite = Resources.Load<Sprite> (Objects [1].Image);
					Name2.text = Objects [1].Name.ToString ();
					Quantity2.text = Objects [1].Quantity.ToString ();
				} else if (SliderObjects.value == 2){
					Image3.sprite = Resources.Load<Sprite> (Objects [2].Image);
					Name3.text = Objects [2].Name.ToString ();
					Quantity3.text = Objects [2].Quantity.ToString ();
				} else if (SliderObjects.value == 3){
					Image4.sprite = Resources.Load<Sprite> (Objects [3].Image);
					Name4.text = Objects [3].Name.ToString ();
					Quantity4.text = Objects [3].Quantity.ToString ();
				} else if (SliderObjects.value == 4){
					Image5.sprite = Resources.Load<Sprite> (Objects [4].Image);
					Name5.text = Objects [4].Name.ToString ();
					Quantity5.text = Objects [4].Quantity.ToString ();
				}
				panelEqUs.SetActive (false);
				state = 3;
				SliderObjects.value = 0;
				ShowObjectValues ();

			} else if (SliderEqUs.value == 0 && Usar.activeSelf){	//objeto dependiente del mismo.

			} else if (SliderEqUs.value == 1){ // tirar;
				if (myInventory.getAllyItems (myAlly.ID).Count > 1){
					myInventory.RemoveItem (myAlly.ID, Mathf.FloorToInt (SliderObjects.value));

					Objects = myInventory.getAllyItems (myAlly.ID);

					ShowObjects ();

					panelEqUs.SetActive (false);
					state = 3;
					SliderObjects.value = 0;
					ShowObjectValues ();
				} else{
					myInventory.RemoveItem (myAlly.ID, Mathf.FloorToInt (SliderObjects.value));

					panelEqUs.SetActive (false);
					state = 2;
					SliderObjects.value = 0;
					panelValuesObject.SetActive (false);
					panelObjects.SetActive (false);
					panelOptions.SetActive (true);
					SliderOptions.value = 0;
				}
			}
		} else if (Input.GetKeyDown (KeyCode.Return) && state == 5){
			state = 6;
			// en transform.position estara el aliado
			Objects = myInventory.getAllyItems (myAlly.ID);
			panelOptions.SetActive (false);
			panelObjects.SetActive (true);
			SliderObjects.value = 0;
			SliderObjects.gameObject.SetActive(true);
			SliderObjects2.gameObject.SetActive(false);
			ShowObjects ();

			Object[] objs = Ally.FindSceneObjectsOfType (typeof(Ally));
			foreach (Ally go in objs){
				if(go.transform.position == transform.position){
					Objects2 = myInventory.getAllyItems (go.ID);
					break;
				}
			}
			panelObjects2.SetActive (true);
			ShowObjects2();
			
		}else if (Input.GetKeyDown (KeyCode.Return) && state == 5){

			// TO DO

		}


		// Elimina Lineas y Transparencias cuando se pulsa en el estado 1.
		if (Input.GetKeyDown (KeyCode.RightShift) && state == 1){
			BorraTransLinea ();
			counter = 0;
			state = 0;
			transform.position = myAlly.transform.position;
		} else if (Input.GetKeyDown (KeyCode.RightShift) && state == 2){ // Vuelve a la situacion previa cuando se pulsa en el estado 2.
			myAlly.transform.position = myAllyLastPos;
			panelOptions.SetActive (false);
			Time.timeScale = 1;
			CampoDeVision (myAlly);
			AutogeneraciondeCaminos (cordX, cordY);
		} else if (Input.GetKeyDown (KeyCode.RightShift) && state == 3){
			panelObjects.SetActive (false);
			panelValuesObject.SetActive (false);
			panelValuesObjectUsable.SetActive (false);
			panelValuesObjectWeapons.SetActive (false);
			panelOptions.SetActive (true);
			state = 2;
		} else if (Input.GetKeyDown (KeyCode.RightShift) && state == 4){
			panelEqUs.SetActive (false);
			state = 3;
		} else if (Input.GetKeyDown (KeyCode.RightShift) && state == 5){
			state = 2;
			panelOptions.SetActive(true);
			SliderOptions.value = 0;
			transform.position = myAlly.transform.position;
		} else if (Input.GetKeyDown (KeyCode.RightShift) && state == 6){ // EHY

			

		}
	
		// Mueve los valores del menu cuando se esta en el estado 2.
		if (Input.GetKeyDown (KeyCode.W) && state == 2){
			SliderOptions.value -= 1;
		}

		if (Input.GetKeyDown (KeyCode.S) && state == 2){
			SliderOptions.value += 1;
		}

		if (Input.GetKeyDown (KeyCode.W) && state == 3){ // hacerle que el slider no se mueva mas de lo que debe
			SliderObjects.value -= 1;
			ShowObjectValues ();
		}

		if (Input.GetKeyDown (KeyCode.S) && state == 3 && SliderObjects.value < Objects.Count - 1){
			SliderObjects.value += 1;
			ShowObjectValues ();
		}

		if (Input.GetKeyDown (KeyCode.W) && state == 4){
			SliderEqUs.value -= 1;
		}

		if (Input.GetKeyDown (KeyCode.S) && state == 4){
			SliderEqUs.value += 1;
		}

		if (Input.GetKeyDown (KeyCode.W) && state == 6){
			if(SliderObjects.gameObject.activeSelf){
				SliderObjects.value -= 1;
			}else{
				SliderObjects2.value -= 1;
			}
		}

		if (Input.GetKeyDown (KeyCode.S) && state == 6){
			if(SliderObjects.gameObject.activeSelf && SliderObjects.value < Objects.Count - 1){
				SliderObjects.value += 1;
			}else if (SliderObjects2.gameObject.activeSelf && SliderObjects2.value < Objects2.Count -1){
				SliderObjects2.value += 1;
			}
		}
		if (Input.GetKeyDown (KeyCode.A) && state == 6){
			
			if(SliderObjects2.gameObject.activeSelf){
				if(SliderObjects2.value < Objects.Count){
					SliderObjects.value = SliderObjects2.value;
				}else{
					SliderObjects.value = Objects.Count-1;
				}
			
				SliderObjects.gameObject.SetActive(true);
				SliderObjects2.gameObject.SetActive(false);
			}

		}

		if (Input.GetKeyDown (KeyCode.D) && state == 6){
			
			if(SliderObjects.gameObject.activeSelf){
				if(SliderObjects.value < Objects2.Count){
					SliderObjects2.value = SliderObjects.value;
				}else{
					SliderObjects2.value = Objects2.Count-1;
				}
		
				SliderObjects.gameObject.SetActive(false);
				SliderObjects2.gameObject.SetActive(true);
			}

		}
	}

	//*******************
	// Comienzo subprogramas independientes
	//*******************


	private void ShowObjects (){
		panelObjectPanel.GetComponent<RectTransform> ().offsetMin = new Vector2 (panelObjectPanel.GetComponent<RectTransform> ().offsetMin.x, -11.5f);
		SliderOptions.value = 0;
		Color alpha = new Color ();
		alpha.r = 1;
		alpha.g = 1;
		alpha.b = 1;
		// se coloquen en cada sitio sus necesarios


		if (Objects.Count >= 1){
			alpha.a = 1;
			Image1.color = alpha;
			Image1.sprite = Resources.Load<Sprite> (Objects [0].Image);
			Name1.text = Objects [0].Name.ToString ();
			Quantity1.text = Objects [0].Quantity.ToString ();
			if(state != 6)
			panelObjectPanel.GetComponent<RectTransform> ().offsetMin = new Vector2 (panelObjectPanel.GetComponent<RectTransform> ().offsetMin.x, 200);
		} else{
					
			alpha.a = 0;
			Image1.color = alpha;
			Name1.text = "";
			Quantity1.text = "";
		}
		if (Objects.Count >= 2){
			alpha.a = 1;
			Image2.color = alpha;
			Image2.sprite = Resources.Load<Sprite> (Objects [1].Image);
			Name2.text = Objects [1].Name.ToString ();
			Quantity2.text = Objects [1].Quantity.ToString ();
			if(state != 6)
			panelObjectPanel.GetComponent<RectTransform> ().offsetMin = new Vector2 (panelObjectPanel.GetComponent<RectTransform> ().offsetMin.x, 155);
		} else{
			alpha.a = 0;
			Image2.color = alpha;
			Name2.text = "";
			Quantity2.text = "";
		}
		if (Objects.Count >= 3){
			alpha.a = 1;
			Image3.color = alpha;
			Image3.sprite = Resources.Load<Sprite> (Objects [2].Image);
			Name3.text = Objects [2].Name.ToString ();
			Quantity3.text = Objects [2].Quantity.ToString ();
			if(state != 6)
			panelObjectPanel.GetComponent<RectTransform> ().offsetMin = new Vector2 (panelObjectPanel.GetComponent<RectTransform> ().offsetMin.x, 110);
		} else{
			alpha.a = 0;
			Image3.color = alpha;
			Name3.text = "";
			Quantity3.text = "";
		}
		if (Objects.Count >= 4){
			alpha.a = 1;
			Image4.color = alpha;
			Image4.sprite = Resources.Load<Sprite> (Objects [3].Image);
			Name4.text = Objects [3].Name.ToString ();
			Quantity4.text = Objects [3].Quantity.ToString ();
			if(state != 6)
			panelObjectPanel.GetComponent<RectTransform> ().offsetMin = new Vector2 (panelObjectPanel.GetComponent<RectTransform> ().offsetMin.x, 65);
		} else{
			alpha.a = 0;
			Image4.color = alpha;
			Name4.text = "";
			Quantity4.text = "";
		}
		if (Objects.Count >= 5){
			alpha.a = 1;
			Image5.color = alpha;
			Image5.sprite = Resources.Load<Sprite> (Objects [4].Image);
			Name5.text = Objects [4].Name.ToString ();
			Quantity5.text = Objects [4].Quantity.ToString ();
			if(state != 6)
			panelObjectPanel.GetComponent<RectTransform> ().offsetMin = new Vector2 (panelObjectPanel.GetComponent<RectTransform> ().offsetMin.x, 0);
		} else{
			alpha.a = 0;
			Image5.color = alpha;
			Name5.text = "";
			Quantity5.text = "";
		}

	}

	private void ShowObjects2 (){
		Color alpha = new Color ();
		alpha.r = 1;
		alpha.g = 1;
		alpha.b = 1;
		// se coloquen en cada sitio sus necesarios


		if (Objects2.Count >= 1){
			alpha.a = 1;
			Image12.color = alpha;
			Image12.sprite = Resources.Load<Sprite> (Objects2 [0].Image);
			Name12.text = Objects2 [0].Name.ToString ();
			Quantity12.text = Objects2 [0].Quantity.ToString ();

		} else{
					
			alpha.a = 0;
			Image12.color = alpha;
			Name12.text = "";
			Quantity12.text = "";
		}
		if (Objects2.Count >= 2){
			alpha.a = 1;
			Image22.color = alpha;
			Image22.sprite = Resources.Load<Sprite> (Objects2 [1].Image);
			Name22.text = Objects2 [1].Name.ToString ();
			Quantity22.text = Objects2 [1].Quantity.ToString ();

		} else{
			alpha.a = 0;
			Image22.color = alpha;
			Name22.text = "";
			Quantity22.text = "";
		}
		if (Objects2.Count >= 3){
			alpha.a = 1;
			Image32.color = alpha;
			Image32.sprite = Resources.Load<Sprite> (Objects2 [2].Image);
			Name32.text = Objects2 [2].Name.ToString ();
			Quantity32.text = Objects2 [2].Quantity.ToString ();

		} else{
			alpha.a = 0;
			Image32.color = alpha;
			Name32.text = "";
			Quantity32.text = "";
		}
		if (Objects2.Count >= 4){
			alpha.a = 1;
			Image42.color = alpha;
			Image42.sprite = Resources.Load<Sprite> (Objects2 [3].Image);
			Name42.text = Objects2 [3].Name.ToString ();
			Quantity42.text = Objects2 [3].Quantity.ToString ();

		} else{
			alpha.a = 0;
			Image42.color = alpha;
			Name42.text = "";
			Quantity42.text = "";
		}
		if (Objects2.Count >= 5){
			alpha.a = 1;
			Image52.color = alpha;
			Image52.sprite = Resources.Load<Sprite> (Objects2 [4].Image);
			Name52.text = Objects2 [4].Name.ToString ();
			Quantity52.text = Objects2 [4].Quantity.ToString ();

		} else{
			alpha.a = 0;
			Image52.color = alpha;
			Name52.text = "";
			Quantity52.text = "";
		}

	}

	private void ShowObjectValues (){
		int index = Mathf.FloorToInt (SliderObjects.value);
		if (Objects [index].Type == "Weapon"){
			panelValuesObjectUsable.SetActive (false);
			panelValuesObjectWeapons.SetActive (true);
			QuantityAttack.text = Objects [index].Attack.ToString ();
			QuantityCritical.text = Objects [index].Critical.ToString ();
			QuantityDodge.text = Objects [index].Dodge.ToString ();
			QuantityHit.text = Objects [index].Accuracy.ToString ();
		} else{
			panelValuesObjectUsable.SetActive (true);
			panelValuesObjectWeapons.SetActive (true);
			Description.text = Objects [index].Description;
		}
	}


	// Comprueba si se pulsan dos teclas a la vez.
	private bool CheckTwoAtaTime (){
		bool ok = false;

		if (Time.timeScale > 0){ 
			if (Input.GetKeyDown (KeyCode.A) && Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.A) && Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.D) && Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.D) && Input.GetKeyDown (KeyCode.S)){
				ok = true;
			} else{
				ok = false;
			}
		}
		return ok;
	}

	private void JustASec (){ // PARA MOV CURSOR

		
	}
	// Elimina todas las lineas y Transparencias.
	private void BorraTransLinea (){
		
		Object[] objs = Lines.FindSceneObjectsOfType (typeof(Lines));
		foreach (Lines g in objs){
			Destroy (g.gameObject);
		}
		Object[] objss = Transparency.FindSceneObjectsOfType (typeof(Transparency));
		foreach (Transparency g in objss){
			Destroy (g.gameObject);
		}
	}


	private void EnsureMap (){
		transform.position = new Vector3 (Mathf.Clamp (transform.position.x, 0.5f, myMap.Right + 0.5f), Mathf.Clamp (transform.position.y, 0.5f, myMap.Up + 0.5f), 0);
	}

	// Genera el campo en el que un aliado se puede mover. que genere solo hasta los limites del mapa y no mas.
	private void CampoDeVision (Ally myAlly){

		state = 1;
		int myAllyX = Mathf.FloorToInt (myAlly.transform.position.x);
		int myAllyY = Mathf.FloorToInt (myAlly.transform.position.y);
		Spot[] path = new Spot[(myMap.Right + 1) * (myMap.Up + 1)];
		Spot posInPath = gameObject.AddComponent<Spot> ();

		for (int i = 0; i <= myAlly.movement; i++){ // arriba derecha
			for (int j = 0; j <= myAlly.movement - i; j++){
				if (myAllyX + i > myMap.Right || myAllyY + j > myMap.Up){
					break;
				}

				if (myMap.GetValueScenary (myAllyX + i, myAllyY + j) != Types.Ocupaid){
					posInPath.x = 0;
					if (myMap.isPathFinding (myAllyX, myAllyY, myAllyX + i, myAllyY + j, myAlly, path, posInPath)){
						Instantiate (BlueTransparency, new Vector3 (myAllyX + 0.5f + i, myAllyY + 0.5f + j, 0), Quaternion.identity);

					}
				}
			}
		}

		for (int i = 1; i <= myAlly.movement; i++){ // abjo derecha
			for (int j = 1; j <= myAlly.movement - i; j++){
				if (myAllyX + i > myMap.Right || myAllyY - j < 0){
					break;
				}

				if (myMap.GetValueScenary (myAllyX + i, myAllyY - j) != Types.Ocupaid){
					posInPath.x = 0;
					if (myMap.isPathFinding (myAllyX, myAllyY, myAllyX + i, myAllyY - j, myAlly, path, posInPath)){
						Instantiate (BlueTransparency, new Vector3 (myAllyX + 0.5f + i, myAllyY + 0.5f - j, 0), Quaternion.identity);
					}


				} 

			}
		}

		for (int i = 1; i <= myAlly.movement; i++){ // arriba izquierda
			for (int j = 1; j <= myAlly.movement - i; j++){

				if (myAllyX - i < 0 || myAllyY + j > myMap.Up){

					break;
				}
				if (myMap.GetValueScenary (myAllyX - i, myAllyY + j) != Types.Ocupaid){
					posInPath.x = 0;
					if (myMap.isPathFinding (myAllyX, myAllyY, myAllyX - i, myAllyY + j, myAlly, path, posInPath)){
						Instantiate (BlueTransparency, new Vector3 (myAllyX + 0.5f - i, myAllyY + 0.5f + j, 0), Quaternion.identity);
					}

				
				} 
			}
		}

		for (int i = 0; i <= myAlly.movement; i++){ // abajo izquierda
			for (int j = 0; j <= myAlly.movement - i; j++){
				if (myAllyX - i < 0 || myAllyY - j < 0){
					break;
				}

				if (i != 0 || j != 0){
					if (myMap.GetValueScenary (myAllyX - i, myAllyY - j) != Types.Ocupaid){
						posInPath.x = 0;
						if (myMap.isPathFinding (myAllyX, myAllyY, myAllyX - i, myAllyY - j, myAlly, path, posInPath)){
							Instantiate (BlueTransparency, new Vector3 (myAllyX + 0.5f - i, myAllyY + 0.5f - j, 0), Quaternion.identity);
						}

					
					} 
				}
			}
		}


		// RED TRANSPARENCIES	// cuando haga que la matriz soporte las transparencias quitar el foreach y acceder directo
								// el ispathFinding lo podrias sustituir por si hay una blue transparency alli o no en O(1)!!!

		for (int i = 0; i <= myAlly.movement; i++){ // arriba derecha
			for (int j = 0; j <= myAlly.movement - i; j++){
				if (myAllyX + i > myMap.Right || myAllyY + j > myMap.Up){
					break;
				}
				posInPath.x = 0;
				if (myMap.isPathFinding (myAllyX, myAllyY, myAllyX + i, myAllyY + j, myAlly, path, posInPath)){
					// mira neighbors si tienen tranparency si no pone la roja
					Object[] objs = Transparency.FindSceneObjectsOfType (typeof(Transparency));
					bool isTransparency1 = false;
					bool isTransparency2 = false;
					bool isTransparency3 = false;
					bool isTransparency4 = false;
					foreach (Transparency g in objs){
						if (g.transform.position.x == myAllyX + i + 1 + 0.5f && g.transform.position.y == myAllyY + j + 0.5f){
							isTransparency1 = true;
						}
						if (g.transform.position.x == myAllyX + i - 1 + 0.5f && g.transform.position.y == myAllyY + j + 0.5f){
							isTransparency2 = true;
						}
						if (g.transform.position.x == myAllyX + i + 0.5f && g.transform.position.y == myAllyY + j + 1 + 0.5f){
							isTransparency3 = true;
						}
						if (g.transform.position.x == myAllyX + i + 0.5f && g.transform.position.y == myAllyY + j - 1 + 0.5f){
							isTransparency4 = true;
						}
						if (isTransparency1 && isTransparency2 && isTransparency3 && isTransparency4){
							break;
						}
					}

					if (!isTransparency1){
						Instantiate (RedTransparency, new Vector3 (myAllyX + 0.5f + i + 1, myAllyY + 0.5f + j, 0), Quaternion.identity);
					}
					if (!isTransparency2){
						Instantiate (RedTransparency, new Vector3 (myAllyX + 0.5f + i - 1, myAllyY + 0.5f + j, 0), Quaternion.identity);
					}
					if (!isTransparency3){
						Instantiate (RedTransparency, new Vector3 (myAllyX + 0.5f + i, myAllyY + 0.5f + j + 1, 0), Quaternion.identity);
					}
					if (!isTransparency4){
						Instantiate (RedTransparency, new Vector3 (myAllyX + 0.5f + i, myAllyY + 0.5f + j - 1, 0), Quaternion.identity);
					}
				}
			}
		}



		for (int i = 1; i <= myAlly.movement; i++){ // abjo derecha
			for (int j = 1; j <= myAlly.movement - i; j++){
				if (myAllyX + i > myMap.Right || myAllyY - j < 0){
					break;
				}
				posInPath.x = 0;
				if (myMap.isPathFinding (myAllyX, myAllyY, myAllyX + i, myAllyY - j, myAlly, path, posInPath)){
					// mira neighbors si tienen tranparency si no pone la roja
					Object[] objs = Transparency.FindSceneObjectsOfType (typeof(Transparency));
					bool isTransparency1 = false;
					bool isTransparency2 = false;
					bool isTransparency3 = false;
					bool isTransparency4 = false;
					foreach (Transparency g in objs){
						if (g.transform.position.x == myAllyX + i + 1 + 0.5f && g.transform.position.y == myAllyY - j + 0.5f){
							isTransparency1 = true;
						}
						if (g.transform.position.x == myAllyX + i - 1 + 0.5f && g.transform.position.y == myAllyY - j + 0.5f){
							isTransparency2 = true;
						}
						if (g.transform.position.x == myAllyX + i + 0.5f && g.transform.position.y == myAllyY - j + 1 + 0.5f){
							isTransparency3 = true;
						}
						if (g.transform.position.x == myAllyX + i + 0.5f && g.transform.position.y == myAllyY - j - 1 + 0.5f){
							isTransparency4 = true;
						}
						if (isTransparency1 && isTransparency2 && isTransparency3 && isTransparency4){
							break;
						}
					}

					if (!isTransparency1){
						Instantiate (RedTransparency, new Vector3 (myAllyX + 0.5f + i + 1, myAllyY + 0.5f - j, 0), Quaternion.identity);
					}
					if (!isTransparency2){
						Instantiate (RedTransparency, new Vector3 (myAllyX + 0.5f + i - 1, myAllyY + 0.5f - j, 0), Quaternion.identity);
					}
					if (!isTransparency3){
						Instantiate (RedTransparency, new Vector3 (myAllyX + 0.5f + i, myAllyY + 0.5f - j + 1, 0), Quaternion.identity);
					}
					if (!isTransparency4){
						Instantiate (RedTransparency, new Vector3 (myAllyX + 0.5f + i, myAllyY + 0.5f - j - 1, 0), Quaternion.identity);
					}
				}
			}
		}



		for (int i = 1; i <= myAlly.movement; i++){ // arriba izquierda
			for (int j = 1; j <= myAlly.movement - i; j++){

				if (myAllyX - i < 0 || myAllyY + j > myMap.Up){

					break;
				}

				posInPath.x = 0;
				if (myMap.isPathFinding (myAllyX, myAllyY, myAllyX - i, myAllyY + j, myAlly, path, posInPath)){
					// mira neighbors si tienen tranparency si no pone la roja
					Object[] objs = Transparency.FindSceneObjectsOfType (typeof(Transparency));
					bool isTransparency1 = false;
					bool isTransparency2 = false;
					bool isTransparency3 = false;
					bool isTransparency4 = false;
					foreach (Transparency g in objs){
						if (g.transform.position.x == myAllyX - i + 1 + 0.5f && g.transform.position.y == myAllyY + j + 0.5f){
							isTransparency1 = true;
						}
						if (g.transform.position.x == myAllyX - i - 1 + 0.5f && g.transform.position.y == myAllyY + j + 0.5f){
							isTransparency2 = true;
						}
						if (g.transform.position.x == myAllyX - i + 0.5f && g.transform.position.y == myAllyY + j + 1 + 0.5f){
							isTransparency3 = true;
						}
						if (g.transform.position.x == myAllyX - i + 0.5f && g.transform.position.y == myAllyY + j - 1 + 0.5f){
							isTransparency4 = true;
						}
						if (isTransparency1 && isTransparency2 && isTransparency3 && isTransparency4){
							break;
						}
					}

					if (!isTransparency1){
						Instantiate (RedTransparency, new Vector3 (myAllyX + 0.5f - i + 1, myAllyY + 0.5f + j, 0), Quaternion.identity);
					}
					if (!isTransparency2){
						Instantiate (RedTransparency, new Vector3 (myAllyX + 0.5f - i - 1, myAllyY + 0.5f + j, 0), Quaternion.identity);
					}
					if (!isTransparency3){
						Instantiate (RedTransparency, new Vector3 (myAllyX + 0.5f - i, myAllyY + 0.5f + j + 1, 0), Quaternion.identity);
					}
					if (!isTransparency4){
						Instantiate (RedTransparency, new Vector3 (myAllyX + 0.5f - i, myAllyY + 0.5f + j - 1, 0), Quaternion.identity);
					}
				}
			}
		}

	



		for (int i = 0; i <= myAlly.movement; i++){ // abajo izquierda
			for (int j = 0; j <= myAlly.movement - i; j++){
				if (myAllyX - i < 0 || myAllyY - j < 0){
					break;
				}

				if (i != 0 || j != 0){
					posInPath.x = 0;
					if (myMap.isPathFinding (myAllyX, myAllyY, myAllyX - i, myAllyY - j, myAlly, path, posInPath)){
						// mira neighbors si tienen tranparency si no pone la roja
						Object[] objs = Transparency.FindSceneObjectsOfType (typeof(Transparency));
						bool isTransparency1 = false;
						bool isTransparency2 = false;
						bool isTransparency3 = false;
						bool isTransparency4 = false;
						foreach (Transparency g in objs){
							if (g.transform.position.x == myAllyX - i + 1 + 0.5f && g.transform.position.y == myAllyY - j + 0.5f){
								isTransparency1 = true;
							}
							if (g.transform.position.x == myAllyX - i - 1 + 0.5f && g.transform.position.y == myAllyY - j + 0.5f){
								isTransparency2 = true;
							}
							if (g.transform.position.x == myAllyX - i + 0.5f && g.transform.position.y == myAllyY - j + 1 + 0.5f){
								isTransparency3 = true;
							}
							if (g.transform.position.x == myAllyX - i + 0.5f && g.transform.position.y == myAllyY - j - 1 + 0.5f){
								isTransparency4 = true;
							}
							if (isTransparency1 && isTransparency2 && isTransparency3 && isTransparency4){
								break;
							}
						}

						if (!isTransparency1){
							Instantiate (RedTransparency, new Vector3 (myAllyX + 0.5f - i + 1, myAllyY + 0.5f - j, 0), Quaternion.identity);
						}
						if (!isTransparency2){
							Instantiate (RedTransparency, new Vector3 (myAllyX + 0.5f - i - 1, myAllyY + 0.5f - j, 0), Quaternion.identity);
						}
						if (!isTransparency3){
							Instantiate (RedTransparency, new Vector3 (myAllyX + 0.5f - i, myAllyY + 0.5f - j + 1, 0), Quaternion.identity);
						}
						if (!isTransparency4){
							Instantiate (RedTransparency, new Vector3 (myAllyX + 0.5f - i, myAllyY + 0.5f - j - 1, 0), Quaternion.identity);
						}
					}
				}
			}
		}



		Destroy (posInPath);
	}

	//*******************
	// Conjunto de Autogeneracion del camino
	//*******************

	private void MueveA (Transform myTransform){
		myTransform.position += new Vector3 (-1, 0, 0);
		Object[] objsss = Lines.FindSceneObjectsOfType (typeof(Lines)); 
		foreach (Lines g in objsss){
			if (g.transform.position == lastPos){
				Destroy (g.gameObject);
				break;
			}
		}

		if (lastQuat == Quaternion.Euler (0, 0, 90) && lastPos != myAlly.transform.position){			
			Instantiate (Line, lastPos, Quaternion.Euler (0, 0, 90));	
		} else{
			bool wasSomethingPut = false;
			Object[] objssss = Lines.FindSceneObjectsOfType (typeof(Lines));
			foreach (Lines g in objssss){
				if (g.transform.position.x == myTransform.position.x + 1 && g.transform.position.y == myTransform.position.y + 1 && lastPos != myAlly.transform.position){
					Instantiate (IzArr, lastPos, Quaternion.Euler (0, 0, 0));
					wasSomethingPut = true;
					break;
				} else if (g.transform.position.x == myTransform.position.x + 1 && g.transform.position.y == myTransform.position.y - 1 && lastPos != myAlly.transform.position){
					Instantiate (IzAba, lastPos, Quaternion.Euler (0, 0, 0));
					wasSomethingPut = true;
					break;
				} 
			}
			if (!wasSomethingPut){
				Instantiate (tester, lastPos, Quaternion.Euler (0, 0, 0));
			}
		}
		Instantiate (EndLineArr, new Vector3 (myTransform.position.x, myTransform.position.y, 0), Quaternion.Euler (0, 0, 90));
		lastPos = myTransform.position;
		lastQuat = Quaternion.Euler (0, 0, 90);
	}

	private void MueveW (Transform myTransform){

		myTransform.position += new Vector3 (0, 1, 0);

		Object[] objsss = Lines.FindSceneObjectsOfType (typeof(Lines)); 
		foreach (Lines g in objsss){
			if (g.transform.position == lastPos){
				Destroy (g.gameObject);
				break;
			}
		}

		if (lastQuat == Quaternion.Euler (0, 0, 0) && lastPos != myAlly.transform.position){
			Instantiate (Line, lastPos, Quaternion.Euler (0, 0, 0));	
		} else{
			bool wasSomethingPut = false;
			Object[] objssss = Lines.FindSceneObjectsOfType (typeof(Lines));
			foreach (Lines g in objssss){
				if (g.transform.position.x == myTransform.position.x - 1 && g.transform.position.y == myTransform.position.y - 1 && lastPos != myAlly.transform.position){
					Instantiate (IzArr, lastPos, Quaternion.Euler (0, 0, 0));
					wasSomethingPut = true;
					break;
				} else if (g.transform.position.x == myTransform.position.x + 1 && g.transform.position.y == myTransform.position.y - 1 && lastPos != myAlly.transform.position){
					Instantiate (DerArr, lastPos, Quaternion.Euler (0, 0, 0));
					wasSomethingPut = true;
					break;	
				} 
			}
			if (!wasSomethingPut){
				Instantiate (tester, lastPos, Quaternion.Euler (0, 0, 0));	
			}

		}
		Instantiate (EndLineArr, new Vector3 (myTransform.position.x, myTransform.position.y, 0), Quaternion.Euler (0, 0, 0));
		lastPos = myTransform.position;
		lastQuat = Quaternion.Euler (0, 0, 0);

	}

	private void MueveD (Transform myTransform){

		myTransform.position += new Vector3 (1, 0, 0);

		Object[] objsss = Lines.FindSceneObjectsOfType (typeof(Lines)); 
		foreach (Lines g in objsss){
			if (g.transform.position == lastPos){
				Destroy (g.gameObject);
				break;
			}
		}

		if (lastQuat == Quaternion.Euler (0, 0, 90) && lastPos != myAlly.transform.position){
			Instantiate (Line, lastPos, Quaternion.Euler (0, 0, 90));	
		} else{
			bool wasSomethingPut = false;
			Object[] objssss = Lines.FindSceneObjectsOfType (typeof(Lines));
			foreach (Lines g in objssss){
				if (g.transform.position.x == myTransform.position.x - 1 && g.transform.position.y == myTransform.position.y + 1 && lastPos != myAlly.transform.position){
					Instantiate (DerArr, lastPos, Quaternion.Euler (0, 0, 0));
					wasSomethingPut = true;
					break;
				} else if (g.transform.position.x == myTransform.position.x - 1 && g.transform.position.y == myTransform.position.y - 1 && lastPos != myAlly.transform.position){
					Instantiate (DerAba, lastPos, Quaternion.Euler (0, 0, 0));	
					wasSomethingPut = true;
					break;
				} 
					
				
			}
			if (!wasSomethingPut){
				Instantiate (tester, lastPos, Quaternion.Euler (0, 0, 0));	
			}
		}
		Instantiate (EndLine, new Vector3 (myTransform.position.x, myTransform.position.y, 0), Quaternion.Euler (0, 0, 90));
		lastPos = myTransform.position;
		lastQuat = Quaternion.Euler (0, 0, 90);
	}

	private void MueveS (Transform myTransform){

		myTransform.position += new Vector3 (0, -1, 0);

		Object[] objsss = Lines.FindSceneObjectsOfType (typeof(Lines)); 
		foreach (Lines g in objsss){
			if (g.transform.position == lastPos){
				Destroy (g.gameObject);
				break;
			}
		}

		if (lastQuat == Quaternion.Euler (0, 0, 0) && lastPos != myAlly.transform.position){
			Instantiate (Line, lastPos, Quaternion.Euler (0, 0, 0));	
		} else{
			bool wasSomethingPut = false;
			Object[] objssss = Lines.FindSceneObjectsOfType (typeof(Lines));
			foreach (Lines g in objssss){
				if (g.transform.position.x == myTransform.position.x - 1 && g.transform.position.y == myTransform.position.y + 1 && lastPos != myAlly.transform.position){
					Instantiate (IzAba, lastPos, Quaternion.Euler (0, 0, 0));
					wasSomethingPut = true;
					break;
				} else if (g.transform.position.x == myTransform.position.x + 1 && g.transform.position.y == myTransform.position.y + 1 && lastPos != myAlly.transform.position){
					Instantiate (DerAba, lastPos, Quaternion.Euler (0, 0, 0));	
					wasSomethingPut = true;
					break;
				} 
					

			}
			if (!wasSomethingPut){
				Instantiate (tester, lastPos, Quaternion.Euler (0, 0, 0));
			}
		}
		Instantiate (EndLine, new Vector3 (myTransform.position.x, myTransform.position.y, 0), Quaternion.Euler (0, 0, 0));
		lastPos = myTransform.position;
		lastQuat = Quaternion.Euler (0, 0, 0);
	}


	// Genera un camino desde la posicion actual hasta las coordenadas indicadas.
	private void AutogeneraciondeCaminos (int Xcord, int Ycord){
		
		int myAllyX = Mathf.FloorToInt (myAlly.transform.position.x);
		int myAllyY = Mathf.FloorToInt (myAlly.transform.position.y);
		Spot[] path = new Spot[(myMap.Right + 1) * (myMap.Up + 1)];
		Spot posInPath = gameObject.AddComponent<Spot> ();
		posInPath.x = 0;





		if (myMap.isPathFinding (myAllyX, myAllyY, Xcord, Ycord, myAlly, path, posInPath)){ // si existe un path updatea las variables

			Object[] objs = Lines.FindSceneObjectsOfType (typeof(Lines));
			foreach (Lines g in objs){
				Destroy (g.gameObject);
			}

			counter = 0;
			lastPos = myAlly.transform.position;
			lastQuat = Quaternion.Euler (2, 2, 2);
			auxCursor.transform.position = myAlly.transform.position;

			System.Array.Resize (ref path, posInPath.x); // dejas solo lo importante del path
			System.Array.Reverse (path); // le das la vuelta ya que antes estaba de final -> principio




			for (int i = 1; i < path.Length; i++){

				if (path [i].x > path [i - 1].x){
					MueveD (auxCursor.transform);
				}
				if (path [i].x < path [i - 1].x){
					MueveA (auxCursor.transform);
				}
				if (path [i].y > path [i - 1].y){
					MueveW (auxCursor.transform);
				}
				if (path [i].y < path [i - 1].y){
					MueveS (auxCursor.transform);
				}
			}

			// decodifica para poner con muves

			counter = path [path.Length - 1].g; // valor de la g
		}
		Destroy (posInPath);
		//************
		// Genera las lineas de movimiento.
		//************
	}

	private void GeneracionDeLineas (){

		if (Input.GetKeyDown (KeyCode.A) && Time.timeScale > 0){ 
			isRectifying = false;
			Object[] objss = Lines.FindSceneObjectsOfType (typeof(Lines)); // Mira si donde esta el cursor hay una linea y la elimina.
			foreach (Lines g in objss){
				if (g.transform.position == transform.position){
					Destroy (g.gameObject);
					counter--;
					if (myMap.matrixScenary [cordX] [cordY].type == Types.Forest){
						counter--;
					}


					isRectifying = true;
					break;
				}
			}
														
			Object[] objsss = Lines.FindSceneObjectsOfType (typeof(Lines)); 	// Mira si en la anterior posicion habia una linea y la elimina.
			foreach (Lines g in objsss){
				if (g.transform.position == lastPos){
					Destroy (g.gameObject);
					break;
				}
			}

			if (isRectifying){ // si se esta rectificando 
				bool isTriggered = false;
				Object[] objssss = Lines.FindSceneObjectsOfType (typeof(Lines)); 	
				foreach (Lines g in objssss){
					if (g.transform.position.x == lastPos.x - 1 && g.transform.position.y == lastPos.y && g.name == "Line(Clone)" && Quaternion.Euler (0, 0, 90) == g.transform.rotation){ // si en laspos esta linea simple , giro de un tipo, y del otro
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 90);
						Instantiate (EndLine, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 90));
						isTriggered = true;
						break;
					} else if (g.transform.position.x == lastPos.x - 1 && g.transform.position.y == lastPos.y && g.name == "DerArr(Clone)"){
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 0);
						Instantiate (EndLine, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 0));
						isTriggered = true;
						break;
					} else if (g.transform.position.x == lastPos.x - 1 && g.transform.position.y == lastPos.y && g.name == "DerAba(Clone)"){
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 0);
						Instantiate (EndLineArr, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 0));
						isTriggered = true;
						break;
					}
				}
				if (!isTriggered){ 
					AutogeneraciondeCaminos (cordX, cordY);
				}
			}

//			if(counter >= myAlly.movement){
//				isCountPlus1 = true;
//			}else{
//				isCountPlus1 = false;
//			}

			if (counter <= myAlly.movement && !isRectifying){	// mira que tiene que poner en la anterior posicion.
				counter++;
				if (myMap.matrixScenary [cordX] [cordY].type == Types.Forest){
					counter++;
				}

				if (lastQuat == Quaternion.Euler (0, 0, 90) && lastPos != myAlly.transform.position){			
					Instantiate (Line, lastPos, Quaternion.Euler (0, 0, 90));	
				} else{
					bool wasSomethingPut = false;
					Object[] objssss = Lines.FindSceneObjectsOfType (typeof(Lines));
					foreach (Lines g in objssss){
						if (g.transform.position.x == transform.position.x + 1 && g.transform.position.y == transform.position.y + 1 && lastPos != myAlly.transform.position){
							Instantiate (IzArr, lastPos, Quaternion.Euler (0, 0, 0));
							wasSomethingPut = true;
							break;
						} else if (g.transform.position.x == transform.position.x + 1 && g.transform.position.y == transform.position.y - 1 && lastPos != myAlly.transform.position){
							Instantiate (IzAba, lastPos, Quaternion.Euler (0, 0, 0));
							wasSomethingPut = true;
							break;
						} 
					}
					if (!wasSomethingPut){
						Instantiate (tester, lastPos, Quaternion.Euler (0, 0, 0));
					}
				}

				// pone en esta posicion.
				Instantiate (EndLineArr, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 90));
				lastPos = transform.position;
				lastQuat = Quaternion.Euler (0, 0, 90);
			}

			if (counter > myAlly.movement){ // es 5 
				isCountPlus1 = true;
			} else{
				isCountPlus1 = false;
			}

			if ((transform.position == myAlly.transform.position || isCountPlus1 || wasRecentlyOut)){
				AutogeneraciondeCaminos (cordX, cordY);
				wasRecentlyOut = false;
			}

		}

		if (Input.GetKeyDown (KeyCode.W) && Time.timeScale > 0){

			isRectifying = false;

			Object[] objss = Lines.FindSceneObjectsOfType (typeof(Lines)); // Mira si donde esta el cursor hay una linea y la elimina.
			foreach (Lines g in objss){
				if (g.transform.position == transform.position){
					Destroy (g.gameObject);
					counter--;
					if (myMap.matrixScenary [cordX] [cordY].type == Types.Forest){
						counter--;
					}
					isRectifying = true;
					break;
				}
			}

																		
			Object[] objsss = Lines.FindSceneObjectsOfType (typeof(Lines)); 	// Mira si en la anterior posicion habia una linea y la elimina.
			foreach (Lines g in objsss){
				if (g.transform.position == lastPos){
					Destroy (g.gameObject);
					break;
				}
			}

			if (isRectifying){ // si se esta rectificando 
				bool isTriggered = false;
				Object[] objssss = Lines.FindSceneObjectsOfType (typeof(Lines)); 	
				foreach (Lines g in objssss){
					if (g.transform.position.x == lastPos.x && g.transform.position.y == lastPos.y + 1 && g.name == "Line(Clone)" && Quaternion.Euler (0, 0, 0) == g.transform.rotation){ // si en laspos esta linea simple , giro de un tipo, y del otro
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 0);
						Instantiate (EndLine, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 0));
						isTriggered = true;
						break;
					} else if (g.transform.position.x == lastPos.x && g.transform.position.y == lastPos.y + 1 && g.name == "IzAba(Clone)"){
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 90);
						Instantiate (EndLine, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 90));
						isTriggered = true;
						break;
					} else if (g.transform.position.x == lastPos.x && g.transform.position.y == lastPos.y + 1 && g.name == "DerAba(Clone)"){
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 90);
						Instantiate (EndLineArr, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 90));
						isTriggered = true;
						break;
					}
				}
				if (!isTriggered){ 
					AutogeneraciondeCaminos (cordX, cordY);
				}
			}


			if (counter <= myAlly.movement && !isRectifying){
				counter++;
				if (myMap.matrixScenary [cordX] [cordY].type == Types.Forest){
					counter++;
				}
				if (lastQuat == Quaternion.Euler (0, 0, 0) && lastPos != myAlly.transform.position){
					Instantiate (Line, lastPos, Quaternion.Euler (0, 0, 0));	
				} else{
					bool wasSomethingPut = false;
					Object[] objssss = Lines.FindSceneObjectsOfType (typeof(Lines));
					foreach (Lines g in objssss){
						if (g.transform.position.x == transform.position.x - 1 && g.transform.position.y == transform.position.y - 1 && lastPos != myAlly.transform.position){
							Instantiate (IzArr, lastPos, Quaternion.Euler (0, 0, 0));
							wasSomethingPut = true;
							break;
						} else if (g.transform.position.x == transform.position.x + 1 && g.transform.position.y == transform.position.y - 1 && lastPos != myAlly.transform.position){
							Instantiate (DerArr, lastPos, Quaternion.Euler (0, 0, 0));
							wasSomethingPut = true;
							break;	
						} 
					}
					if (!wasSomethingPut){
						Instantiate (tester, lastPos, Quaternion.Euler (0, 0, 0));	
					}

				}
				Instantiate (EndLineArr, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 0));
				lastPos = transform.position;
				lastQuat = Quaternion.Euler (0, 0, 0);
			}

			if (counter > myAlly.movement){
				isCountPlus1 = true;
			} else{
				isCountPlus1 = false;
			}
			if (transform.position == myAlly.transform.position || isCountPlus1 || wasRecentlyOut){
				AutogeneraciondeCaminos (cordX, cordY);
				wasRecentlyOut = false;
			}
		}

		if (Input.GetKeyDown (KeyCode.D) && Time.timeScale > 0){
			isRectifying = false;
			Object[] objss = Lines.FindSceneObjectsOfType (typeof(Lines));
			foreach (Lines g in objss){
				if (g.transform.position == transform.position){
					Destroy (g.gameObject);
					counter--;
					if (myMap.matrixScenary [cordX] [cordY].type == Types.Forest){
						counter--;
					}
					isRectifying = true;
					break;
				}
			}
				
			Object[] objsss = Lines.FindSceneObjectsOfType (typeof(Lines));
			foreach (Lines g in objsss){
				if (g.transform.position == lastPos){
					Destroy (g.gameObject);
					break;
				}
			}
			
			if (isRectifying){ // si se esta rectificando 
				bool isTriggered = false;
				Object[] objssss = Lines.FindSceneObjectsOfType (typeof(Lines)); 	
				foreach (Lines g in objssss){
					if (g.transform.position.x == lastPos.x + 1 && g.transform.position.y == lastPos.y && g.name == "Line(Clone)" && Quaternion.Euler (0, 0, 90) == g.transform.rotation){ // si en laspos esta linea simple , giro de un tipo, y del otro
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 90);
						Instantiate (EndLineArr, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 90));
						isTriggered = true;
						break;
					} else if (g.transform.position.x == lastPos.x + 1 && g.transform.position.y == lastPos.y && g.name == "IzArr(Clone)"){
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 0);
						Instantiate (EndLine, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 0));
						isTriggered = true;
						break;
					} else if (g.transform.position.x == lastPos.x + 1 && g.transform.position.y == lastPos.y && g.name == "IzAba(Clone)"){
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 0);
						Instantiate (EndLineArr, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 0));
						isTriggered = true;
						break;
					}
				}
				if (!isTriggered){ 
					AutogeneraciondeCaminos (cordX, cordY);
				}
			}



			if (counter <= myAlly.movement && !isRectifying){
				counter++;
				if (myMap.matrixScenary [cordX] [cordY].type == Types.Forest){
					counter++;
				}
				if (lastQuat == Quaternion.Euler (0, 0, 90) && lastPos != myAlly.transform.position){
					Instantiate (Line, lastPos, Quaternion.Euler (0, 0, 90));	
				} else{
					bool wasSomethingPut = false;
					Object[] objssss = Lines.FindSceneObjectsOfType (typeof(Lines));
					foreach (Lines g in objssss){
						if (g.transform.position.x == transform.position.x - 1 && g.transform.position.y == transform.position.y + 1 && lastPos != myAlly.transform.position){
							Instantiate (DerArr, lastPos, Quaternion.Euler (0, 0, 0));
							wasSomethingPut = true;
							break;
						} else if (g.transform.position.x == transform.position.x - 1 && g.transform.position.y == transform.position.y - 1 && lastPos != myAlly.transform.position){
							Instantiate (DerAba, lastPos, Quaternion.Euler (0, 0, 0));	
							wasSomethingPut = true;
							break;
						} 
					
				
					}
					if (!wasSomethingPut){
						Instantiate (tester, lastPos, Quaternion.Euler (0, 0, 0));	
					}
				}
				Instantiate (EndLine, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 90));
				lastPos = transform.position;
				lastQuat = Quaternion.Euler (0, 0, 90);
			}

			if (counter > myAlly.movement){
				isCountPlus1 = true;
			} else{
				isCountPlus1 = false;
			}
			if (transform.position == myAlly.transform.position || isCountPlus1 || wasRecentlyOut){

				AutogeneraciondeCaminos (cordX, cordY);
				wasRecentlyOut = false;
			}
		}

		if (Input.GetKeyDown (KeyCode.S) && Time.timeScale > 0){
			isRectifying = false;
		
			Object[] objss = Lines.FindSceneObjectsOfType (typeof(Lines));
			foreach (Lines g in objss){
				if (g.transform.position == transform.position){
					Destroy (g.gameObject);
					counter--;
					if (myMap.matrixScenary [cordX] [cordY].type == Types.Forest){
						counter--;
					}
					isRectifying = true;
					break;
				}
			}

			Object[] objsss = Lines.FindSceneObjectsOfType (typeof(Lines));
			foreach (Lines g in objsss){
				if (g.transform.position == lastPos){
					Destroy (g.gameObject);
					break;
				}
			}
			
			if (isRectifying){ // si se esta rectificando 
				bool isTriggered = false;
				Object[] objssss = Lines.FindSceneObjectsOfType (typeof(Lines)); 	
				foreach (Lines g in objssss){
					if (g.transform.position.x == lastPos.x && g.transform.position.y == lastPos.y - 1 && g.name == "Line(Clone)" && Quaternion.Euler (0, 0, 0) == g.transform.rotation){ // si en laspos esta linea simple , giro de un tipo, y del otro
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 0);
						Instantiate (EndLineArr, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 0));
						isTriggered = true;
						break;
					} else if (g.transform.position.x == lastPos.x && g.transform.position.y == lastPos.y - 1 && g.name == "IzArr(Clone)"){
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 90);
						Instantiate (EndLine, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 90));
						isTriggered = true;
						break;
					} else if (g.transform.position.x == lastPos.x && g.transform.position.y == lastPos.y - 1 && g.name == "DerArr(Clone)"){
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 90);
						Instantiate (EndLineArr, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 90));
						isTriggered = true;
						break;
					}
				}
				if (!isTriggered){ 
					AutogeneraciondeCaminos (cordX, cordY);
				}
			}


			
			if (counter <= myAlly.movement && !isRectifying){
				counter++;
				if (myMap.matrixScenary [cordX] [cordY].type == Types.Forest){
					counter++;
				}
				if (lastQuat == Quaternion.Euler (0, 0, 0) && lastPos != myAlly.transform.position){
					Instantiate (Line, lastPos, Quaternion.Euler (0, 0, 0));	
				} else{
					bool wasSomethingPut = false;
					Object[] objssss = Lines.FindSceneObjectsOfType (typeof(Lines));
					foreach (Lines g in objssss){
						if (g.transform.position.x == transform.position.x - 1 && g.transform.position.y == transform.position.y + 1 && lastPos != myAlly.transform.position){
							Instantiate (IzAba, lastPos, Quaternion.Euler (0, 0, 0));
							wasSomethingPut = true;
							break;
						} else if (g.transform.position.x == transform.position.x + 1 && g.transform.position.y == transform.position.y + 1 && lastPos != myAlly.transform.position){
							Instantiate (DerAba, lastPos, Quaternion.Euler (0, 0, 0));	
							wasSomethingPut = true;
							break;
						} 
					

					}
					if (!wasSomethingPut){
						Instantiate (tester, lastPos, Quaternion.Euler (0, 0, 0));
					}
				}
				Instantiate (EndLine, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 0));
				lastPos = transform.position;
				lastQuat = Quaternion.Euler (0, 0, 0);
			}
			if (counter > myAlly.movement){
				isCountPlus1 = true;
			} else{
				isCountPlus1 = false;
			}
			if (transform.position == myAlly.transform.position || isCountPlus1 || wasRecentlyOut){

				AutogeneraciondeCaminos (cordX, cordY);
				wasRecentlyOut = false;
			}
		}
	}
}
