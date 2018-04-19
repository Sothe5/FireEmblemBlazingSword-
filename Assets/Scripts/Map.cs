using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour{


	public Spot[][] matrixScenary;

	public int Right;
	public int Up;


	void Start (){

		matrixScenary = new Spot[Right + 1][];

		for (int i = 0; i <= Right; i++){
			matrixScenary [i] = new Spot[Up + 1];
			for (int j = 0; j <= Up; j++){
				matrixScenary [i] [j] = gameObject.AddComponent<Spot> (); // == new Spot();
				matrixScenary [i] [j].SetPoint (i, j); // dudoso
				matrixScenary [i] [j].setType (Types.Free);
				matrixScenary [i] [j].setPersonType (null);
				matrixScenary [i] [j].setTerrainType (Types.Free);
				matrixScenary [i] [j].platformSprite = Resources.Load<Sprite> ("Platforms/llano");
			}
		}
	}

	public string GetValue (int a, int b){
		return matrixScenary [a] [b].personType;
	}

	public void SetValue (int a, int b, string c){
		matrixScenary [a] [b].personType = c;
	}


	public string GetValueScenary (int a, int b){
		return matrixScenary [a] [b].type;
	}

	public string GetTerrainValue (int a, int b){
		return matrixScenary [a] [b].terrainType;
	}

	public void EndTurnAllie (){
		Allie[] listAllies = FindObjectsOfType<Allie> ();
		foreach (Allie a in listAllies){
			a.setDisponibility (true);
		}
	}

	public void EndTurnEnemy (){
		Enemy[] listEnemies = FindObjectsOfType<Enemy> ();
		foreach (Enemy a in listEnemies){
			a.setDisponibility (true);
		}
	}


	private void RemoveFromArray (Spot[] myArray, Spot elem, int pos){ // quita de open set el spot prometedor		*****Plantear hacer quicksort y despues binary search

		for (int i = pos - 1; i >= 0; i--){ // por como esta definida deberia hacerlo de princi
			if (myArray [i].Equals (elem)){
				for (int j = i; j < myArray.Length - 1; j++){
					myArray [j] = myArray [j + 1];
				}
				break;			
			}
		} 
	}

	private bool isIncluded (Spot a, Spot[] list,int pos){

		for (int i = 0; i < pos; i++){
			if (list [i].Equals (a)){
				return true;
			}
		}

		return false;
	}

	private int heuristic (Spot a, Spot b){
		return Mathf.Abs (a.x - b.x) + Mathf.Abs (a.y - b.y);
	}

	private int distance (Allie myAllies, Spot start){

		int myAllieX = Mathf.FloorToInt (myAllies.transform.position.x);
		int myAllieY = Mathf.FloorToInt (myAllies.transform.position.y);

		return Mathf.Abs ((myAllieX - start.x) + (myAllieY - start.y));
	}

	private int distBetween (Spot a, Spot b){
		if (b.terrainType == Types.Forest){
			return 2;
		}else if (b.terrainType == Types.Mountain){ 
			return 4;
		}else{
			return 1;
		}
	}


	public bool isPathFinding (int startX, int startY, int endX, int endY, int movement, List<Spot> path){

		int posInOpen = 0;
		int posInClose = 0;


		Spot start = gameObject.AddComponent<Spot> (); // start and end generated
		Spot end = gameObject.AddComponent<Spot> ();
		start.SetPoint (startX, startY);
		end.SetPoint (endX, endY);
		start.setType (Types.Free);
		end.setType (Types.Free);

		Spot[] openSet;	//open and close set generated and initiallized
		Spot[] closeSet;
		openSet = new Spot[(Right + 1) * (Up + 1)];
		closeSet = new Spot[(Right + 1) * (Up + 1)];
		openSet [posInOpen] = start;
		posInOpen++;
		bool ok = false;

		while (posInOpen > 0 && !ok){
			
			int prometedor = 0; // buscamos de la lista de los posibles neighbors cual es el que tiene una f mas baja
			for (int i = 0; i < posInOpen; i++){ 
				if (openSet [i].getF () < openSet [prometedor].getF ()){ // la distancia entre inicio y prometedor es menor que myallie.movement 		&& distance(myAllie,openSet[i]) < myAllie.movement
					prometedor = i;

				}
			}

			Spot current = openSet [prometedor];
			if (current.Equals (end)){ // si el prometedor es el final

				path.Add (current);
				while (current.previous){
					path.Add (current.previous);
					current = current.previous;
				}
				ok = true;
				if (path [0].g > movement){ // se deberia comparar la g con el movement de myAllie
					Destroy (start);
					Destroy (end);
					return false;
				}
				Destroy (start);
				Destroy (end);
				return ok;
			}

			RemoveFromArray (openSet, current, posInOpen);
			System.Array.Resize<Spot> (ref openSet, openSet.Length - 1);
			posInOpen--;
			closeSet [posInClose] = current; // añadimos a los visitados el nodo prometedor
			posInClose++;

			List<Spot> neighborsOfPrometedor = current.getNeighbors(matrixScenary, Right, Up);

			foreach (Spot spot in neighborsOfPrometedor){
				if (!isIncluded (spot, closeSet,posInClose) && spot.type != Types.Ocupaid){ 

					int TempG = current.g + distBetween (current, spot); // lo que cuesta llegar al neighbor
					bool newPath = false;
					if (isIncluded (spot, openSet,posInOpen)){
						if (TempG < spot.g){
							spot.g = TempG;
							newPath = true;
						}
					} else{
						spot.g = TempG;
						newPath = true;
						openSet [posInOpen] = spot;
						posInOpen++;
					}
					if (newPath){
						spot.h = heuristic (spot, end);		// neighborsOfPrometedor[i] = neighbor
						spot.f = spot.g + spot.h;
						spot.previous = current;
					}
				}
			}


		}

		Destroy (start);
		Destroy (end);
		return ok;

		// cuando saliese seria no solucion

	}

	public List<Spot> Dijkstra(List<Spot> graph, Spot start,int movement){
		List<Spot> visited = new List<Spot>();
		List<Spot> remainings = new List<Spot> ();

		foreach(Spot a in graph){
			a.distancia = int.MaxValue;
		}
		start.distancia = 0;
		remainings.Add (start);
		while(remainings.Count != 0){
			Spot prometedor = new Spot();
			prometedor.distancia = int.MaxValue;
			foreach (Spot a in remainings) {
				if (a.distancia < prometedor.distancia) {
					prometedor = a;
				}
			}
			visited.Add (prometedor);
			foreach (Spot neighbour in prometedor.getNeighbors(matrixScenary,Right,Up)) {
				if(graph.Contains(neighbour) && !visited.Contains(neighbour) && neighbour.distancia > prometedor.distancia+distBetween(prometedor,neighbour)){
					neighbour.distancia = prometedor.distancia + distBetween (prometedor, neighbour);
					if (neighbour.distancia <= movement) {
						remainings.Add (neighbour);
					}
				}
			}
			remainings.Remove (prometedor);
		}

		return visited;
	}
}
