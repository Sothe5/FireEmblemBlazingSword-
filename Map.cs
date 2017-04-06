using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	
	public Spot [][] matrixScenary;

	public int Right;
	public int Up;


	void Start () {

		matrixScenary = new Spot[Right+1][];

		for(int i = 0; i <= Right; i++){
			matrixScenary[i] = new Spot[Up+1];
			for(int j = 0; j <= Up; j++){
				matrixScenary[i][j] = gameObject.AddComponent<Spot>(); // == new Spot();
				matrixScenary[i][j].SetPoint(i,j); // dudoso
				matrixScenary[i][j].setType(Types.Free);
				matrixScenary[i][j].setPersonType(null);
			}
		}

		for(int i = 0; i <= Right; i++){	// mejorar eficiencia
			for(int j = 0; j <= Up; j++){
				matrixScenary[i][j].addNeighbors(matrixScenary,Right,Up);
			}
		}

	}

	public string GetValue(int a,int b){
		return matrixScenary[a][b].personType;
	}

	public void SetValue(int a, int b, string c){
		matrixScenary[a][b].personType = c;
	}


	public string GetValueScenary(int a,int b){
		return matrixScenary[a][b].type;
	}

	public void EndTurn(){
		Object[] objs = Allie.FindSceneObjectsOfType(typeof(Allie));
		foreach (Allie a in objs){
			a.setDisponibility(true);
		}
	}


	private void RemoveFromArray (Spot[] myArray, Spot elem,int pos){ // quita de open set el spot prometedor		*****Plantear hacer quicksort y despues binary search

		for(int i = pos-1; i >= 0; i--){ // por como esta definida deberia hacerlo de princi
			if(myArray[i].equal(elem)){
				for(int j = i; j< myArray.Length-1; j++){
					myArray[j] = myArray[j+1];
				}
				break;			
			}
		} 
	}

	private bool isIncluded (Spot a, Spot[] list,int pos){

		for(int i = 0; i < pos; i++){
			if(list[i].equal(a)){
				return true;
			}
		}

		return false;
	}

	private int heuristic (Spot a, Spot b){
		return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y -b.y);
	}

	private int distance (Allie myAllies,Spot start){

		int myAllieX = Mathf.FloorToInt(myAllies.transform.position.x);
		int myAllieY = Mathf.FloorToInt(myAllies.transform.position.y);

		return Mathf.Abs((myAllieX-start.x)+ (myAllieY - start.y));
	}

	private int distBetween (Spot a,Spot b){
		// cambiar free de cursor a ocuppied con sus implicaciones
		if(b.type == Types.Forest){
		return 2;
		}else{
		return 1;
		}
	}


	public bool isPathFinding(int startX , int startY, int endX, int endY,Allie myAllie,Spot [] path,Spot posInPath){
		
		int posInOpen = 0;
		int posInClose = 0;


		Spot start = gameObject.AddComponent<Spot>(); // start and end generated
		Spot end = gameObject.AddComponent<Spot>();
		start.SetPoint(startX,startY);
		end.SetPoint(endX,endY);
		start.setType(Types.Free);
		end.setType(Types.Free);

		start.addNeighbors(matrixScenary,Right,Up);
		end.addNeighbors(matrixScenary,Right,Up);

		Spot [] openSet;	//open and close set generated and initiallized
		Spot [] closeSet;
		openSet = new Spot[(Right+1)*(Up+1)];
		closeSet = new Spot[(Right+1)*(Up+1)];
		openSet[posInOpen]  = start;
		posInOpen++;
		bool ok = false;

		while (posInOpen > 0 && !ok){

			int prometedor = 0; // buscamos de la lista de los posibles neighbors cual es el que tiene una f mas baja
			for(int i = 0; i < posInOpen; i++){ 
				if(openSet[i].getF() < openSet[prometedor].getF() && distance(myAllie,openSet[i]) < myAllie.movement){ // la distancia entre inicio y prometedor es menor que myallie.movement
					prometedor = i;

				}
			}

			Spot current = openSet[prometedor];
			if(current.equal(end)){ // si el prometedor es el final

				 // intentar reducirlo despues
				

				Spot temp = current;
				path[posInPath.x] = temp;
				posInPath.x++;

				while(temp.previous){
					path[posInPath.x] = temp.previous;
					posInPath.x++;
					temp = temp.previous;

				}

				ok = true;
				if(path[0].g > myAllie.movement){ // se deberia comparar la g con el movement de myAllie
					Destroy(start);
					Destroy(end);
					return false;
				}
				Destroy(start);
				Destroy(end);
				return ok;
			}

			RemoveFromArray(openSet,current,posInOpen);
			System.Array.Resize<Spot>(ref openSet,openSet.Length-1);
			posInOpen--;
			closeSet[posInClose] = current; // añadimos a los visitados el nodo prometedor
			posInClose++;


			Spot [] neighborsOfPrometedor = current.getNeighbors();

			for(int i = 0; i < current.numNeigh; i++){
				
				if(!isIncluded(neighborsOfPrometedor[i], closeSet,posInClose) && neighborsOfPrometedor[i].type != Types.Ocupaid){ 
					
					int TempG = current.g + distBetween(current,neighborsOfPrometedor[i]); // lo que cuesta llegar al neighbor
					bool newPath = false;
					if(isIncluded(neighborsOfPrometedor[i], openSet,posInOpen)){
						if(TempG < neighborsOfPrometedor[i].g){
							neighborsOfPrometedor[i].g = TempG;
							newPath = true;
						}
					}else{
						neighborsOfPrometedor[i].g = TempG;
						newPath = true;
						openSet[posInOpen] = neighborsOfPrometedor[i];
						posInOpen++;
					}
					if(newPath){
						neighborsOfPrometedor[i].h = heuristic(neighborsOfPrometedor[i],end);		// neighborsOfPrometedor[i] = neighbor
						neighborsOfPrometedor[i].f = neighborsOfPrometedor[i].g +neighborsOfPrometedor[i].h;
						neighborsOfPrometedor[i].previous = current;
					}
				}
			}


		}

		Destroy(start);
		Destroy(end);
		return ok;

		// cuando saliese seria no solucion

	}



}
