using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cursor : MonoBehaviour {

	private Map myMap;
	private int cordX;
	private int cordY;
	private Allie myAllie;
	private Enemy myEnemy;
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
	private Vector3 myAllieLastPos;
	// mira la ultima posicion del aliado
	private int state;
	private bool isCellOcupaid;
	private bool wasRecentlyOut;
	private List<Item> Objects;
	private List<Item> Objects2;
	private List <Spot> usefull = new List <Spot> ();
	private bool isObjectCursorLeft;
	private bool isIntercObjectSelected;
	private bool isAttackAndNoInterc;
	private int totalAllieDealDamage;
	private float totalAllieAccuracyProbability;
	private float totalAllieCriticalProbability;
	private int totalEnemyDealDamage;
	private float totalEnemyAccuracyProbability;
	private float totalEnemyCriticalProbability;
	private int allieVelocity;
	private int enemyVelocity;
	private List<Lines> lineasForMovementAnimation;
	private EnemyTurn myEnemyTurn;



	// IDEA: para el movimiento del personaje al return == 1. 		private List <Spot> currentPath = new List <Spot> ();
	// BUGS: los enemigos se superponen en su ia al moverse.

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
	public Text QuantityWeight;
	public Text QuantityHit;
	public GameObject panelValuesObjectUsable;
	public Text Description;
	public Image FaceMugshot;

	public GameObject panelEqUs;
	public GameObject Equipar;
	public GameObject Usar;
	public Text Tirar;
	public Slider SliderEqUs;

	public GameObject panelAttackValues;
	public Text AllieName;
	public Text EnemyName;
	public Text WeaponEnemyName;
	public Image ImageAllieWeapon;
	public Image ImageEnemyWeapon;
	public Text AllieHp;
	public Text EnemyHp;
	public Text AllieDmg;
	public Text EnemyDmg;
	public Text AllieAccu;
	public Text EnemyAccu;
	public Text AllieCrit;
	public Text EnemyCrit;
	public Text AllieDoubleAttack;
	public Text EnemyDoubleAttack;

	public GameObject panelObjective;

	public GameObject panelTerrainInfo;
	public Text TerrainType;
	public Text DefValue;
	public Text EsqValue;

	public GameObject panelPersonajeInfo;
	public Text Nombre;
	public Text VidaActual;
	public Text VidaMaxima;
	public Image Cara;
	public Image SliderPersonajeInfoFiller;

	public GameObject MarcoBatalla;
	public Text nombreAliado;
	public Text nombreEnemigo;
	public Text nombreArmaAliado;
	public Text nombreArmaEnemigo;
	public Text vidaAliado;
	public Text vidaEnemigo;
	public Image sliderVidaAliadoFiller;
	public Image sliderVidaEnemigoFiller;
	public Image imagenArmaAliado;
	public Image imagenArmaEnemigo;
	public Text vGolAliado;
	public Text vDanAliado;
	public Text vCriAliado;
	public Text vGolEnemigo;
	public Text vDanEnemigo;
	public Text vCriEnemigo;
	public SpriteRenderer platformEnemy;
	public SpriteRenderer platformAllie;
	public GameObject boneAllie;
	public GameObject boneEnemy;
	public GameObject allieAttack;

	public GameObject PanelVictoriaDerrota;
	public Text VictoriaDerrota;

	public GameObject PanelEndTurn;

	public AudioSource MoverCursorEffect;
	public AudioSource SeleccionarCursorEffect;
	public AudioSource SeleccionIncorrectaEffect;
	public AudioSource AllyBackgroundSound;
	public AudioSource EnemyBackgroundSound;
	public AudioSource BattleSound;
	public AudioSource VictorySound;
	public AudioSource DefeatSound;

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
	public GameObject hitAllie;
	public GameObject hitEnemy;


	// state 0 = free movement
	// state 1 = a character has been selected
	// state 2 = options menu spawn.
	// state 3 = Object menu is open
	// state 4 = panelEqUs is open
	// state 5 = choosing adjacent Allie/Enemy;
	// state 6 = Moving between the 2 inventories while INTERC.
	// state 7 = Selecting weapon to attack with.
	// state 8 = AtackValuesMenu menu.
	// state 9 = EndTurn menu.

	void Start () {
		myInventory = FindObjectOfType<Inventory> ();
		myEnemyTurn = FindObjectOfType<EnemyTurn> ();
		state = 0;
		myMap = FindObjectOfType<Map> ();
		InvokeRepeating ("JustASec", 0, 0.15f);
		counter = 0;
		isCellOcupaid = false;

		if (transform.position.y < ((myMap.Up + 1) / 2)) {
			panelObjective.transform.position = new Vector3 (myMap.Right - (0.25f * myMap.Right) + 0.5f, myMap.Up - (0.12f * myMap.Up) + 0.5f, 0);
			panelPersonajeInfo.transform.position = new Vector3 (myMap.Right - (0.8f * myMap.Right), myMap.Up - (0.84f * myMap.Up), 0);
		} else {
			panelObjective.transform.position = new Vector3 (myMap.Right - (0.25f * myMap.Right) + 0.5f, myMap.Up * 0.025f + 0.5f, 0);
			panelPersonajeInfo.transform.position = new Vector3 (myMap.Right - (0.8f * myMap.Right), myMap.Up - (0.1f * myMap.Up), 0);
		}
		panelTerrainInfo.transform.position = new Vector3 (myMap.Right - (0.09f * myMap.Right) + 0.5f, myMap.Up * 0.1f + 0.5f, 0);
		UpdatePersonajeInfo ();
	
	}

	void Update () {			
		// Realiza los movimientos basicos
		if (!CheckTwoAtaTime ()) {
			if (Input.GetKeyDown (KeyCode.A) && state < 2 && state != -1) {
				transform.position += new Vector3 (-1, 0, 0);
				EnsureMap ();
				if (state == 0) {
					panelTerrainInfo.SetActive (true);
					bool isAllieOrEnemy = false;
					Allie[] listAllie = FindObjectsOfType<Allie> ();
					foreach (Allie go in listAllie) {
						if (go.transform.position.x == transform.position.x && go.transform.position.y == transform.position.y) {
							isAllieOrEnemy = true;
							panelPersonajeInfo.SetActive (true);
							if (transform.position.y < ((myMap.Up + 1) / 2)) {
								panelPersonajeInfo.transform.position = new Vector3 (myMap.Right - (0.8f * myMap.Right), myMap.Up - (0.1f * myMap.Up), 0);
							} else {
								if (transform.position.x < ((myMap.Right) / 2)) {
									panelPersonajeInfo.transform.position = new Vector3 (myMap.Right - (0.8f * myMap.Right), myMap.Up - (0.84f * myMap.Up), 0);
								} else {
									panelPersonajeInfo.transform.position = new Vector3 (myMap.Right - (0.8f * myMap.Right), myMap.Up - (0.1f * myMap.Up), 0);
								}
							}
							Nombre.text = go.Name;
							VidaActual.text = go.Hp.ToString ();
							VidaMaxima.text = go.MaxHp.ToString ();
							SliderPersonajeInfoFiller.fillAmount = go.Hp / (float)go.MaxHp;
							Cara.sprite = go.FaceImage;
							break;
						}
					}
					Enemy[] listEnemy = FindObjectsOfType<Enemy> ();
					foreach (Enemy go in listEnemy) {
						if (go.transform.position.x == transform.position.x && go.transform.position.y == transform.position.y) {
							isAllieOrEnemy = true;
							panelPersonajeInfo.SetActive (true);
							Nombre.text = go.Name;
							VidaActual.text = go.Hp.ToString ();
							VidaMaxima.text = go.MaxHp.ToString ();
							SliderPersonajeInfoFiller.fillAmount = go.Hp / (float)go.MaxHp;
							Cara.sprite = go.FaceImage;
							break;
						} 
						if (!isAllieOrEnemy) {
							panelPersonajeInfo.SetActive (false);
						}
					}
				}

				if (transform.position.x < ((myMap.Right) / 2)) {
					panelObjective.transform.position = new Vector3 (myMap.Right - (0.25f * myMap.Right) + 0.5f, myMap.Up - (0.12f * myMap.Up) + 0.5f, 0);
					panelTerrainInfo.transform.position = new Vector3 (myMap.Right - (0.09f * myMap.Right) + 0.5f, myMap.Up * 0.1f + 0.5f, 0);
				}
				TerrainType.text = myMap.GetTerrainValue (Mathf.FloorToInt (transform.position.x), Mathf.FloorToInt (transform.position.y));
				EsqValue.text = calculateTerrainEvasion (transform.position).ToString ();	// estoy considerando que no puedo estar en posiciones bloqueadas ni tmpc 
				DefValue.text = calculateTerrainDefense (transform.position).ToString ();	// los enemigos
			}

			if (Input.GetKeyDown (KeyCode.W) && state < 2 && state != -1) {
				transform.position += new Vector3 (0, 1, 0);
				EnsureMap ();
				if (state == 0) {
					panelTerrainInfo.SetActive (true);
					UpdatePersonajeInfo ();
				}
				if (transform.position.x > ((myMap.Right) / 2)) {
					panelPersonajeInfo.transform.position = new Vector3 (myMap.Right - (0.8f * myMap.Right), myMap.Up - (0.1f * myMap.Up), 0);
					if (transform.position.y < ((myMap.Up + 1) / 2)) {
						panelObjective.transform.position = new Vector3 (myMap.Right - (0.25f * myMap.Right) + 0.5f, myMap.Up - (0.12f * myMap.Up) + 0.5f, 0);
					} else {
						panelObjective.transform.position = new Vector3 (myMap.Right - (0.25f * myMap.Right) + 0.5f, myMap.Up * 0.025f + 0.5f, 0);
					}
				} else {
					if (transform.position.y < ((myMap.Up + 1) / 2)) {
						panelPersonajeInfo.transform.position = new Vector3 (myMap.Right - (0.8f * myMap.Right), myMap.Up - (0.1f * myMap.Up), 0);
					} else {
						panelPersonajeInfo.transform.position = new Vector3 (myMap.Right - (0.8f * myMap.Right), myMap.Up - (0.84f * myMap.Up), 0);
					}
				}
				TerrainType.text = myMap.GetTerrainValue (Mathf.FloorToInt (transform.position.x), Mathf.FloorToInt (transform.position.y));
				EsqValue.text = calculateTerrainEvasion (transform.position).ToString ();
				DefValue.text = calculateTerrainDefense (transform.position).ToString ();
			}

			if (Input.GetKeyDown (KeyCode.D) && state < 2 && state != -1) {
				transform.position += new Vector3 (1, 0, 0);
				EnsureMap ();
				if (state == 0) {
					panelTerrainInfo.SetActive (true);
					bool isAllieOrEnemy = false;
					Allie[] listAllie = FindObjectsOfType<Allie> ();
					foreach (Allie go in listAllie) {
						if (go.transform.position.x == transform.position.x && go.transform.position.y == transform.position.y) {
							isAllieOrEnemy = true;
							panelPersonajeInfo.SetActive (true);
							if (transform.position.y < ((myMap.Up + 1) / 2)) {
								panelPersonajeInfo.transform.position = new Vector3 (myMap.Right - (0.8f * myMap.Right), myMap.Up - (0.1f * myMap.Up), 0);
							} else {
								if (transform.position.x < ((myMap.Right) / 2)) {
									panelPersonajeInfo.transform.position = new Vector3 (myMap.Right - (0.8f * myMap.Right), myMap.Up - (0.84f * myMap.Up), 0);
								} else {
									panelPersonajeInfo.transform.position = new Vector3 (myMap.Right - (0.8f * myMap.Right), myMap.Up - (0.1f * myMap.Up), 0);
								}
							}
							Nombre.text = go.Name;
							VidaActual.text = go.Hp.ToString ();
							VidaMaxima.text = go.MaxHp.ToString ();
							SliderPersonajeInfoFiller.fillAmount = go.Hp / (float)go.MaxHp;
							Cara.sprite = go.FaceImage;
							break;
						}
					}
					Enemy[] listEnemy = FindObjectsOfType<Enemy> ();
					foreach (Enemy go in listEnemy) {
						if (go.transform.position.x == transform.position.x && go.transform.position.y == transform.position.y) {
							isAllieOrEnemy = true;
							panelPersonajeInfo.SetActive (true);
							Nombre.text = go.Name;
							VidaActual.text = go.Hp.ToString ();
							VidaMaxima.text = go.MaxHp.ToString ();
							SliderPersonajeInfoFiller.fillAmount = go.Hp / (float)go.MaxHp;
							Cara.sprite = go.FaceImage;
							break;
						} 
						if (!isAllieOrEnemy) {
							panelPersonajeInfo.SetActive (false);
						}
					}
				}
				if (transform.position.x > ((myMap.Right) / 2)) {
					if (transform.position.y < ((myMap.Up + 1) / 2)) {
						panelObjective.transform.position = new Vector3 (myMap.Right - (0.25f * myMap.Right) + 0.5f, myMap.Up - (0.12f * myMap.Up) + 0.5f, 0);
					} else {
						panelObjective.transform.position = new Vector3 (myMap.Right - (0.25f * myMap.Right) + 0.5f, myMap.Up * 0.025f + 0.5f, 0);
					}
					panelTerrainInfo.transform.position = new Vector3 (myMap.Right - (0.875f * myMap.Right), myMap.Up * 0.1f + 0.5f, 0);
				}
				TerrainType.text = myMap.GetTerrainValue (Mathf.FloorToInt (transform.position.x), Mathf.FloorToInt (transform.position.y));
				EsqValue.text = calculateTerrainEvasion (transform.position).ToString ();
				DefValue.text = calculateTerrainDefense (transform.position).ToString ();
			}

			if (Input.GetKeyDown (KeyCode.S) && state < 2 && state != -1) {
				transform.position += new Vector3 (0, -1, 0);
				EnsureMap ();
				if (state == 0) {
					panelTerrainInfo.SetActive (true);
					UpdatePersonajeInfo ();
				}

				if (transform.position.x > ((myMap.Right) / 2)) {
					panelPersonajeInfo.transform.position = new Vector3 (myMap.Right - (0.8f * myMap.Right), myMap.Up - (0.1f * myMap.Up), 0);
					if (transform.position.y < ((myMap.Up + 1) / 2)) {
						panelObjective.transform.position = new Vector3 (myMap.Right - (0.25f * myMap.Right) + 0.5f, myMap.Up - (0.12f * myMap.Up) + 0.5f, 0);
					} else {
						panelObjective.transform.position = new Vector3 (myMap.Right - (0.25f * myMap.Right) + 0.5f, myMap.Up * 0.025f + 0.5f, 0);
					}
				} else {
					if (transform.position.y < ((myMap.Up + 1) / 2)) {
						panelPersonajeInfo.transform.position = new Vector3 (myMap.Right - (0.8f * myMap.Right), myMap.Up - (0.1f * myMap.Up), 0);
					} else {
						panelPersonajeInfo.transform.position = new Vector3 (myMap.Right - (0.8f * myMap.Right), myMap.Up - (0.84f * myMap.Up), 0);
					}
				}
				TerrainType.text = myMap.GetTerrainValue (Mathf.FloorToInt (transform.position.x), Mathf.FloorToInt (transform.position.y));
				EsqValue.text = calculateTerrainEvasion (transform.position).ToString ();
				DefValue.text = calculateTerrainDefense (transform.position).ToString ();
			}
		}

		// Genera las coordenadas en Integers.
		cordX = Mathf.FloorToInt (transform.position.x); // la da por debajo siempre
		cordY = Mathf.FloorToInt (transform.position.y);

		// Generacion de Lineas de Movimiento
		if (state == 1 && myMap.GetValueScenary (cordX, cordY) != Types.Ocupaid && (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.S))) {
			// indica si el cursor esta fuera del area.

			MoverCursorEffect.Play ();
		
			List<Spot> path = new List<Spot> ();
			if (!myMap.isPathFinding (Mathf.FloorToInt (myAllie.transform.position.x), Mathf.FloorToInt (myAllie.transform.position.y), cordX, cordY, myAllie.movement, path)) {
				isOutOfArea = true;
				wasRecentlyOut = true;
			} else {
				isOutOfArea = false;
			}

			if (CheckTwoAtaTime () || isCellOcupaid || isOutOfArea) {
				AutogeneraciondeCaminos (cordX, cordY);
			} else {
				GeneracionDeLineas ();
			}
			isCellOcupaid = false;
		} else if (myMap.GetValueScenary (cordX, cordY) == Types.Ocupaid) {
			isCellOcupaid = true;
			isOutOfArea = true;
			wasRecentlyOut = true;
		}

		if (state == 5 && (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.D))) {

			MoverCursorEffect.Play ();

			for (int i = 0; i < usefull.Count; i++) {
				if (transform.position.x == usefull [i].x + 0.5f && transform.position.y == usefull [i].y + 0.5f) {
					i++;
					if (i > usefull.Count - 1) {
						i = 0;
					}
					transform.position = new Vector3 (usefull [i].x + 0.5f, usefull [i].y + 0.5f);
					break;
				}
			}
		}

		// Mira si en la casilla hay algo cuando se pulsa en el estado 0.
		if (Input.GetKeyDown (KeyCode.Return) && state == 0) {
			if (myMap.GetValue (cordX, cordY) == null || myMap.GetValue (cordX, cordY) == Types.Enemy) {	// se abra otro menu
				PanelEndTurn.SetActive (true);
				panelTerrainInfo.SetActive (false);
				panelPersonajeInfo.SetActive (false);
				panelObjective.SetActive (false);
				state = 9;
				GetComponent<SpriteRenderer> ().sortingOrder = -3;
				if (transform.position.x >= myMap.Right / 2) {
					PanelEndTurn.transform.position = new Vector3 (myMap.Right - (0.8f * myMap.Right), PanelEndTurn.transform.position.y, 0);
				} else {
					PanelEndTurn.transform.position = new Vector3 (myMap.Right - (0.1f * myMap.Right), PanelEndTurn.transform.position.y, 0);
				}
			} else if (myMap.GetValue (cordX, cordY) == Types.Allie) { 
				SeleccionarCursorEffect.Play ();
				Vector3 position = new Vector3 (Mathf.FloorToInt (transform.position.x), Mathf.FloorToInt (transform.position.y), 0);

				Allie[] listAllie = FindObjectsOfType<Allie> ();
				myAllie = null;
   			
				foreach (Allie go in listAllie) {
					if (Mathf.FloorToInt (go.transform.position.x) == position.x && Mathf.FloorToInt (go.transform.position.y) == position.y) {
						myAllie = go;
						break;
					}
				}
			
				if (myAllie.getDisponibility ()) {
					panelObjective.SetActive (false);
					panelTerrainInfo.SetActive (false);
					panelPersonajeInfo.SetActive (false);
					state = 1;
					if (!myAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Selected")) {
						myAllie.GetComponent<Animator> ().SetTrigger ("Selected");
					}
					isOutOfArea = false;
					CampoDeVision (myAllie);
					lastPos = transform.position;
					lastQuat = Quaternion.Euler (2, 2, 2);
				} else {
					PanelEndTurn.SetActive (true);
					panelTerrainInfo.SetActive (false);
					panelPersonajeInfo.SetActive (false);
					panelObjective.SetActive (false);
					state = 9;
					GetComponent<SpriteRenderer> ().sortingOrder = -3;
					if (transform.position.x >= myMap.Right / 2) {
						PanelEndTurn.transform.position = new Vector3 (myMap.Right - (0.8f * myMap.Right), PanelEndTurn.transform.position.y, 0);
					} else {
						PanelEndTurn.transform.position = new Vector3 (myMap.Right - (0.1f * myMap.Right), PanelEndTurn.transform.position.y, 0);
					}
				}
			
			}

			// Se va a la posicion indicada y abre el menu cuando se pulsa en el estado 1.
		} else if (Input.GetKeyDown (KeyCode.Return) && !isOutOfArea && state == 1 && !(Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.S))) {
			bool posHasAllie = false;
			Allie[] listAllie = FindObjectsOfType<Allie> ();
		
			foreach (Allie go in listAllie) {
				if (transform.position == go.transform.position && go.GetComponent<Allie> () && go.ID != myAllie.ID) {
					posHasAllie = true;
					break;
				}
			}
 
			if (!posHasAllie) {
				SeleccionarCursorEffect.Play ();
				myAllieLastPos = myAllie.transform.position;
				BorraTransLinea ();
				StartCoroutine (AnimationMovement ());
			}
			// Ejecuta una accion cuando se pulsa en el estado 2.
		} else if (Input.GetKeyDown (KeyCode.Return) && state == 2) {
			BorraTransLinea ();
			if (SliderOptions.value == 0) { // ATTACK
				isAttackAndNoInterc = true;
				usefull.Clear ();


				List<Spot> neighbors = new List<Spot> ();

				int attackDistance = myInventory.getAllieWeapons (myAllie.ID) [0].AttackDistance;

				if (attackDistance == 1 || attackDistance == 12) {
					neighbors.AddRange (myMap.matrixScenary [cordX] [cordY].getNeighbors (myMap.matrixScenary, myMap.Right, myMap.Up));
				}
				if (attackDistance == 2 || attackDistance == 12) {
					neighbors.AddRange (myMap.matrixScenary [cordX] [cordY].getFarNeighbors (myMap.matrixScenary, myMap.Right, myMap.Up));
				}

				foreach (Spot a in neighbors) {
					if (myMap.matrixScenary [a.x] [a.y].personType == Types.Enemy) {
						usefull.Add (a);
					}
				}
			
				// ya tenemos la lista usefull con los posibles spot. pasar a state 5
				if (usefull.Count > 0) {
					SeleccionarCursorEffect.Play ();
					state = 7;
					panelOptions.SetActive (false);
					panelObjects.SetActive (true);
					panelValuesObject.SetActive (true);
					FaceMugshot.sprite = myAllie.FaceImage;
					SliderObjects.value = 0;
					SliderObjects.gameObject.SetActive (true);
					Objects = myInventory.getAllieWeapons (myAllie.ID);
					ShowObjects ();
					ShowObjectValues ();

					attackDistance = myInventory.getAllieWeapons (myAllie.ID) [0].AttackDistance;

					if (attackDistance == 1 || attackDistance == 12) {
						foreach (Spot a in myMap.matrixScenary[Mathf.FloorToInt(myAllie.transform.position.x)][Mathf.FloorToInt(myAllie.transform.position.y)].getNeighbors(myMap.matrixScenary,myMap.Right,myMap.Up)) {
							Instantiate (RedTransparency, new Vector3 (a.x + 0.5f, a.y + 0.5f, 0), Quaternion.identity);
						}
					}

					if (attackDistance == 2 || attackDistance == 12) {
						foreach (Spot a in myMap.matrixScenary[Mathf.FloorToInt(myAllie.transform.position.x)][Mathf.FloorToInt(myAllie.transform.position.y)].getFarNeighbors(myMap.matrixScenary,myMap.Right,myMap.Up)) {
							Instantiate (RedTransparency, new Vector3 (a.x + 0.5f, a.y + 0.5f, 0), Quaternion.identity);
						}
					}
				} else {
					if (!SeleccionIncorrectaEffect.isPlaying) {
						SeleccionIncorrectaEffect.Play ();
					}
				}
			}
			if (SliderOptions.value == 1) { // OBJECTS
				if (myInventory.getAllieItems (myAllie.ID).Count > 0) {
					SeleccionarCursorEffect.Play ();
					state = 3;
					Objects = myInventory.getAllieItems (myAllie.ID);
					panelOptions.SetActive (false);
					panelObjects.SetActive (true);
					panelValuesObject.SetActive (true);
					FaceMugshot.sprite = myAllie.FaceImage;
					SliderObjects.value = 0;
					SliderObjects.gameObject.SetActive (true);
					ShowObjectValues ();
					ShowObjects ();
				} else {
					if (!SeleccionIncorrectaEffect.isPlaying) {
						SeleccionIncorrectaEffect.Play ();
					}
				}

			}
			if (SliderOptions.value == 2) { // INTERC
				isAttackAndNoInterc = false;
				usefull.Clear ();
				List<Spot> neighbors = myMap.matrixScenary [cordX] [cordY].getNeighbors (myMap.matrixScenary, myMap.Right, myMap.Up);

				foreach (Spot a in neighbors) {
					if (myMap.matrixScenary [a.x] [a.y].personType == Types.Allie && (a.x + 0.5f != myAllieLastPos.x || a.y + 0.5f != myAllieLastPos.y)) {
						usefull.Add (a);
					}
				}
				neighbors = null;
				// ya tenemos la lista usefull con los posibles spot. pasar a state 5
				if (usefull.Count > 0) {
					SeleccionarCursorEffect.Play ();
					state = 5;
					panelOptions.SetActive (false);
					if (transform.position.x - 1 == usefull [0].x + 0.5f) {
						if (!myAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("MovementA")) {
							myAllie.GetComponent<Animator> ().SetTrigger ("A");
						}
					}
					if (transform.position.x + 1 == usefull [0].x + 0.5f) {
						if (!myAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("MovementD")) {
							myAllie.GetComponent<Animator> ().SetTrigger ("D");
						}
					}
					if (transform.position.y - 1 == usefull [0].y + 0.5f) {
						if (!myAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("MovementS")) {
							myAllie.GetComponent<Animator> ().SetTrigger ("S");
						}
					}
					if (transform.position.y + 1 == usefull [0].y + 0.5f) {
						if (!myAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("MovementW")) {
							myAllie.GetComponent<Animator> ().SetTrigger ("W");
						}
					}
					transform.position = new Vector3 (usefull [0].x + 0.5f, usefull [0].y + 0.5f);
				} else {
					if (!SeleccionIncorrectaEffect.isPlaying) {
						SeleccionIncorrectaEffect.Play ();
					}
				}
			}
			if (SliderOptions.value == 3) { // ESPERAR
				SeleccionarCursorEffect.Play ();
				state = 0;
				BorraTransLinea ();
				panelOptions.SetActive (false);
				myAllie.setDisponibility (false);
				myMap.SetValue (Mathf.FloorToInt (myAllieLastPos.x), Mathf.FloorToInt (myAllieLastPos.y), null);
				myMap.SetValue (cordX, cordY, Types.Allie);
				myAllie.GetComponent<Animator> ().SetTrigger ("BackToIdle");
				panelObjective.SetActive (true);
				panelTerrainInfo.SetActive (true);

				panelPersonajeInfo.SetActive (true);
				Nombre.text = myAllie.Name;
				VidaActual.text = myAllie.Hp.ToString ();
				VidaMaxima.text = myAllie.MaxHp.ToString ();
				SliderPersonajeInfoFiller.fillAmount = myAllie.Hp / (float)myAllie.MaxHp;
				Cara.sprite = myAllie.FaceImage;
				if (checkEndTurn ()) {
					EndTurn ();
				}
			}
		} else if (Input.GetKeyDown (KeyCode.Return) && state == 3) {
			SeleccionarCursorEffect.Play ();
			panelEqUs.SetActive (true);
			state = 4;
			SliderEqUs.value = 0;
			int index = Mathf.FloorToInt (SliderObjects.value);
			if (Objects [index].Type == "Weapon") {
				Equipar.SetActive (true);
				Usar.SetActive (false);
			} else {
				Equipar.SetActive (false);
				Usar.SetActive (true);
			}
		} else if (Input.GetKeyDown (KeyCode.Return) && state == 4) {
			SeleccionarCursorEffect.Play ();
			if (SliderEqUs.value == 0 && Equipar.activeSelf) { // equipar
				myInventory.SwapItems (myAllie.ID, myAllie.ID, Mathf.FloorToInt (SliderObjects.value), 0);
				Objects = myInventory.getAllieItems (myAllie.ID);

				Image1.sprite = Resources.Load<Sprite> (Objects [0].Image);
				Name1.text = Objects [0].Name.ToString ();
				Quantity1.text = Objects [0].Quantity.ToString ();

				if (SliderObjects.value == 1) {
					Image2.sprite = Resources.Load<Sprite> (Objects [1].Image);
					Name2.text = Objects [1].Name.ToString ();
					Quantity2.text = Objects [1].Quantity.ToString ();
				} else if (SliderObjects.value == 2) {
					Image3.sprite = Resources.Load<Sprite> (Objects [2].Image);
					Name3.text = Objects [2].Name.ToString ();
					Quantity3.text = Objects [2].Quantity.ToString ();
				} else if (SliderObjects.value == 3) {
					Image4.sprite = Resources.Load<Sprite> (Objects [3].Image);
					Name4.text = Objects [3].Name.ToString ();
					Quantity4.text = Objects [3].Quantity.ToString ();
				} else if (SliderObjects.value == 4) {
					Image5.sprite = Resources.Load<Sprite> (Objects [4].Image);
					Name5.text = Objects [4].Name.ToString ();
					Quantity5.text = Objects [4].Quantity.ToString ();
				}
				panelEqUs.SetActive (false);
				state = 3;
				SliderObjects.value = 0;
				ShowObjectValues ();

			} else if (SliderEqUs.value == 0 && Usar.activeSelf) {	//objeto dependiente del mismo.

			} else if (SliderEqUs.value == 1) { // tirar;
				if (myInventory.getAllieItems (myAllie.ID).Count > 1) {
					myInventory.RemoveItem (myAllie.ID, Mathf.FloorToInt (SliderObjects.value));

					Objects = myInventory.getAllieItems (myAllie.ID);

					ShowObjects ();

					panelEqUs.SetActive (false);
					state = 3;
					SliderObjects.value = 0;
					ShowObjectValues ();
			
				} else {
					myInventory.RemoveItem (myAllie.ID, Mathf.FloorToInt (SliderObjects.value));

					panelEqUs.SetActive (false);
					state = 2;
					SliderObjects.value = 0;
					panelValuesObject.SetActive (false);
					panelObjects.SetActive (false);
					panelOptions.SetActive (true);
					SliderOptions.value = 0;
					ifNeighbourHasEnemySpawnRedTransparency ();
				}
			}
		} else if (Input.GetKeyDown (KeyCode.Return) && state == 5) {
			SeleccionarCursorEffect.Play ();
			if (isAttackAndNoInterc) {
				Enemy[] listEnemy = FindObjectsOfType<Enemy> ();
				foreach (Enemy go in listEnemy) {
					if (go.transform.position == transform.position) {
						myEnemy = go;
						break;
					}
				}

				int weaponTriangle = calculateWeaponTriangleAccuracy ();
				calculateAllieCriticalProbability ();
				isAllieAttackSucced (weaponTriangle);
				calculateAllieDealDamage (weaponTriangle);
				calculateAllieVelocity ();


				calculateEnemyCriticalProbability ();
				isEnemyAttackSucced (weaponTriangle);
				calculateEnemyDealDamage (weaponTriangle);
				calculateEnemyVelocity ();

				ShowAttackValues ();
				state = 8;
			} else {
				state = 6;
				// en transform.position estara el aliado
				Objects = myInventory.getAllieItems (myAllie.ID);
				panelOptions.SetActive (false);
				panelObjects.SetActive (true);
				SliderObjects.value = 0;
				SliderObjects.gameObject.SetActive (true);
				SliderObjects2.gameObject.SetActive (false);
				isObjectCursorLeft = true;
				isIntercObjectSelected = false;
				ShowObjects ();

				Allie[] listAllie = FindObjectsOfType<Allie> ();
				foreach (Allie go in listAllie) {
					if (go.transform.position == transform.position) {
						Objects2 = myInventory.getAllieItems (go.ID);
						break;
					}
				}
				panelObjects2.SetActive (true);
				ShowObjects2 ();
			}
		} else if (Input.GetKeyDown (KeyCode.Return) && state == 6) {
			SeleccionarCursorEffect.Play ();
			if (!isIntercObjectSelected) {
				if (isObjectCursorLeft) {
					SliderObjects2.gameObject.SetActive (true);
					isObjectCursorLeft = false;
					isIntercObjectSelected = true;
				} else {
					SliderObjects.gameObject.SetActive (true);
					isObjectCursorLeft = true;
					isIntercObjectSelected = true;
				}
			} else { // guardar en aux el del lado que sea y eliminar y añadirlo al otro lado
				
				Allie[] listAllie = FindObjectsOfType<Allie> ();
				foreach (Allie go in listAllie) {
					if (go.transform.position == transform.position) {
						myInventory.SwapItems (myAllie.ID, go.ID, Mathf.FloorToInt (SliderObjects.value), Mathf.FloorToInt (SliderObjects2.value));
						if (SliderObjects.value > myInventory.getAllieItems (myAllie.ID).Count - 1) {
							SliderObjects.value -= 1;
						} else if (SliderObjects2.value > myInventory.getAllieItems (go.ID).Count - 1) {
							SliderObjects2.value -= 1;
						}
						Objects2 = myInventory.getAllieItems (go.ID);
						break;
					}
				}
				Objects = myInventory.getAllieItems (myAllie.ID);

				ShowObjects ();
				ShowObjects2 ();
				isIntercObjectSelected = false;
				if (isObjectCursorLeft) {
					isObjectCursorLeft = false;
					SliderObjects.gameObject.SetActive (false);
					SliderObjects2.gameObject.SetActive (true);
				} else {
					isObjectCursorLeft = true;
					SliderObjects.gameObject.SetActive (true);
					SliderObjects2.gameObject.SetActive (false);
				}

			}
		} else if (Input.GetKeyDown (KeyCode.Return) && state == 7) {
			SeleccionarCursorEffect.Play ();
			myInventory.SwapItems (myAllie.ID, myAllie.ID, Mathf.FloorToInt (SliderObjects.value), 0);
			state = 5;
			panelObjects.SetActive (false);
			panelValuesObject.SetActive (false);

		
			if (transform.position.x - 1 == usefull [0].x + 0.5f) {
				if (!myAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("MovementA")) {
					myAllie.GetComponent<Animator> ().SetTrigger ("A");
				}
			}
			if (transform.position.x + 1 == usefull [0].x + 0.5f) {
				if (!myAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("MovementD")) {
					myAllie.GetComponent<Animator> ().SetTrigger ("D");
				}
			}
			if (transform.position.y - 1 == usefull [0].y + 0.5f) {
				if (!myAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("MovementS")) {
					myAllie.GetComponent<Animator> ().SetTrigger ("S");
				}
			}
			if (transform.position.y + 1 == usefull [0].y + 0.5f) {
				if (!myAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("MovementW")) {
					myAllie.GetComponent<Animator> ().SetTrigger ("W");
				}
			}

			transform.position = new Vector3 (usefull [0].x + 0.5f, usefull [0].y + 0.5f);
		} else if (Input.GetKeyDown (KeyCode.Return) && state == 8) { // BATALLA 
			SeleccionarCursorEffect.Play ();
			panelAttackValues.SetActive (false);
			AllieDoubleAttack.gameObject.SetActive (false);
			EnemyDoubleAttack.gameObject.SetActive (false);
			myMap.gameObject.GetComponent<SpriteRenderer> ().sortingOrder = 5;
			StartCoroutine (BattleAction ());
		} else if (Input.GetKeyDown (KeyCode.Return) && state == 9) {
			PanelEndTurn.SetActive (false);
			EndTurn ();
		}

		// Elimina Lineas y Transparencias cuando se pulsa en el estado 1.
		if (Input.GetKeyDown (KeyCode.RightShift) && state == 1) {
			myAllie.GetComponent<Animator> ().SetTrigger ("BackToIdle");
			BorraTransLinea ();
			counter = 0;
			state = 0;
			transform.position = myAllie.transform.position;
			panelObjective.SetActive (true);
			panelTerrainInfo.SetActive (true);
			TerrainType.text = myMap.GetTerrainValue (Mathf.FloorToInt (transform.position.x), Mathf.FloorToInt (transform.position.y));
			EsqValue.text = calculateTerrainEvasion (transform.position).ToString ();
			DefValue.text = calculateTerrainDefense (transform.position).ToString ();
			panelPersonajeInfo.SetActive (true);
			Nombre.text = myAllie.Name;
			VidaActual.text = myAllie.Hp.ToString ();
			VidaMaxima.text = myAllie.MaxHp.ToString ();
			SliderPersonajeInfoFiller.fillAmount = myAllie.Hp / (float)myAllie.MaxHp;
			Cara.sprite = myAllie.FaceImage;
		} else if (Input.GetKeyDown (KeyCode.RightShift) && state == 2) {
			state = 1;
			isOutOfArea = false;
			BorraTransLinea ();
			myAllie.transform.position = myAllieLastPos;
			panelOptions.SetActive (false);
		
			CampoDeVision (myAllie);
			AutogeneraciondeCaminos (cordX, cordY);
			if (!myAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Selected")) {
				myAllie.GetComponent<Animator> ().SetTrigger ("Selected");
			}
		} else if (Input.GetKeyDown (KeyCode.RightShift) && state == 3) {
			panelObjects.SetActive (false);
			panelValuesObject.SetActive (false);
			panelValuesObjectUsable.SetActive (false);
			panelValuesObjectWeapons.SetActive (false);
			panelOptions.SetActive (true);
			SliderOptions.value = 0;
			ifNeighbourHasEnemySpawnRedTransparency ();
			state = 2;
		} else if (Input.GetKeyDown (KeyCode.RightShift) && state == 4) {
			panelEqUs.SetActive (false);
			state = 3;
		} else if (Input.GetKeyDown (KeyCode.RightShift) && state == 5) {
			if (!isAttackAndNoInterc) {
				state = 2;
				panelOptions.SetActive (true);
				SliderOptions.value = 0;
				ifNeighbourHasEnemySpawnRedTransparency ();
				transform.position = myAllie.transform.position;
			} else {
				state = 7;
				panelOptions.SetActive (false);
				panelObjects.SetActive (true);
				panelValuesObject.SetActive (true);
				FaceMugshot.sprite = myAllie.FaceImage;
				SliderObjects.value = 0;
				SliderObjects.gameObject.SetActive (true);
				Objects = myInventory.getAllieWeapons (myAllie.ID);
				ShowObjects ();
				ShowObjectValues ();
				transform.position = myAllie.transform.position;
			}

		} else if (Input.GetKeyDown (KeyCode.RightShift) && state == 6) { 

			if (!isIntercObjectSelected) {
				state = 5;
				panelObjects.SetActive (false);
				panelObjects2.SetActive (false);
			} else {
				isIntercObjectSelected = false;
				if (isObjectCursorLeft) {
					isObjectCursorLeft = false;
					SliderObjects.gameObject.SetActive (false);
					SliderObjects2.gameObject.SetActive (true);
				} else {
					isObjectCursorLeft = true;
					SliderObjects.gameObject.SetActive (true);
					SliderObjects2.gameObject.SetActive (false);
				}
			}
		} else if (Input.GetKeyDown (KeyCode.RightShift) && state == 7) {
			state = 2;
			panelOptions.SetActive (true);
			SliderOptions.value = 0;
			transform.position = myAllie.transform.position;
			panelObjects.SetActive (false);
			panelValuesObject.SetActive (false);
			BorraTransLinea ();
			ifNeighbourHasEnemySpawnRedTransparency ();

		} else if (Input.GetKeyDown (KeyCode.RightShift) && state == 8) {
			panelAttackValues.SetActive (false);
			AllieDoubleAttack.gameObject.SetActive (false);
			EnemyDoubleAttack.gameObject.SetActive (false);
			state = 5;
		} else if (Input.GetKeyDown (KeyCode.RightShift) && state == 9) {
			PanelEndTurn.SetActive (false);
			if (myMap.GetTerrainValue (Mathf.FloorToInt (transform.position.x), Mathf.FloorToInt (transform.position.y)) != Types.Blocked) {
				panelTerrainInfo.SetActive (true);
			}
			panelPersonajeInfo.SetActive (true);
			panelObjective.SetActive (true);
			UpdatePersonajeInfo ();
			state = 0;
			GetComponent<SpriteRenderer> ().sortingOrder = 4;
		}

	
		// Mueve los valores del menu cuando se esta en el estado 2.
		if (Input.GetKeyDown (KeyCode.W) && state == 2) {
			
			if (SliderOptions.value != 0) {
				MoverCursorEffect.Play ();
			}
			if (SliderOptions.value == 1) {
				ifNeighbourHasEnemySpawnRedTransparency ();
			}
			SliderOptions.value -= 1;
		}

		if (Input.GetKeyDown (KeyCode.S) && state == 2) {
			MoverCursorEffect.Play ();
			SliderOptions.value += 1;
			BorraTransLinea ();
		}

		if (Input.GetKeyDown (KeyCode.W) && state == 3) { // hacerle que el slider no se mueva mas de lo que debe
			if (SliderObjects.value != 0) {
				MoverCursorEffect.Play ();
			}
			SliderObjects.value -= 1;
			ShowObjectValues ();
		}

		if (Input.GetKeyDown (KeyCode.S) && state == 3 && SliderObjects.value < Objects.Count - 1) {
			MoverCursorEffect.Play ();
			SliderObjects.value += 1;
			ShowObjectValues ();
		}

		if (Input.GetKeyDown (KeyCode.W) && state == 4) {
			if (SliderEqUs.value != 0) {
				MoverCursorEffect.Play ();
			}
			SliderEqUs.value -= 1;
		}

		if (Input.GetKeyDown (KeyCode.S) && state == 4) {
			MoverCursorEffect.Play ();
			SliderEqUs.value += 1;
		}

		// errores: de izquierda a derecha no se controla cuanto se puede bajar.


		if (Input.GetKeyDown (KeyCode.W) && state == 6) {
			
			if (isObjectCursorLeft) {
				if (SliderObjects.value != 0) {
					MoverCursorEffect.Play ();
				}
				SliderObjects.value -= 1;
			} else {
				if (SliderObjects2.value != 0) {
					MoverCursorEffect.Play ();
				}
				SliderObjects2.value -= 1;
			}
		}

		if (Input.GetKeyDown (KeyCode.S) && state == 6) {
			MoverCursorEffect.Play ();
			if (!isIntercObjectSelected) {
				if (isObjectCursorLeft && SliderObjects.value < Objects.Count - 1) {
					SliderObjects.value += 1;
				} else if (!isObjectCursorLeft && SliderObjects2.value < Objects2.Count - 1) {
					SliderObjects2.value += 1;
				}
			} else {
				if (isObjectCursorLeft && SliderObjects.value < Objects.Count) {
					SliderObjects.value += 1;
				} else if (!isObjectCursorLeft && SliderObjects2.value < Objects2.Count) {
					SliderObjects2.value += 1;
				}
			}

		}
		if (Input.GetKeyDown (KeyCode.A) && state == 6 && !isIntercObjectSelected) {
			MoverCursorEffect.Play ();
			if (!isObjectCursorLeft) {
				if (SliderObjects2.value < Objects.Count) {
					SliderObjects.value = SliderObjects2.value;
				} else {
					SliderObjects.value = Objects.Count - 1;
				}
			
				SliderObjects.gameObject.SetActive (true);
				SliderObjects2.gameObject.SetActive (false);
				isObjectCursorLeft = true;
			}

		}

		if (Input.GetKeyDown (KeyCode.D) && state == 6 && !isIntercObjectSelected) {
			MoverCursorEffect.Play ();
			if (isObjectCursorLeft) {
				if (SliderObjects.value < Objects2.Count) {
					SliderObjects2.value = SliderObjects.value;
				} else {
					SliderObjects2.value = Objects2.Count - 1;
				}
		
				SliderObjects.gameObject.SetActive (false);
				SliderObjects2.gameObject.SetActive (true);
				isObjectCursorLeft = false;
			}

		}
		if (Input.GetKeyDown (KeyCode.W) && state == 7) {
			if (SliderObjects.value != 0) {
				MoverCursorEffect.Play ();
			}
			SliderObjects.value -= 1;
			ShowObjectValues ();
			BorraTransLinea ();
			usefull.Clear ();

			int attackDistance = myInventory.getAllieWeapons (myAllie.ID) [Mathf.FloorToInt (SliderObjects.value)].AttackDistance;
			List<Spot> neighbors = new List<Spot> ();

			if (attackDistance == 1 || attackDistance == 12) {
				neighbors.AddRange (myMap.matrixScenary [cordX] [cordY].getNeighbors (myMap.matrixScenary, myMap.Right, myMap.Up));
				foreach (Spot a in myMap.matrixScenary[Mathf.FloorToInt(myAllie.transform.position.x)][Mathf.FloorToInt(myAllie.transform.position.y)].getNeighbors(myMap.matrixScenary,myMap.Right,myMap.Up)) {
					Instantiate (RedTransparency, new Vector3 (a.x + 0.5f, a.y + 0.5f, 0), Quaternion.identity);
				}
			}

			if (attackDistance == 2 || attackDistance == 12) {
				neighbors.AddRange (myMap.matrixScenary [cordX] [cordY].getFarNeighbors (myMap.matrixScenary, myMap.Right, myMap.Up));
				foreach (Spot a in myMap.matrixScenary[Mathf.FloorToInt(myAllie.transform.position.x)][Mathf.FloorToInt(myAllie.transform.position.y)].getFarNeighbors(myMap.matrixScenary,myMap.Right,myMap.Up)) {
					Instantiate (RedTransparency, new Vector3 (a.x + 0.5f, a.y + 0.5f, 0), Quaternion.identity);
				}
			}

			foreach (Spot a in neighbors) {
				if (myMap.matrixScenary [a.x] [a.y].personType == Types.Enemy) {
					usefull.Add (a);
				}
			}
		}
		if (Input.GetKeyDown (KeyCode.S) && state == 7 && SliderObjects.value < Objects.Count - 1) {
			MoverCursorEffect.Play ();
			SliderObjects.value += 1;
			ShowObjectValues ();
			BorraTransLinea ();
			usefull.Clear ();

			int attackDistance = myInventory.getAllieWeapons (myAllie.ID) [Mathf.FloorToInt (SliderObjects.value)].AttackDistance;
			List<Spot> neighbors = new List<Spot> ();

			if (attackDistance == 1 || attackDistance == 12) {
				neighbors.AddRange (myMap.matrixScenary [cordX] [cordY].getNeighbors (myMap.matrixScenary, myMap.Right, myMap.Up));
				foreach (Spot a in myMap.matrixScenary[Mathf.FloorToInt(myAllie.transform.position.x)][Mathf.FloorToInt(myAllie.transform.position.y)].getNeighbors(myMap.matrixScenary,myMap.Right,myMap.Up)) {
					Instantiate (RedTransparency, new Vector3 (a.x + 0.5f, a.y + 0.5f, 0), Quaternion.identity);
				}
			}

			if (attackDistance == 2 || attackDistance == 12) {
				neighbors.AddRange (myMap.matrixScenary [cordX] [cordY].getFarNeighbors (myMap.matrixScenary, myMap.Right, myMap.Up));
				foreach (Spot a in myMap.matrixScenary[Mathf.FloorToInt(myAllie.transform.position.x)][Mathf.FloorToInt(myAllie.transform.position.y)].getFarNeighbors(myMap.matrixScenary,myMap.Right,myMap.Up)) {
					Instantiate (RedTransparency, new Vector3 (a.x + 0.5f, a.y + 0.5f, 0), Quaternion.identity);
				}
			}

			foreach (Spot a in neighbors) {
				if (myMap.matrixScenary [a.x] [a.y].personType == Types.Enemy) {
					usefull.Add (a);
				}
			}
		}
	}

	//*******************
	// Comienzo subprogramas independientes
	//*******************


	private IEnumerator AnimationMovement () {
		state = -1;
		List<Lines> lineasForMovementAnimation;
		List<LineasTransform> salvaculos;


		salvaculos = new List<LineasTransform> ();

		lineasForMovementAnimation = new List<Lines> (Lines.FindObjectsOfType<Lines> ());
		lineasForMovementAnimation.Reverse ();

		for (int i = 0; i < lineasForMovementAnimation.Count; i++) {
			salvaculos.Add (new LineasTransform ());
			salvaculos [i].x = lineasForMovementAnimation [i].transform.position.x;
			salvaculos [i].y = lineasForMovementAnimation [i].transform.position.y;
		}
		float counter;
		float x = myAllie.transform.position.x;
		float y = myAllie.transform.position.y;

		for (int i = 1; i < salvaculos.Count; i++) {
			counter = 0;
			if (salvaculos [i].x > salvaculos [i - 1].x) {
				x += 1;

				if (!myAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("MovementD")) {
					myAllie.GetComponent<Animator> ().SetTrigger ("D");
				}

				while (counter < 1) {
					myAllie.transform.position += new Vector3 (0.1f, 0, 0);
					counter += 0.1f;
					yield return new WaitForSecondsRealtime (0.015f);
				}
			} else if (salvaculos [i].x < salvaculos [i - 1].x) {
				x -= 1;

				if (!myAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("MovementA")) {
					myAllie.GetComponent<Animator> ().SetTrigger ("A");
				}
					

				while (counter < 1) {
					myAllie.transform.position += new Vector3 (-0.1f, 0, 0);
					counter += 0.1f;
					yield return new WaitForSecondsRealtime (0.015f);
				}
			} else if (salvaculos [i].y > salvaculos [i - 1].y) {
				y += 1;

				if (!myAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("MovementW")) {
					myAllie.GetComponent<Animator> ().SetTrigger ("W");
				}

				while (counter < 1) {
					myAllie.transform.position += new Vector3 (0, 0.1f, 0);
					counter += 0.1f;
					yield return new WaitForSecondsRealtime (0.015f);
				}
			} else if (salvaculos [i].y < salvaculos [i - 1].y) {
				y -= 1;

				if (!myAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("MovementS")) {
					myAllie.GetComponent<Animator> ().SetTrigger ("S");
				}

				while (counter < 1) {
					myAllie.transform.position += new Vector3 (0, -0.1f, 0);
					counter += 0.1f;
					yield return new WaitForSecondsRealtime (0.015f);
				}
			}
		}
		myAllie.transform.position = new Vector3 (Mathf.FloorToInt (x) + 0.5f, Mathf.FloorToInt (y) + 0.5f, myAllie.transform.position.z);
		panelOptions.SetActive (true);
		SliderOptions.value = 0;
		ifNeighbourHasEnemySpawnRedTransparency ();
		state = 2;

		if (transform.position.x < ((myMap.Right + 1) / 2)) {
			panelOptions.transform.position = new Vector3 (myMap.Right - (0.25f * myMap.Right) + 0.5f, myMap.Up - (0.3f * myMap.Up) + 0.5f, 0);
		} else {
			panelOptions.transform.position = new Vector3 (0.01f * myMap.Right + 0.5f, myMap.Up - (0.3f * myMap.Up) + 0.5f, 0);
		}
	}

	public void UpdatePersonajeInfo () {
		Allie[] listAllie = FindObjectsOfType<Allie> ();
		bool isAllieOrEnemy = false;
		foreach (Allie go in listAllie) {
			if (go.transform.position.x == transform.position.x && go.transform.position.y == transform.position.y) {
				isAllieOrEnemy = true;
				panelPersonajeInfo.SetActive (true);
				Nombre.text = go.Name;
				VidaActual.text = go.Hp.ToString ();
				VidaMaxima.text = go.MaxHp.ToString ();
				SliderPersonajeInfoFiller.fillAmount = go.Hp / (float)go.MaxHp;
				Cara.sprite = go.FaceImage;
				break;
			}
		}
		Enemy[] listEnemy = FindObjectsOfType<Enemy> ();
		foreach (Enemy go in listEnemy) {
			if (go.transform.position.x == transform.position.x && go.transform.position.y == transform.position.y) {
				isAllieOrEnemy = true;
				panelPersonajeInfo.SetActive (true);
				Nombre.text = go.Name;
				VidaActual.text = go.Hp.ToString ();
				VidaMaxima.text = go.MaxHp.ToString ();
				SliderPersonajeInfoFiller.fillAmount = go.Hp / (float)go.MaxHp;
				Cara.sprite = go.FaceImage;
				break;
			} 
			if (!isAllieOrEnemy) {
				panelPersonajeInfo.SetActive (false);
			}
		}
	}

	private IEnumerator BattleAction () {
		AllyBackgroundSound.Pause ();
		BattleSound.Play ();
		state = -1;
		MarcoBatalla.SetActive (true);
		boneAllie.GetComponent<Animator> ().SetTrigger ("Idle" + myAllie.Name);
		nombreAliado.text = myAllie.Name;
		nombreEnemigo.text = myEnemy.Name;
		nombreArmaAliado.text = myInventory.getAllieWeapons (myAllie.ID) [0].Name;
		nombreArmaEnemigo.text = myEnemy.items [0].Name;
		vidaAliado.text = myAllie.Hp.ToString ();
		vidaEnemigo.text = myEnemy.Hp.ToString ();
		sliderVidaAliadoFiller.fillAmount = myAllie.Hp / (float)myAllie.MaxHp;
		sliderVidaEnemigoFiller.fillAmount = myEnemy.Hp / (float)myEnemy.MaxHp;
		imagenArmaAliado.sprite = Resources.Load<Sprite> (myInventory.getAllieWeapons (myAllie.ID) [0].Image);
		imagenArmaEnemigo.sprite = Resources.Load<Sprite> (myEnemy.items [0].Image);
		vGolAliado.text = (totalAllieAccuracyProbability * 100).ToString ();
		vGolEnemigo.text = (totalEnemyAccuracyProbability * 100).ToString ();
		vDanAliado.text = totalAllieDealDamage.ToString ();
		vDanEnemigo.text = totalEnemyDealDamage.ToString ();
		vCriAliado.text = (totalAllieCriticalProbability * 100).ToString ();
		vCriEnemigo.text = (totalEnemyCriticalProbability * 100).ToString ();
		platformAllie.sprite = myMap.matrixScenary [Mathf.FloorToInt (myAllie.transform.position.x)] [Mathf.FloorToInt (myAllie.transform.position.y)].platformSprite;
		platformEnemy.sprite = myMap.matrixScenary [Mathf.FloorToInt (myEnemy.transform.position.x)] [Mathf.FloorToInt (myEnemy.transform.position.y)].platformSprite;
		int distanceToAllie = Mathf.FloorToInt (Mathf.Abs (myAllie.transform.position.x - myEnemy.transform.position.x) +
		                      Mathf.Abs (myAllie.transform.position.y - myEnemy.transform.position.y));

		bool hasDodge = false;
		// First Allie Attack
		int criticalMultiplicator = 1;
		if (Random.value < totalAllieCriticalProbability) {
			criticalMultiplicator = 3;
		}

		if (criticalMultiplicator == 1) {
			boneAllie.GetComponent<Animator> ().SetTrigger (myAllie.Name + "Attack");
		} else {
			boneAllie.GetComponent<Animator> ().SetTrigger (myAllie.Name + "Crit");
		}

		if (Random.value < totalAllieAccuracyProbability || criticalMultiplicator > 1) {
			myEnemy.reciveDamage (totalAllieDealDamage * criticalMultiplicator);
			if (criticalMultiplicator == 1) {
				hitAllie.GetComponent<Animator> ().SetTrigger ("Hit");
			} else {
				hitAllie.GetComponent<Animator> ().SetTrigger ("CritHit");
			}
		} else {
			boneEnemy.GetComponent<Animator> ().SetTrigger (myEnemy.ID + "Dodge");
			hasDodge = true;
		}

		while (boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle" + myAllie.Name) || boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			yield return null;
		}

		if (hasDodge) {
			while (boneEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				yield return null;
			}
		}
		hasDodge = false;

		while (!boneEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			yield return null;
		}

		while (!boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle" + myAllie.Name)) {
			yield return null;
		}

	
		// Response of enemy
		if (myEnemy.AmIAlive () && (myEnemy.items [0].AttackDistance == distanceToAllie || myEnemy.items [0].AttackDistance == 12)) {
			criticalMultiplicator = 1;
			if (Random.value < totalEnemyCriticalProbability) {
				criticalMultiplicator = 3;
			}

			if (criticalMultiplicator == 1) {
				boneEnemy.GetComponent<Animator> ().SetTrigger (myEnemy.ID + "Attack");
			} else {
				boneEnemy.GetComponent<Animator> ().SetTrigger (myEnemy.ID + "Crit");
			}

			if (Random.value < totalEnemyAccuracyProbability || criticalMultiplicator > 1) {
				myAllie.reciveDamage (totalEnemyDealDamage * criticalMultiplicator);
				if (criticalMultiplicator == 1) {
					hitEnemy.GetComponent<Animator> ().SetTrigger ("Hit");
				} else {
					hitEnemy.GetComponent<Animator> ().SetTrigger ("CritHit");
				}
			} else {
				boneAllie.GetComponent<Animator> ().SetTrigger (myAllie.Name + "Dodge");
				hasDodge = true;
			}
			while (boneEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				yield return null;
			}

			if (hasDodge) {
				while (boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle" + myAllie.Name) || boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
					yield return null;
				}
			}
			hasDodge = false;


			while (!boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle" + myAllie.Name)) {
				yield return null;
			}

			while (!boneEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				yield return null;
			}
		}
		// Second Allie Attack
		if (allieVelocity - enemyVelocity >= 4 && myEnemy.AmIAlive () && myAllie.AmIAlive ()) {
			criticalMultiplicator = 1;
			if (Random.value < totalAllieCriticalProbability) {
				criticalMultiplicator = 3;
			}

			if (criticalMultiplicator == 1) {
				boneAllie.GetComponent<Animator> ().SetTrigger (myAllie.Name + "Attack");
			} else {
				boneAllie.GetComponent<Animator> ().SetTrigger (myAllie.Name + "Crit");
			}
				
			if (Random.value < totalAllieAccuracyProbability || criticalMultiplicator > 1) {
				myEnemy.reciveDamage (totalAllieDealDamage * criticalMultiplicator);
				if (criticalMultiplicator == 1) {
					hitAllie.GetComponent<Animator> ().SetTrigger ("Hit");
				} else {
					hitAllie.GetComponent<Animator> ().SetTrigger ("CritHit");
				}
			} else {
				boneEnemy.GetComponent<Animator> ().SetTrigger (myEnemy.ID + "Dodge");
				hasDodge = true;
			}
			while (boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle" + myAllie.Name) || boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				yield return null;
			}

			if (hasDodge) {
				while (boneEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
					yield return null;
				}
			}
			hasDodge = false;

			while (!boneEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				yield return null;
			}

			while (!boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle" + myAllie.Name)) {
				yield return null;
			}
		} 
		// Second Response of enemy
		if (allieVelocity - enemyVelocity <= -4 && myEnemy.AmIAlive () && myAllie.AmIAlive () && (myEnemy.items [0].AttackDistance == distanceToAllie || myEnemy.items [0].AttackDistance == 12)) {
			criticalMultiplicator = 1;
			if (Random.value < totalEnemyCriticalProbability) {
				criticalMultiplicator = 3;
			}

			if (criticalMultiplicator == 1) {
				boneEnemy.GetComponent<Animator> ().SetTrigger (myEnemy.ID + "Attack");
			} else {
				boneEnemy.GetComponent<Animator> ().SetTrigger (myEnemy.ID + "Crit");
			}

			if (Random.value < totalEnemyAccuracyProbability || criticalMultiplicator > 1) {
				myAllie.reciveDamage (totalEnemyDealDamage * criticalMultiplicator);
				if (criticalMultiplicator == 1) {
					hitEnemy.GetComponent<Animator> ().SetTrigger ("Hit");
				} else {
					hitEnemy.GetComponent<Animator> ().SetTrigger ("CritHit");
				}
			} else {
				boneAllie.GetComponent<Animator> ().SetTrigger (myAllie.Name + "Dodge");
				hasDodge = true;
			}

			while (boneEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				yield return null;
			}

			if (hasDodge) {
				while (boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle" + myAllie.Name) || boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
					yield return null;
				}
			}
			hasDodge = false;

			while (!boneAllie.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle" + myAllie.Name)) {
				yield return null;
			}

			while (!boneEnemy.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				yield return null;
			}
		}
		yield return new WaitForSecondsRealtime (0.8f);

		myMap.SetValue (Mathf.FloorToInt (myAllieLastPos.x), Mathf.FloorToInt (myAllieLastPos.y), null);
		if (myAllie.AmIAlive ()) {
			myAllie.setDisponibility (false);
			myMap.SetValue (Mathf.FloorToInt (myAllie.transform.position.x), Mathf.FloorToInt (myAllie.transform.position.y), Types.Allie);
			myAllie.GetComponent<Animator> ().SetTrigger ("BackToIdle");
		}
		BorraTransLinea ();
		state = 0;

		panelObjective.SetActive (true);
		panelTerrainInfo.SetActive (true);
		TerrainType.text = myMap.GetTerrainValue (Mathf.FloorToInt (transform.position.x), Mathf.FloorToInt (transform.position.y));
		EsqValue.text = calculateTerrainEvasion (transform.position).ToString ();
		DefValue.text = calculateTerrainDefense (transform.position).ToString ();

		if (myEnemy.AmIAlive ()) {
			panelPersonajeInfo.SetActive (true);
			Nombre.text = myEnemy.Name;
			VidaActual.text = myEnemy.Hp.ToString ();
			VidaMaxima.text = myEnemy.MaxHp.ToString ();
			SliderPersonajeInfoFiller.fillAmount = myEnemy.Hp / (float)myEnemy.MaxHp;
			Cara.sprite = myEnemy.FaceImage;
		}
		myMap.gameObject.GetComponent<SpriteRenderer> ().sortingOrder = 0;
		MarcoBatalla.SetActive (false);

		if (AllieWin ()) {
			PanelVictoriaDerrota.SetActive (true);
			VictoriaDerrota.text = "¡VICTORIA!";
			AllyBackgroundSound.Stop ();
			EnemyBackgroundSound.Stop ();
			VictorySound.Play ();
			state = -1;
			Invoke ("BackToTittle", 9);
		} else if (EnemyWin ()) {
			PanelVictoriaDerrota.SetActive (true);
			VictoriaDerrota.text = "¡DERROTA!";
			AllyBackgroundSound.Stop ();
			EnemyBackgroundSound.Stop ();
			DefeatSound.Play ();
			state = -1;
			Invoke ("BackToTittle", 10);
		} else if (checkEndTurn ()) {
			yield return new WaitForSecondsRealtime (0.5f);
			EndTurn ();
		}
		boneAllie.GetComponent<Animator> ().SetTrigger ("BackToIdle");
		AllyBackgroundSound.UnPause ();
		BattleSound.Stop ();
	}

	private void BackToTittle () {
		SceneManager.LoadScene ("Titulo");
	}

	public void UpdateEnemyHp () {
		if (myEnemy.Hp < 0) {
			myEnemy.Hp = 0;
		}

		if (myEnemy.Hp.ToString () != vidaEnemigo.text) {
			hitAllie.GetComponent<Animator> ().SetTrigger ("Execute");
		}

		vidaEnemigo.text = myEnemy.Hp.ToString ();
		sliderVidaEnemigoFiller.fillAmount = myEnemy.Hp / (float)myEnemy.MaxHp;
	}

	public void UpdateAllieHp () {
		if (myAllie.Hp < 0) {
			myAllie.Hp = 0;
		}
		if (myAllie.Hp.ToString () != vidaAliado.text) {
			hitEnemy.GetComponent<Animator> ().SetTrigger ("Execute");
		}
		vidaAliado.text = myAllie.Hp.ToString ();
		sliderVidaAliadoFiller.fillAmount = myAllie.Hp / (float)myAllie.MaxHp;
	}

	public void setState (int i) {
		state = i;
	}

	public void setEnemy (Enemy i) {
		myEnemy = i;
	}

	public void setAllie (Allie i) {
		myAllie = i;
	}

	private int calculateAttackDistance () {
		int attackDistance = 0;
		foreach (Item i in myInventory.getAllieWeapons (myAllie.ID)) {
			attackDistance += i.AttackDistance;
		}

		if (attackDistance == myInventory.getAllieWeapons (myAllie.ID).Count) {
			attackDistance = 1;
		} else if (attackDistance == 2 * myInventory.getAllieWeapons (myAllie.ID).Count) {
			attackDistance = 2;
		} else {
			attackDistance = 12;
		}
		return attackDistance;
	}

	private bool checkEndTurn () {
		Allie[] allies = FindObjectsOfType<Allie> ();
		foreach (Allie a in allies) {
			if (a.getDisponibility ()) {
				return false;
			}
		}
		return true;
	}

	private void EndTurn () {
		myMap.EndTurnAllie ();
		BorraTransLinea ();
		panelOptions.SetActive (false);
		panelTerrainInfo.SetActive (false);
		panelPersonajeInfo.SetActive (false);
		panelObjective.SetActive (false);
		GetComponent<SpriteRenderer> ().sortingOrder = -3;
		state = -1;

		myEnemyTurn.Development ();
		counter = 0;
	}

	private void ifNeighbourHasEnemySpawnRedTransparency () {
		int myAllieX = Mathf.FloorToInt (myAllie.transform.position.x);
		int myAllieY = Mathf.FloorToInt (myAllie.transform.position.y);

		int attackDistance = calculateAttackDistance ();

		if (attackDistance == 1 || attackDistance == 12) {
			if (myAllieX + 1 < myMap.Right && myMap.GetValue (myAllieX + 1, myAllieY) == Types.Enemy) {
				InstantiateRedToNeighbours ();
			} else if (myAllieX - 1 > 0 && myMap.GetValue (myAllieX - 1, myAllieY) == Types.Enemy) {
				InstantiateRedToNeighbours ();
			} else if (myAllieY + 1 < myMap.Up && myMap.GetValue (myAllieX, myAllieY + 1) == Types.Enemy) {
				InstantiateRedToNeighbours ();
			} else if (myAllieY - 1 > 0 && myMap.GetValue (myAllieX, myAllieY - 1) == Types.Enemy) {
				InstantiateRedToNeighbours ();
			}
		}

		if (attackDistance == 2 || attackDistance == 12) {
			if (myAllieX + 2 < myMap.Right && myMap.GetValue (myAllieX + 2, myAllieY) == Types.Enemy) {
				InstantiateRedToFarNeighbours ();
			} else if (myAllieX - 2 > 0 && myMap.GetValue (myAllieX - 2, myAllieY) == Types.Enemy) {
				InstantiateRedToFarNeighbours ();
			} else if (myAllieY + 2 < myMap.Up && myMap.GetValue (myAllieX, myAllieY + 2) == Types.Enemy) {
				InstantiateRedToFarNeighbours ();
			} else if (myAllieY - 2 > 0 && myMap.GetValue (myAllieX, myAllieY - 2) == Types.Enemy) {
				InstantiateRedToFarNeighbours ();
			} else if (myAllieX + 1 < myMap.Right && myAllieY + 1 < myMap.Up && myMap.GetValue (myAllieX + 1, myAllieY + 1) == Types.Enemy) {
				InstantiateRedToFarNeighbours ();
			} else if (myAllieX - 1 > 0 && myAllieY - 1 > 0 && myMap.GetValue (myAllieX - 1, myAllieY - 1) == Types.Enemy) {
				InstantiateRedToFarNeighbours ();
			} else if (myAllieY + 1 < myMap.Up && myAllieX - 1 > 0 && myMap.GetValue (myAllieX - 1, myAllieY + 1) == Types.Enemy) {
				InstantiateRedToFarNeighbours ();
			} else if (myAllieY - 1 > 0 && myAllieX + 1 < myMap.Right && myMap.GetValue (myAllieX + 1, myAllieY - 1) == Types.Enemy) {
				InstantiateRedToFarNeighbours ();
			}
		}
	}

	private void InstantiateRedToNeighbours () {
		int myAllieX = Mathf.FloorToInt (myAllie.transform.position.x);
		int myAllieY = Mathf.FloorToInt (myAllie.transform.position.y);

		foreach (Spot a in myMap.matrixScenary[myAllieX][myAllieY].getNeighbors(myMap.matrixScenary,myMap.Right,myMap.Up)) {
			Instantiate (RedTransparency, new Vector3 (a.x + 0.5f, a.y + 0.5f, 0), Quaternion.identity);
		}
	}

	private void InstantiateRedToFarNeighbours () {
		int myAllieX = Mathf.FloorToInt (myAllie.transform.position.x);
		int myAllieY = Mathf.FloorToInt (myAllie.transform.position.y);

		foreach (Spot a in myMap.matrixScenary[myAllieX][myAllieY].getFarNeighbors(myMap.matrixScenary,myMap.Right,myMap.Up)) {
			Instantiate (RedTransparency, new Vector3 (a.x + 0.5f, a.y + 0.5f, 0), Quaternion.identity);
		}
	}

	private void ShowAttackValues () {

		if (myAllie.transform.position.x < ((myMap.Right + 1) / 2)) {
			panelAttackValues.transform.position = new Vector3 (myMap.Right - (0.42f * myMap.Right) + 0.5f, panelAttackValues.transform.position.y, 0);
		} else {
			panelAttackValues.transform.position = new Vector3 (0.0005f * myMap.Right + 0.5f, panelAttackValues.transform.position.y, 0);
		}
		panelAttackValues.SetActive (true);
		AllieName.text = myAllie.Name;
		EnemyName.text = myEnemy.Name;
		WeaponEnemyName.text = myEnemy.items [0].Name;
		ImageAllieWeapon.sprite = Resources.Load<Sprite> (myInventory.getAllieWeapons (myAllie.ID) [0].Image);
		ImageEnemyWeapon.sprite = Resources.Load<Sprite> (myEnemy.items [0].Image);
		AllieHp.text = myAllie.Hp.ToString ();
		EnemyHp.text = myEnemy.Hp.ToString ();
		AllieDmg.text = totalAllieDealDamage.ToString ();
		AllieAccu.text = (totalAllieAccuracyProbability * 100).ToString ();
		AllieCrit.text = (totalAllieCriticalProbability * 100).ToString ();

		if (allieVelocity - enemyVelocity >= 4) {
			AllieDoubleAttack.gameObject.SetActive (true);
			EnemyDoubleAttack.gameObject.SetActive (false);
		} else if (allieVelocity - enemyVelocity <= -4) {
			AllieDoubleAttack.gameObject.SetActive (false);
			EnemyDoubleAttack.gameObject.SetActive (true);
		}

		int distanceToAllie = Mathf.FloorToInt (Mathf.Abs (myAllie.transform.position.x - myEnemy.transform.position.x) +
		                      Mathf.Abs (myAllie.transform.position.y - myEnemy.transform.position.y));

		if (myEnemy.items [0].AttackDistance == 12 || myEnemy.items [0].AttackDistance == distanceToAllie) {
			EnemyDmg.text = totalEnemyDealDamage.ToString ();
			EnemyAccu.text = (totalEnemyAccuracyProbability * 100).ToString ();
			EnemyCrit.text = (totalEnemyCriticalProbability * 100).ToString ();
		} else {
			EnemyDoubleAttack.gameObject.SetActive (false);
			EnemyDmg.text = " --";
			EnemyAccu.text = " --";
			EnemyCrit.text = " --";
		}

	}

	private int calculateWeaponTriangleAccuracy () {
		int ans = 0;
		string allieWeapon = myInventory.getAllieItems (myAllie.ID) [0].Description;
		string enemyWeapon = myEnemy.items [0].Description; // posible bug porque el item no sea un arma.

		if ((allieWeapon == Types.Sword && enemyWeapon == Types.Axe) || (allieWeapon == Types.Spear && enemyWeapon == Types.Sword) || (allieWeapon == Types.Axe && enemyWeapon == Types.Spear)) {
			ans = 15;
		} else if ((allieWeapon == Types.Sword && enemyWeapon == Types.Spear) || (allieWeapon == Types.Spear && enemyWeapon == Types.Axe) || (allieWeapon == Types.Axe && enemyWeapon == Types.Sword)) {
			ans = -15;
		}

	
		return ans;
	}

	private void calculateAllieDealDamage (int weaponTriangle) {
		int allieDamage = myAllie.Strength + myInventory.getAllieWeapons (myAllie.ID) [0].Attack + (weaponTriangle / 10); // faltaria poner armas efectivas
		int enemyDefence = myEnemy.Defence + calculateTerrainDefense (transform.position);
		totalAllieDealDamage = (allieDamage - enemyDefence);
		if (totalAllieDealDamage < 0)
			totalAllieDealDamage = 0;
	}

	private bool isAllieAttackSucced (int weaponTriangle) {
		bool isSucced = false;
		float probability = (myAllie.Skill * 2) + (myAllie.Luck / 2) + weaponTriangle + myInventory.getAllieWeapons (myAllie.ID) [0].Accuracy; // falta rango S
		float enemyEvasionProbability = (myEnemy.Speed * 2) + myEnemy.Luck + calculateTerrainEvasion (transform.position);
		totalAllieAccuracyProbability = (probability - enemyEvasionProbability) / 100;
		if (totalAllieAccuracyProbability < 0) {
			totalAllieAccuracyProbability = 0;
		} else if (totalAllieAccuracyProbability > 1) {
			totalAllieAccuracyProbability = 1;
		}

		if (Random.value < totalAllieAccuracyProbability) {
			isSucced = true;
		}
	

		return isSucced;
	}

	private int calculateAllieCriticalProbability () {
		int ans = 1;
		float probability = (myAllie.Skill / 2) + myInventory.getAllieWeapons (myAllie.ID) [0].Critical;
		totalAllieCriticalProbability = (probability - myEnemy.Luck) / 100; // faltaria el rango S y el bono de clase
		if (totalAllieCriticalProbability < 0) {
			totalAllieCriticalProbability = 0;
		} else if (totalAllieCriticalProbability > 1) {
			totalAllieCriticalProbability = 1;
		}
		if (Random.value < totalAllieCriticalProbability) {
			ans = 3;
		}

		return ans;
	}

	private void calculateAllieVelocity () {
		int allieWeaponWeight = myInventory.getAllieWeapons (myAllie.ID) [0].Weight - myAllie.Complexity;
		if (allieWeaponWeight < 0) {
			allieWeaponWeight = 0;
		}
		allieVelocity = myAllie.Speed - allieWeaponWeight;
	}

	private void calculateEnemyDealDamage (int weaponTriangle) {
		weaponTriangle = weaponTriangle * (-1);
		int enemyDamage = myEnemy.Strength + myEnemy.items [0].Attack + (weaponTriangle / 10); // faltaria poner armas efectivas
		int allieDefence = myAllie.Defence + calculateTerrainDefense (transform.position);
		totalEnemyDealDamage = (enemyDamage - allieDefence);
		if (totalEnemyDealDamage < 0)
			totalEnemyDealDamage = 0;
	}

	private bool isEnemyAttackSucced (int weaponTriangle) {
		weaponTriangle = weaponTriangle * (-1);
		bool isSucced = false;
		float probability = (myEnemy.Skill * 2) + (myEnemy.Luck / 2) + weaponTriangle + myEnemy.items [0].Accuracy; // falta rango S
		float allieEvasionProbability = (myAllie.Speed * 2) + myAllie.Luck + calculateTerrainEvasion (myAllie.transform.position); // falta el terreno
		totalEnemyAccuracyProbability = (probability - allieEvasionProbability) / 100;
		if (totalEnemyAccuracyProbability < 0) {
			totalEnemyAccuracyProbability = 0;
		} else if (totalEnemyAccuracyProbability > 1) {
			totalEnemyAccuracyProbability = 1;
		}
		if (Random.value < totalEnemyAccuracyProbability) {
			isSucced = true;
		}

		return isSucced;
	}

	private int calculateEnemyCriticalProbability () {
		int ans = 1;
		float probability = (myEnemy.Skill / 2) + myEnemy.items [0].Critical;
		totalEnemyCriticalProbability = (probability - myAllie.Luck) / 100; // faltaria el rango S y el bono de clase
		if (totalEnemyCriticalProbability < 0) {
			totalEnemyCriticalProbability = 0;
		} else if (totalEnemyCriticalProbability > 1) {
			totalEnemyCriticalProbability = 1;
		}
		if (Random.value < totalEnemyCriticalProbability) {
			ans = 3;
		}
		return ans;
	}

	private void calculateEnemyVelocity () {
		int enemyWeaponWeight = myEnemy.items [0].Weight - myEnemy.Complexity;
		if (enemyWeaponWeight < 0) {
			enemyWeaponWeight = 0;
		}
		enemyVelocity = myEnemy.Speed - enemyWeaponWeight;
	}

	private int calculateTerrainEvasion (Vector3 position) {
		int ans = 0;

		if (myMap.GetTerrainValue (Mathf.FloorToInt (position.x), Mathf.FloorToInt (position.y)) == Types.Forest) {
			ans = 20;
		} else if (myMap.GetTerrainValue (Mathf.FloorToInt (position.x), Mathf.FloorToInt (position.y)) == Types.Mountain) {
			ans = 30;
		} else if (myMap.GetTerrainValue (Mathf.FloorToInt (position.x), Mathf.FloorToInt (position.y)) == Types.Blocked) {
			panelTerrainInfo.SetActive (false);
		}
		return ans;
	}

	private int calculateTerrainDefense (Vector3 position) {
		int ans = 0;

		if (myMap.GetTerrainValue (Mathf.FloorToInt (position.x), Mathf.FloorToInt (position.y)) == Types.Forest) {
			ans = 1;
		} else if (myMap.GetTerrainValue (Mathf.FloorToInt (position.x), Mathf.FloorToInt (position.y)) == Types.Mountain) {
			ans = 1;
		} else if (myMap.GetTerrainValue (Mathf.FloorToInt (position.x), Mathf.FloorToInt (position.y)) == Types.Blocked) {
			panelTerrainInfo.SetActive (false);
		}
		return ans;
	}

	public bool AllieWin () {
		Enemy[] enemies = FindObjectsOfType<Enemy> ();
		return enemies.Length == 0;
	}

	public bool EnemyWin () {
		Allie[] allies = FindObjectsOfType<Allie> ();
		return allies.Length == 0;
	}

	private void ShowObjects () {
		
		panelObjectPanel.GetComponent<RectTransform> ().offsetMin = new Vector2 (panelObjectPanel.GetComponent<RectTransform> ().offsetMin.x, -11.5f);
		SliderOptions.value = 0;
		Color alpha = new Color ();
		alpha.r = 1;
		alpha.g = 1;
		alpha.b = 1;
		// se coloquen en cada sitio sus necesarios


		if (Objects.Count >= 1) {
			alpha.a = 1;
			Image1.color = alpha;
			Image1.sprite = Resources.Load<Sprite> (Objects [0].Image);
			Name1.text = Objects [0].Name.ToString ();
			Quantity1.text = Objects [0].Quantity.ToString ();
			if (state != 6)
				panelObjectPanel.GetComponent<RectTransform> ().offsetMin = new Vector2 (panelObjectPanel.GetComponent<RectTransform> ().offsetMin.x, 200);
		} else {
					
			alpha.a = 0;
			Image1.color = alpha;
			Name1.text = "";
			Quantity1.text = "";
		}
		if (Objects.Count >= 2) {
			alpha.a = 1;
			Image2.color = alpha;
			Image2.sprite = Resources.Load<Sprite> (Objects [1].Image);
			Name2.text = Objects [1].Name.ToString ();
			Quantity2.text = Objects [1].Quantity.ToString ();
			if (state != 6)
				panelObjectPanel.GetComponent<RectTransform> ().offsetMin = new Vector2 (panelObjectPanel.GetComponent<RectTransform> ().offsetMin.x, 155);
		} else {
			alpha.a = 0;
			Image2.color = alpha;
			Name2.text = "";
			Quantity2.text = "";
		}
		if (Objects.Count >= 3) {
			alpha.a = 1;
			Image3.color = alpha;
			Image3.sprite = Resources.Load<Sprite> (Objects [2].Image);
			Name3.text = Objects [2].Name.ToString ();
			Quantity3.text = Objects [2].Quantity.ToString ();
			if (state != 6)
				panelObjectPanel.GetComponent<RectTransform> ().offsetMin = new Vector2 (panelObjectPanel.GetComponent<RectTransform> ().offsetMin.x, 110);
		} else {
			alpha.a = 0;
			Image3.color = alpha;
			Name3.text = "";
			Quantity3.text = "";
		}
		if (Objects.Count >= 4) {
			alpha.a = 1;
			Image4.color = alpha;
			Image4.sprite = Resources.Load<Sprite> (Objects [3].Image);
			Name4.text = Objects [3].Name.ToString ();
			Quantity4.text = Objects [3].Quantity.ToString ();
			if (state != 6)
				panelObjectPanel.GetComponent<RectTransform> ().offsetMin = new Vector2 (panelObjectPanel.GetComponent<RectTransform> ().offsetMin.x, 65);
		} else {
			alpha.a = 0;
			Image4.color = alpha;
			Name4.text = "";
			Quantity4.text = "";
		}
		if (Objects.Count >= 5) {
			alpha.a = 1;
			Image5.color = alpha;
			Image5.sprite = Resources.Load<Sprite> (Objects [4].Image);
			Name5.text = Objects [4].Name.ToString ();
			Quantity5.text = Objects [4].Quantity.ToString ();
			if (state != 6)
				panelObjectPanel.GetComponent<RectTransform> ().offsetMin = new Vector2 (panelObjectPanel.GetComponent<RectTransform> ().offsetMin.x, 0);
		} else {
			alpha.a = 0;
			Image5.color = alpha;
			Name5.text = "";
			Quantity5.text = "";
		}

	}

	private void ShowObjects2 () {
		Color alpha = new Color ();
		alpha.r = 1;
		alpha.g = 1;
		alpha.b = 1;
		// se coloquen en cada sitio sus necesarios


		if (Objects2.Count >= 1) {
			alpha.a = 1;
			Image12.color = alpha;
			Image12.sprite = Resources.Load<Sprite> (Objects2 [0].Image);
			Name12.text = Objects2 [0].Name.ToString ();
			Quantity12.text = Objects2 [0].Quantity.ToString ();

		} else {
					
			alpha.a = 0;
			Image12.color = alpha;
			Name12.text = "";
			Quantity12.text = "";
		}
		if (Objects2.Count >= 2) {
			alpha.a = 1;
			Image22.color = alpha;
			Image22.sprite = Resources.Load<Sprite> (Objects2 [1].Image);
			Name22.text = Objects2 [1].Name.ToString ();
			Quantity22.text = Objects2 [1].Quantity.ToString ();

		} else {
			alpha.a = 0;
			Image22.color = alpha;
			Name22.text = "";
			Quantity22.text = "";
		}
		if (Objects2.Count >= 3) {
			alpha.a = 1;
			Image32.color = alpha;
			Image32.sprite = Resources.Load<Sprite> (Objects2 [2].Image);
			Name32.text = Objects2 [2].Name.ToString ();
			Quantity32.text = Objects2 [2].Quantity.ToString ();

		} else {
			alpha.a = 0;
			Image32.color = alpha;
			Name32.text = "";
			Quantity32.text = "";
		}
		if (Objects2.Count >= 4) {
			alpha.a = 1;
			Image42.color = alpha;
			Image42.sprite = Resources.Load<Sprite> (Objects2 [3].Image);
			Name42.text = Objects2 [3].Name.ToString ();
			Quantity42.text = Objects2 [3].Quantity.ToString ();

		} else {
			alpha.a = 0;
			Image42.color = alpha;
			Name42.text = "";
			Quantity42.text = "";
		}
		if (Objects2.Count >= 5) {
			alpha.a = 1;
			Image52.color = alpha;
			Image52.sprite = Resources.Load<Sprite> (Objects2 [4].Image);
			Name52.text = Objects2 [4].Name.ToString ();
			Quantity52.text = Objects2 [4].Quantity.ToString ();

		} else {
			alpha.a = 0;
			Image52.color = alpha;
			Name52.text = "";
			Quantity52.text = "";
		}

	}

	private void ShowObjectValues () {
		int index = Mathf.FloorToInt (SliderObjects.value);
		if (Objects [index].Type == "Weapon") {
			panelValuesObjectUsable.SetActive (false);
			panelValuesObjectWeapons.SetActive (true);
			QuantityAttack.text = Objects [index].Attack.ToString ();
			QuantityCritical.text = Objects [index].Critical.ToString ();
			QuantityWeight.text = (Objects [index].Weight + myAllie.Speed).ToString ();
			QuantityHit.text = Objects [index].Accuracy.ToString ();
		} else {
			panelValuesObjectUsable.SetActive (true);
			panelValuesObjectWeapons.SetActive (true);
			Description.text = Objects [index].Description;
		}
	}


	// Comprueba si se pulsan dos teclas a la vez.
	private bool CheckTwoAtaTime () {
		bool ok = false;

		if (state < 2) { 
			if (Input.GetKeyDown (KeyCode.A) && Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.A) && Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.D) && Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.D) && Input.GetKeyDown (KeyCode.S)) {
				ok = true;
			} else {
				ok = false;
			}
		}
		return ok;
	}

	private void JustASec () { // PARA MOV CURSOR

		
	}
	// Elimina todas las lineas y Transparencias.
	private void BorraTransLinea () {
		
		Lines[] listLines = FindObjectsOfType<Lines> ();
		foreach (Lines g in listLines) {
			Destroy (g.gameObject);
		}
		Transparency[] listTransparency = FindObjectsOfType<Transparency> ();
		foreach (Transparency g in listTransparency) {
			Destroy (g.gameObject);
		}


	}


	private void EnsureMap () {
		transform.position = new Vector3 (Mathf.Clamp (transform.position.x, 0.5f, myMap.Right + 0.5f), Mathf.Clamp (transform.position.y, 0.5f, myMap.Up + 0.5f), 0);
	}

	// Genera el campo en el que un aliado se puede mover. que genere solo hasta los limites del mapa y no mas.
	private void CampoDeVision (Allie myAllie) {
		
		
		int myAllieX = Mathf.FloorToInt (myAllie.transform.position.x);
		int myAllieY = Mathf.FloorToInt (myAllie.transform.position.y);
		List<Spot> graph = new List<Spot> ();

		for (int i = 0; i <= myAllie.movement; i++) { // arriba derecha
			for (int j = 0; j <= myAllie.movement - i; j++) {
				if (myAllieX + i > myMap.Right || myAllieY + j > myMap.Up) {
					break;
				}
				if (myMap.GetValueScenary (myAllieX + i, myAllieY + j) != Types.Ocupaid) {
					graph.Add (myMap.matrixScenary [myAllieX + i] [myAllieY + j]);
				}
			}
		}

		for (int i = 1; i <= myAllie.movement; i++) { // abjo derecha
			for (int j = 1; j <= myAllie.movement - i; j++) {
				if (myAllieX + i > myMap.Right || myAllieY - j < 0) {
					break;
				}
				if (myMap.GetValueScenary (myAllieX + i, myAllieY - j) != Types.Ocupaid) {
					graph.Add (myMap.matrixScenary [myAllieX + i] [myAllieY - j]);
				} 

			}
		}

		for (int i = 1; i <= myAllie.movement; i++) { // arriba izquierda
			for (int j = 1; j <= myAllie.movement - i; j++) {

				if (myAllieX - i < 0 || myAllieY + j > myMap.Up) {

					break;
				}
				if (myMap.GetValueScenary (myAllieX - i, myAllieY + j) != Types.Ocupaid) {
					graph.Add (myMap.matrixScenary [myAllieX - i] [myAllieY + j]);
				} 
			}
		}

		for (int i = 0; i <= myAllie.movement; i++) { // abajo izquierda
			for (int j = 0; j <= myAllie.movement - i; j++) {
				if (myAllieX - i < 0 || myAllieY - j < 0) {
					break;
				}
				if (i != 0 || j != 0) {
					if (myMap.GetValueScenary (myAllieX - i, myAllieY - j) != Types.Ocupaid) {
						graph.Add (myMap.matrixScenary [myAllieX - i] [myAllieY - j]);
					} 
				}
			}
		}

		List<Spot> blueTrans = myMap.Dijkstra (graph, myMap.matrixScenary [myAllieX] [myAllieY], myAllie.movement);
		HashSet<Spot> redTrans = new HashSet<Spot> ();

		foreach (Spot a in blueTrans) {
			Instantiate (BlueTransparency, new Vector3 (a.x + 0.5f, a.y + 0.5f, 0), Quaternion.identity);

			if (myMap.GetValue (a.x, a.y) != Types.Allie) {
				int attackDistance = calculateAttackDistance ();

				if (attackDistance == 1 || attackDistance == 12) {
					foreach (Spot neigh in a.getNeighbors(myMap.matrixScenary,myMap.Right,myMap.Up)) {
						if (!blueTrans.Contains (neigh)) {
							redTrans.Add (neigh);
						}
					}
				}
				if (attackDistance == 2 || attackDistance == 12) {
					foreach (Spot neigh in a.getFarNeighbors(myMap.matrixScenary,myMap.Right,myMap.Up)) {
						if (!blueTrans.Contains (neigh)) {
							redTrans.Add (neigh);
						}
					}
				}
			}
		}

		foreach (Spot a in redTrans) {
			Instantiate (RedTransparency, new Vector3 (a.x + 0.5f, a.y + 0.5f, 0), Quaternion.identity);
		}
	}

	//*******************
	// Conjunto de Autogeneracion del camino
	//*******************

	private void MueveA (Transform myTransform) {
		myTransform.position += new Vector3 (-1, 0, 0);
		Lines[] listLines = FindObjectsOfType<Lines> ();
		foreach (Lines g in listLines) {
			if (g.transform.position == lastPos) {
				Destroy (g.gameObject);
				break;
			}
		}

		if (lastQuat == Quaternion.Euler (0, 0, 90) && lastPos != myAllie.transform.position) {			
			Instantiate (Line, lastPos, Quaternion.Euler (0, 0, 90));	
		} else {
			bool wasSomethingPut = false;
			Lines[] listLines2 = FindObjectsOfType<Lines> ();
			foreach (Lines g in listLines2) {
				if (g.transform.position.x == myTransform.position.x + 1 && g.transform.position.y == myTransform.position.y + 1 && lastPos != myAllie.transform.position) {
					Instantiate (IzArr, lastPos, Quaternion.Euler (0, 0, 0));
					wasSomethingPut = true;
					break;
				} else if (g.transform.position.x == myTransform.position.x + 1 && g.transform.position.y == myTransform.position.y - 1 && lastPos != myAllie.transform.position) {
					Instantiate (IzAba, lastPos, Quaternion.Euler (0, 0, 0));
					wasSomethingPut = true;
					break;
				} 
			}
			if (!wasSomethingPut) {
				Instantiate (tester, lastPos, Quaternion.Euler (0, 0, 0));
			}
		}
		Instantiate (EndLineArr, new Vector3 (myTransform.position.x, myTransform.position.y, 0), Quaternion.Euler (0, 0, 90));
		lastPos = myTransform.position;
		lastQuat = Quaternion.Euler (0, 0, 90);
	}

	private void MueveW (Transform myTransform) {

		myTransform.position += new Vector3 (0, 1, 0);

		Lines[] listLines = FindObjectsOfType<Lines> ();
		foreach (Lines g in listLines) {
			if (g.transform.position == lastPos) {
				Destroy (g.gameObject);
				break;
			}
		}

		if (lastQuat == Quaternion.Euler (0, 0, 0) && lastPos != myAllie.transform.position) {
			Instantiate (Line, lastPos, Quaternion.Euler (0, 0, 0));	
		} else {
			bool wasSomethingPut = false;
			Lines[] listLines2 = FindObjectsOfType<Lines> ();
			foreach (Lines g in listLines2) {
				if (g.transform.position.x == myTransform.position.x - 1 && g.transform.position.y == myTransform.position.y - 1 && lastPos != myAllie.transform.position) {
					Instantiate (IzArr, lastPos, Quaternion.Euler (0, 0, 0));
					wasSomethingPut = true;
					break;
				} else if (g.transform.position.x == myTransform.position.x + 1 && g.transform.position.y == myTransform.position.y - 1 && lastPos != myAllie.transform.position) {
					Instantiate (DerArr, lastPos, Quaternion.Euler (0, 0, 0));
					wasSomethingPut = true;
					break;	
				} 
			}
			if (!wasSomethingPut) {
				Instantiate (tester, lastPos, Quaternion.Euler (0, 0, 0));	
			}

		}
		Instantiate (EndLineArr, new Vector3 (myTransform.position.x, myTransform.position.y, 0), Quaternion.Euler (0, 0, 0));
		lastPos = myTransform.position;
		lastQuat = Quaternion.Euler (0, 0, 0);

	}

	private void MueveD (Transform myTransform) {

		myTransform.position += new Vector3 (1, 0, 0);

		Lines[] listLines = FindObjectsOfType<Lines> ();
		foreach (Lines g in listLines) {
			if (g.transform.position == lastPos) {
				Destroy (g.gameObject);
				break;
			}
		}

		if (lastQuat == Quaternion.Euler (0, 0, 90) && lastPos != myAllie.transform.position) {
			Instantiate (Line, lastPos, Quaternion.Euler (0, 0, 90));	
		} else {
			bool wasSomethingPut = false;
			Lines[] listLines2 = FindObjectsOfType<Lines> ();
			foreach (Lines g in listLines2) {
				if (g.transform.position.x == myTransform.position.x - 1 && g.transform.position.y == myTransform.position.y + 1 && lastPos != myAllie.transform.position) {
					Instantiate (DerArr, lastPos, Quaternion.Euler (0, 0, 0));
					wasSomethingPut = true;
					break;
				} else if (g.transform.position.x == myTransform.position.x - 1 && g.transform.position.y == myTransform.position.y - 1 && lastPos != myAllie.transform.position) {
					Instantiate (DerAba, lastPos, Quaternion.Euler (0, 0, 0));	
					wasSomethingPut = true;
					break;
				} 
					
				
			}
			if (!wasSomethingPut) {
				Instantiate (tester, lastPos, Quaternion.Euler (0, 0, 0));	
			}
		}
		Instantiate (EndLine, new Vector3 (myTransform.position.x, myTransform.position.y, 0), Quaternion.Euler (0, 0, 90));
		lastPos = myTransform.position;
		lastQuat = Quaternion.Euler (0, 0, 90);
	}

	private void MueveS (Transform myTransform) {

		myTransform.position += new Vector3 (0, -1, 0);

		Lines[] listLines = FindObjectsOfType<Lines> ();
		foreach (Lines g in listLines) {
			if (g.transform.position == lastPos) {
				Destroy (g.gameObject);
				break;
			}
		}

		if (lastQuat == Quaternion.Euler (0, 0, 0) && lastPos != myAllie.transform.position) {
			Instantiate (Line, lastPos, Quaternion.Euler (0, 0, 0));	
		} else {
			bool wasSomethingPut = false;
			Lines[] listLines2 = FindObjectsOfType<Lines> ();
			foreach (Lines g in listLines2) {
				if (g.transform.position.x == myTransform.position.x - 1 && g.transform.position.y == myTransform.position.y + 1 && lastPos != myAllie.transform.position) {
					Instantiate (IzAba, lastPos, Quaternion.Euler (0, 0, 0));
					wasSomethingPut = true;
					break;
				} else if (g.transform.position.x == myTransform.position.x + 1 && g.transform.position.y == myTransform.position.y + 1 && lastPos != myAllie.transform.position) {
					Instantiate (DerAba, lastPos, Quaternion.Euler (0, 0, 0));	
					wasSomethingPut = true;
					break;
				} 
					

			}
			if (!wasSomethingPut) {
				Instantiate (tester, lastPos, Quaternion.Euler (0, 0, 0));
			}
		}
		Instantiate (EndLine, new Vector3 (myTransform.position.x, myTransform.position.y, 0), Quaternion.Euler (0, 0, 0));
		lastPos = myTransform.position;
		lastQuat = Quaternion.Euler (0, 0, 0);
	}


	// Genera un camino desde la posicion actual hasta las coordenadas indicadas.
	private void AutogeneraciondeCaminos (int Xcord, int Ycord) {
		
		int myAllieX = Mathf.FloorToInt (myAllie.transform.position.x);
		int myAllieY = Mathf.FloorToInt (myAllie.transform.position.y);
		List<Spot> path = new List<Spot> ();

		if (myMap.isPathFinding (myAllieX, myAllieY, Xcord, Ycord, myAllie.movement, path)) { // si existe un path updatea las variables

			Lines[] listLines = FindObjectsOfType<Lines> ();
			foreach (Lines g in listLines) {
				Destroy (g.gameObject);
			}

			counter = 0;
			lastPos = myAllie.transform.position;
			lastQuat = Quaternion.Euler (2, 2, 2);
			auxCursor.transform.position = myAllie.transform.position;
			path.Reverse ();
		


			for (int i = 1; i < path.Count; i++) {

				if (path [i].x > path [i - 1].x) {
					MueveD (auxCursor.transform);
				}
				if (path [i].x < path [i - 1].x) {
					MueveA (auxCursor.transform);
				}
				if (path [i].y > path [i - 1].y) {
					MueveW (auxCursor.transform);
				}
				if (path [i].y < path [i - 1].y) {
					MueveS (auxCursor.transform);
				}
			}

			// decodifica para poner con muves

			counter = path [path.Count - 1].g; // valor de la g
		}
		//************
		// Genera las lineas de movimiento.
		//************
	}

	private void GeneracionDeLineas () {

		if (Input.GetKeyDown (KeyCode.A) && state < 2) { 
			MoverCursorEffect.Play ();
			isRectifying = false;
			Lines[] listLines = FindObjectsOfType<Lines> (); // Mira si donde esta el cursor hay una linea y la elimina.
			foreach (Lines g in listLines) {
				if (g.transform.position == transform.position) {
					Destroy (g.gameObject);
					counter--;
					if (myMap.matrixScenary [cordX] [cordY].terrainType == Types.Forest) {
						counter--;
					} else if (myMap.matrixScenary [cordX] [cordY].terrainType == Types.Mountain) {
						counter -= 3;
					}
					isRectifying = true;
					break;
				}
			}
														
			Lines[] listLines2 = FindObjectsOfType<Lines> ();	// Mira si en la anterior posicion habia una linea y la elimina.
			foreach (Lines g in listLines2) {
				if (g.transform.position == lastPos) {
					Destroy (g.gameObject);
					break;
				}
			}

			if (isRectifying) { // si se esta rectificando 
				bool isTriggered = false;
				Lines[] listLines3 = FindObjectsOfType<Lines> ();
				foreach (Lines g in listLines3) {
					if (g.transform.position.x == lastPos.x - 1 && g.transform.position.y == lastPos.y && g.name == "Line(Clone)" && Quaternion.Euler (0, 0, 90) == g.transform.rotation) { // si en laspos esta linea simple , giro de un tipo, y del otro
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 90);
						Instantiate (EndLine, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 90));
						isTriggered = true;
						break;
					} else if (g.transform.position.x == lastPos.x - 1 && g.transform.position.y == lastPos.y && g.name == "DerArr(Clone)") {
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 0);
						Instantiate (EndLine, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 0));
						isTriggered = true;
						break;
					} else if (g.transform.position.x == lastPos.x - 1 && g.transform.position.y == lastPos.y && g.name == "DerAba(Clone)") {
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 0);
						Instantiate (EndLineArr, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 0));
						isTriggered = true;
						break;
					}
				}
				if (!isTriggered) { 
					AutogeneraciondeCaminos (cordX, cordY);
				}
			}

//			if(counter >= myAllie.movement){
//				isCountPlus1 = true;
//			}else{
//				isCountPlus1 = false;
//			}

			if (counter <= myAllie.movement && !isRectifying) {	// mira que tiene que poner en la anterior posicion.
				counter++;
				if (myMap.matrixScenary [cordX] [cordY].terrainType == Types.Forest) {
					counter++;
				} else if (myMap.matrixScenary [cordX] [cordY].terrainType == Types.Mountain) {
					counter += 3;
				}

				if (lastQuat == Quaternion.Euler (0, 0, 90) && lastPos != myAllie.transform.position) {			
					Instantiate (Line, lastPos, Quaternion.Euler (0, 0, 90));	
				} else {
					bool wasSomethingPut = false;
					Lines[] listLines3 = FindObjectsOfType<Lines> ();
					foreach (Lines g in listLines3) {
						if (g.transform.position.x == transform.position.x + 1 && g.transform.position.y == transform.position.y + 1 && lastPos != myAllie.transform.position) {
							Instantiate (IzArr, lastPos, Quaternion.Euler (0, 0, 0));
							wasSomethingPut = true;
							break;
						} else if (g.transform.position.x == transform.position.x + 1 && g.transform.position.y == transform.position.y - 1 && lastPos != myAllie.transform.position) {
							Instantiate (IzAba, lastPos, Quaternion.Euler (0, 0, 0));
							wasSomethingPut = true;
							break;
						} 
					}
					if (!wasSomethingPut) {
						Instantiate (tester, lastPos, Quaternion.Euler (0, 0, 0));
					}
				}

				// pone en esta posicion.
				Instantiate (EndLineArr, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 90));
				lastPos = transform.position;
				lastQuat = Quaternion.Euler (0, 0, 90);
			}

			isCountPlus1 = counter > myAllie.movement;

			if ((transform.position == myAllie.transform.position || isCountPlus1 || wasRecentlyOut)) {
				AutogeneraciondeCaminos (cordX, cordY);
				wasRecentlyOut = false;
			}

		}

		if (Input.GetKeyDown (KeyCode.W) && state < 2) {
			MoverCursorEffect.Play ();
			isRectifying = false;

			Lines[] listLines = FindObjectsOfType<Lines> (); // Mira si donde esta el cursor hay una linea y la elimina.
			foreach (Lines g in listLines) {
				if (g.transform.position == transform.position) {
					Destroy (g.gameObject);
					counter--;
					if (myMap.matrixScenary [cordX] [cordY].terrainType == Types.Forest) {
						counter--;
					} else if (myMap.matrixScenary [cordX] [cordY].terrainType == Types.Mountain) {
						counter -= 3;
					}
					isRectifying = true;
					break;
				}
			}

																		
			Lines[] listLines2 = FindObjectsOfType<Lines> (); 	// Mira si en la anterior posicion habia una linea y la elimina.
			foreach (Lines g in listLines2) {
				if (g.transform.position == lastPos) {
					Destroy (g.gameObject);
					break;
				}
			}

			if (isRectifying) { // si se esta rectificando 
				bool isTriggered = false;
				Lines[] listLines3 = FindObjectsOfType<Lines> ();	
				foreach (Lines g in listLines3) {
					if (g.transform.position.x == lastPos.x && g.transform.position.y == lastPos.y + 1 && g.name == "Line(Clone)" && Quaternion.Euler (0, 0, 0) == g.transform.rotation) { // si en laspos esta linea simple , giro de un tipo, y del otro
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 0);
						Instantiate (EndLine, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 0));
						isTriggered = true;
						break;
					} else if (g.transform.position.x == lastPos.x && g.transform.position.y == lastPos.y + 1 && g.name == "IzAba(Clone)") {
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 90);
						Instantiate (EndLine, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 90));
						isTriggered = true;
						break;
					} else if (g.transform.position.x == lastPos.x && g.transform.position.y == lastPos.y + 1 && g.name == "DerAba(Clone)") {
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 90);
						Instantiate (EndLineArr, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 90));
						isTriggered = true;
						break;
					}
				}
				if (!isTriggered) { 
					AutogeneraciondeCaminos (cordX, cordY);
				}
			}


			if (counter <= myAllie.movement && !isRectifying) {
				counter++;

				if (myMap.matrixScenary [cordX] [cordY].terrainType == Types.Forest) {
					counter++;
				} else if (myMap.matrixScenary [cordX] [cordY].terrainType == Types.Mountain) {
					counter += 3;
				}
				if (lastQuat == Quaternion.Euler (0, 0, 0) && lastPos != myAllie.transform.position) {
					Instantiate (Line, lastPos, Quaternion.Euler (0, 0, 0));	
				} else {
					bool wasSomethingPut = false;
					Lines[] listLines3 = FindObjectsOfType<Lines> ();
					foreach (Lines g in listLines3) {
						if (g.transform.position.x == transform.position.x - 1 && g.transform.position.y == transform.position.y - 1 && lastPos != myAllie.transform.position) {
							Instantiate (IzArr, lastPos, Quaternion.Euler (0, 0, 0));
							wasSomethingPut = true;
							break;
						} else if (g.transform.position.x == transform.position.x + 1 && g.transform.position.y == transform.position.y - 1 && lastPos != myAllie.transform.position) {
							Instantiate (DerArr, lastPos, Quaternion.Euler (0, 0, 0));
							wasSomethingPut = true;
							break;	
						} 
					}
					if (!wasSomethingPut) {
						Instantiate (tester, lastPos, Quaternion.Euler (0, 0, 0));	
					}

				}
				Instantiate (EndLineArr, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 0));
				lastPos = transform.position;
				lastQuat = Quaternion.Euler (0, 0, 0);
			}

			isCountPlus1 = counter > myAllie.movement;
			if (transform.position == myAllie.transform.position || isCountPlus1 || wasRecentlyOut) {
				AutogeneraciondeCaminos (cordX, cordY);
				wasRecentlyOut = false;
			}
		}

		if (Input.GetKeyDown (KeyCode.D) && state < 2) {
			MoverCursorEffect.Play ();
			isRectifying = false;
			Lines[] listLines = FindObjectsOfType<Lines> ();
			foreach (Lines g in listLines) {
				if (g.transform.position == transform.position) {
					Destroy (g.gameObject);
					counter--;

					if (myMap.matrixScenary [cordX] [cordY].terrainType == Types.Forest) {
						counter--;
					} else if (myMap.matrixScenary [cordX] [cordY].terrainType == Types.Mountain) {
						counter -= 3;
					}
					isRectifying = true;
					break;
				}
			}
				
			Lines[] listLines2 = FindObjectsOfType<Lines> ();
			foreach (Lines g in listLines2) {
				if (g.transform.position == lastPos) {
					Destroy (g.gameObject);
					break;
				}
			}
			
			if (isRectifying) { // si se esta rectificando 
				bool isTriggered = false;
				Lines[] listLines3 = FindObjectsOfType<Lines> ();	
				foreach (Lines g in listLines3) {
					if (g.transform.position.x == lastPos.x + 1 && g.transform.position.y == lastPos.y && g.name == "Line(Clone)" && Quaternion.Euler (0, 0, 90) == g.transform.rotation) { // si en laspos esta linea simple , giro de un tipo, y del otro
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 90);
						Instantiate (EndLineArr, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 90));
						isTriggered = true;
						break;
					} else if (g.transform.position.x == lastPos.x + 1 && g.transform.position.y == lastPos.y && g.name == "IzArr(Clone)") {
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 0);
						Instantiate (EndLine, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 0));
						isTriggered = true;
						break;
					} else if (g.transform.position.x == lastPos.x + 1 && g.transform.position.y == lastPos.y && g.name == "IzAba(Clone)") {
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 0);
						Instantiate (EndLineArr, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 0));
						isTriggered = true;
						break;
					}
				}
				if (!isTriggered) { 
					AutogeneraciondeCaminos (cordX, cordY);
				}
			}



			if (counter <= myAllie.movement && !isRectifying) {
				counter++;

				if (myMap.matrixScenary [cordX] [cordY].terrainType == Types.Forest) {
					counter++;
				} else if (myMap.matrixScenary [cordX] [cordY].terrainType == Types.Mountain) {
					counter += 3;
				}
				if (lastQuat == Quaternion.Euler (0, 0, 90) && lastPos != myAllie.transform.position) {
					Instantiate (Line, lastPos, Quaternion.Euler (0, 0, 90));	
				} else {
					bool wasSomethingPut = false;
					Lines[] listLines3 = FindObjectsOfType<Lines> ();
					foreach (Lines g in listLines3) {
						if (g.transform.position.x == transform.position.x - 1 && g.transform.position.y == transform.position.y + 1 && lastPos != myAllie.transform.position) {
							Instantiate (DerArr, lastPos, Quaternion.Euler (0, 0, 0));
							wasSomethingPut = true;
							break;
						} else if (g.transform.position.x == transform.position.x - 1 && g.transform.position.y == transform.position.y - 1 && lastPos != myAllie.transform.position) {
							Instantiate (DerAba, lastPos, Quaternion.Euler (0, 0, 0));	
							wasSomethingPut = true;
							break;
						} 
					
				
					}
					if (!wasSomethingPut) {
						Instantiate (tester, lastPos, Quaternion.Euler (0, 0, 0));	
					}
				}
				Instantiate (EndLine, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 90));
				lastPos = transform.position;
				lastQuat = Quaternion.Euler (0, 0, 90);
			}

			isCountPlus1 = counter > myAllie.movement;
			if (transform.position == myAllie.transform.position || isCountPlus1 || wasRecentlyOut) {

				AutogeneraciondeCaminos (cordX, cordY);
				wasRecentlyOut = false;
			}
		}

		if (Input.GetKeyDown (KeyCode.S) && state < 2) {
			MoverCursorEffect.Play ();
			isRectifying = false;
		
			Lines[] listLines = FindObjectsOfType<Lines> ();
			foreach (Lines g in listLines) {
				if (g.transform.position == transform.position) {
					Destroy (g.gameObject);
					counter--;

					if (myMap.matrixScenary [cordX] [cordY].terrainType == Types.Forest) {
						counter--;
					} else if (myMap.matrixScenary [cordX] [cordY].terrainType == Types.Mountain) {
						counter -= 3;
					}
					isRectifying = true;
					break;
				}
			}

			Lines[] listLines2 = FindObjectsOfType<Lines> ();
			foreach (Lines g in listLines2) {
				if (g.transform.position == lastPos) {
					Destroy (g.gameObject);
					break;
				}
			}
			
			if (isRectifying) { // si se esta rectificando 
				bool isTriggered = false;
				Lines[] listLines3 = FindObjectsOfType<Lines> ();
				foreach (Lines g in listLines3) {
					if (g.transform.position.x == lastPos.x && g.transform.position.y == lastPos.y - 1 && g.name == "Line(Clone)" && Quaternion.Euler (0, 0, 0) == g.transform.rotation) { // si en laspos esta linea simple , giro de un tipo, y del otro
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 0);
						Instantiate (EndLineArr, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 0));
						isTriggered = true;
						break;
					} else if (g.transform.position.x == lastPos.x && g.transform.position.y == lastPos.y - 1 && g.name == "IzArr(Clone)") {
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 90);
						Instantiate (EndLine, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 90));
						isTriggered = true;
						break;
					} else if (g.transform.position.x == lastPos.x && g.transform.position.y == lastPos.y - 1 && g.name == "DerArr(Clone)") {
						lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						lastQuat = Quaternion.Euler (0, 0, 90);
						Instantiate (EndLineArr, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 90));
						isTriggered = true;
						break;
					}
				}
				if (!isTriggered) { 
					AutogeneraciondeCaminos (cordX, cordY);
				}
			}

			if (counter <= myAllie.movement && !isRectifying) {
				counter++;

				if (myMap.matrixScenary [cordX] [cordY].terrainType == Types.Forest) {
					counter++;
				} else if (myMap.matrixScenary [cordX] [cordY].terrainType == Types.Mountain) {
					counter += 3;
				}
				if (lastQuat == Quaternion.Euler (0, 0, 0) && lastPos != myAllie.transform.position) {
					Instantiate (Line, lastPos, Quaternion.Euler (0, 0, 0));	
				} else {
					bool wasSomethingPut = false;
					Lines[] listLines3 = FindObjectsOfType<Lines> ();
					foreach (Lines g in listLines3) {
						if (g.transform.position.x == transform.position.x - 1 && g.transform.position.y == transform.position.y + 1 && lastPos != myAllie.transform.position) {
							Instantiate (IzAba, lastPos, Quaternion.Euler (0, 0, 0));
							wasSomethingPut = true;
							break;
						} else if (g.transform.position.x == transform.position.x + 1 && g.transform.position.y == transform.position.y + 1 && lastPos != myAllie.transform.position) {
							Instantiate (DerAba, lastPos, Quaternion.Euler (0, 0, 0));	
							wasSomethingPut = true;
							break;
						} 
					

					}
					if (!wasSomethingPut) {
						Instantiate (tester, lastPos, Quaternion.Euler (0, 0, 0));
					}
				}
				Instantiate (EndLine, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.Euler (0, 0, 0));
				lastPos = transform.position;
				lastQuat = Quaternion.Euler (0, 0, 0);
			}
			isCountPlus1 = counter > myAllie.movement;

			if (transform.position == myAllie.transform.position || isCountPlus1 || wasRecentlyOut) {

				AutogeneraciondeCaminos (cordX, cordY);
				wasRecentlyOut = false;
			}
		}
	}
}
